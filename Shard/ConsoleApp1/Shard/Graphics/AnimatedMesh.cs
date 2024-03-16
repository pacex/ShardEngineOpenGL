using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using Shard.Shard.Physics;

namespace Shard.Shard.Graphics
{

    class BoneNode
    {
        public List<BoneNode> Children { get; private set; }
        public Matrix4 Offset { get; private set; }
        public uint ID { get; private set; }
        public uint NumBones { get; private set; }

        public Matrix4 Transform { get; private set; }

        public BoneNode(List<BoneNode> children, Matrix4 offset, uint iD, Matrix4 transform)
        {
            Children = children;
            Offset = offset;
            ID = iD;

            uint n = 1;
            foreach (BoneNode child in Children)
                n += child.NumBones;
            NumBones = n;

            Transform = transform;
        }

        public void ComputeBoneMatrices(ref float[] boneMatrices, Matrix4 parent, Animation anim, float timestamp)
        {
            Matrix4 own = parent * anim.GetTransform(ID, timestamp);

            Matrix4 final = own * Offset;
            final.Transpose();

            boneMatrices[16 * ID + 0] = final.M11; boneMatrices[16 * ID + 1] = final.M12; boneMatrices[16 * ID + 2] = final.M13; boneMatrices[16 * ID + 3] = final.M14;
            boneMatrices[16 * ID + 4] = final.M21; boneMatrices[16 * ID + 5] = final.M22; boneMatrices[16 * ID + 6] = final.M23; boneMatrices[16 * ID + 7] = final.M24;
            boneMatrices[16 * ID + 8] = final.M31; boneMatrices[16 * ID + 9] = final.M32; boneMatrices[16 * ID + 10] = final.M33; boneMatrices[16 * ID + 11] = final.M34;
            boneMatrices[16 * ID + 12] = final.M41; boneMatrices[16 * ID + 13] = final.M42; boneMatrices[16 * ID + 14] = final.M43; boneMatrices[16 * ID + 15] = final.M44;

            foreach (BoneNode c in Children)
                c.ComputeBoneMatrices(ref boneMatrices, own, anim, timestamp);
        }
    }

    class Keyframe<T>
    {
        public float TimeStamp { get; private set; }
        public T Value { get; private set;}
        public Keyframe(float timeStamp, T value)
        {
            TimeStamp = timeStamp;
            Value = value;
        }
    }

    class Channel
    {
        public List<Keyframe<Vector3>> PosKeyframes;
        public List<Keyframe<Quaternion>> RotKeyframes;
        public List<Keyframe<Vector3>> ScaleKeyframes;

        public Channel()
        {
            PosKeyframes = new List<Keyframe<Vector3>>();
            RotKeyframes = new List<Keyframe<Quaternion>>();
            ScaleKeyframes = new List<Keyframe<Vector3>>();
        }
    }

    class Animation
    {
        public Channel[] Channels;

        public float Length { get; private set; }

        public Animation(int nBones, float length)
        {
            Channels = new Channel[nBones];
            for(int i = 0; i < nBones; i++)
            {
                Channels[i] = new Channel();
            }
            Length = length;
        }

        public void AddPosKeyframe(uint boneId, float timestamp, Vector3 posKey)
        {
            Keyframe<Vector3> keyframe = new Keyframe<Vector3>(timestamp, posKey);
            Channels[boneId].PosKeyframes.Add(keyframe);
        }

        public void AddRotKeyframe(uint boneId, float timestamp, Quaternion rotKey)
        {
            Keyframe<Quaternion> keyframe = new Keyframe<Quaternion>(timestamp, rotKey);
            Channels[boneId].RotKeyframes.Add(keyframe);
        }

        public void AddScaleKeyframe(uint boneId, float timestamp, Vector3 scaleKey)
        {
            Keyframe<Vector3> keyframe = new Keyframe<Vector3>(timestamp, scaleKey);
            Channels[boneId].ScaleKeyframes.Add(keyframe);
        }

        public Matrix4 GetTransform(uint boneId, float timeStamp)
        {
            Channel c = Channels[boneId];

            Keyframe<Vector3> kp0 = c.PosKeyframes[0], kp1 = c.PosKeyframes[0];
            for(int i = 0; i < c.PosKeyframes.Count-1; i++)
            {
                kp0 = c.PosKeyframes[i];
                kp1 = c.PosKeyframes[i + 1];
                if (c.PosKeyframes[i].TimeStamp <= timeStamp && c.PosKeyframes[i + 1].TimeStamp > timeStamp)
                    break;
                    
            }
            float w = (timeStamp - kp0.TimeStamp) / (kp1.TimeStamp - kp0.TimeStamp);
            w = Math.Clamp(w, 0.0f, 1.0f);
            Vector3 translation = w * kp1.Value + (1.0f - w) * kp0.Value;


            Keyframe<Quaternion> kr0 = c.RotKeyframes[0], kr1 = c.RotKeyframes[0];
            for (int i = 0; i < c.RotKeyframes.Count - 1; i++)
            {
                kr0 = c.RotKeyframes[i];
                kr1 = c.RotKeyframes[i + 1];
                if (c.RotKeyframes[i].TimeStamp <= timeStamp && c.RotKeyframes[i + 1].TimeStamp > timeStamp)
                    break;

            }
            w = (timeStamp - kr0.TimeStamp) / (kr1.TimeStamp - kr0.TimeStamp);
            w = Math.Clamp(w, 0.0f, 1.0f);
            Quaternion rotation = Quaternion.Slerp(kr0.Value, kr1.Value, w);
            //Quaternion rotation = Quaternion.Identity;

            Keyframe<Vector3> ks0 = c.ScaleKeyframes[0], ks1 = c.ScaleKeyframes[0];
            for (int i = 0; i < c.ScaleKeyframes.Count - 1; i++)
            {
                ks0 = c.ScaleKeyframes[i];
                ks1 = c.ScaleKeyframes[i + 1];
                if (c.ScaleKeyframes[i].TimeStamp <= timeStamp && c.ScaleKeyframes[i + 1].TimeStamp > timeStamp)
                    break;

            }
            w = (timeStamp - ks0.TimeStamp) / (ks1.TimeStamp - ks0.TimeStamp);
            w = Math.Clamp(w, 0.0f, 1.0f);
            Vector3 scale = w * ks1.Value + (1.0f - w) * ks0.Value;


            Matrix4 t = Matrix4.CreateTranslation(translation);
            t.Transpose();
            Matrix4 r = Matrix4.CreateFromQuaternion(rotation);
            Matrix4 s = Matrix4.CreateScale(scale);

            Matrix4 res = t * r * s;
            return res;
        }
    }

    class AnimatedMesh : Mesh
    {

        public BoneNode BoneHierarchy { get; private set; }
        public Animation Animation { get; private set; }

        public AnimatedMesh(float[] vert, float[] texcoord, float[] normal, float[] weights, uint[] boneIds, uint[] ind, BoneNode boneHierarchy, Animation animation)
        {
            int nVerts = vert.Length / 3;
            if (texcoord.Length != nVerts * 2 || normal.Length != nVerts * 3 || weights.Length != nVerts * 4 || boneIds.Length != nVerts * 4)
            {
                throw new ArgumentException("Non-matching sizes of vertex attribute arrays!");
            }
            float[] combinedVerts = new float[nVerts * 16];

            for (int i = 0; i < nVerts; i++)
            {
                combinedVerts[16 * i] = vert[3 * i];
                combinedVerts[16 * i + 1] = vert[3 * i + 1];
                combinedVerts[16 * i + 2] = vert[3 * i + 2];

                combinedVerts[16 * i + 3] = normal[3 * i];
                combinedVerts[16 * i + 4] = normal[3 * i + 1];
                combinedVerts[16 * i + 5] = normal[3 * i + 2];

                combinedVerts[16 * i + 6] = texcoord[2 * i];
                combinedVerts[16 * i + 7] = texcoord[2 * i + 1];

                combinedVerts[16 * i + 8] = weights[4 * i];
                combinedVerts[16 * i + 9] = weights[4 * i + 1];
                combinedVerts[16 * i + 10] = weights[4 * i + 2];
                combinedVerts[16 * i + 11] = weights[4 * i + 3];

                combinedVerts[16 * i + 12] = boneIds[4 * i];
                combinedVerts[16 * i + 13] = boneIds[4 * i + 1];
                combinedVerts[16 * i + 14] = boneIds[4 * i + 2];
                combinedVerts[16 * i + 15] = boneIds[4 * i + 3];
            }

            Vertices = combinedVerts;
            Indices = ind;
            BoneHierarchy = boneHierarchy;
            Animation = animation;
        }

    }
}
