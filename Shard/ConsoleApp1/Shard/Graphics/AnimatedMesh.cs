using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

    class Channel<T>
    {
        public List<Keyframe<T>> Keyframes;
        public int LastIndex = 0;
    }

    class Animation
    {
        public Channel<Vector3>[] PosChannel { get; private set; }
        public Channel<Quaternion>[] RotChannel { get; private set; }
        public Channel<Vector3>[] ScaleChannel { get; private set; }

        public float Length { get; private set; }

        public Animation(int nBones, float length)
        {
            PosChannel = new Channel<Vector3>[nBones];
            RotChannel = new Channel<Quaternion>[nBones];
            ScaleChannel = new Channel<Vector3>[nBones];
            Length = length;
        }

        public void AddPosKeyframe(uint boneId, float timestamp, Vector3 posKey)
        {
            if (PosChannel[boneId] == null)
            {
                PosChannel[boneId] = new Channel<Vector3>();
                PosChannel[boneId].Keyframes = new List<Keyframe<Vector3>>();
            }
                

            Keyframe<Vector3> keyframe = new Keyframe<Vector3>(timestamp, posKey);
            PosChannel[boneId].Keyframes.Add(keyframe);
        }

        public void AddRotKeyframe(uint boneId, float timestamp, Quaternion rotKey)
        {
            if (RotChannel[boneId] == null)
            {
                RotChannel[boneId] = new Channel<Quaternion>();
                RotChannel[boneId].Keyframes = new List<Keyframe<Quaternion>>();
            }


            Keyframe<Quaternion> keyframe = new Keyframe<Quaternion>(timestamp, rotKey);
            RotChannel[boneId].Keyframes.Add(keyframe);
        }

        public void AddScaleKeyframe(uint boneId, float timestamp, Vector3 scaleKey)
        {
            if (ScaleChannel[boneId] == null)
            {
                ScaleChannel[boneId] = new Channel<Vector3>();
                ScaleChannel[boneId].Keyframes = new List<Keyframe<Vector3>>();
            }


            Keyframe<Vector3> keyframe = new Keyframe<Vector3>(timestamp, scaleKey);
            ScaleChannel[boneId].Keyframes.Add(keyframe);
        }

        public Matrix4 GetTransform(uint boneId, float timestamp)
        {
            Matrix4 t = Matrix4.CreateTranslation(PosChannel[boneId].Keyframes[0].Value);
            t.Transpose();
            Matrix4 r = Matrix4.CreateFromQuaternion(RotChannel[boneId].Keyframes[0].Value);
            Matrix4 s = Matrix4.CreateScale(ScaleChannel[boneId].Keyframes[0].Value);
            return t * r * s;
        }

        private int findIndex<T>(Channel<T> channel, float timeStamp)
        {
            int n = channel.Keyframes.Count;

            if (n == 0)
                return 0;

            for (int i = 0; i < n; i++)
            {
                int index = (i + channel.LastIndex) % n;
                // TODO: complete index search
            }

            return 0;

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
