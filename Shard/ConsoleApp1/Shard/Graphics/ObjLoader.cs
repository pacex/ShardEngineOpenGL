using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using Assimp;
using System.Xml.Serialization;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Graphics.OpenGL;

namespace Shard.Shard.Graphics
{
    static class ObjLoader
    {

        private static readonly string AssetParentDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.Parent.FullName;

        public static Mesh LoadMeshObj(string path)
        {

            List<float[]> pos = new List<float[]>();
            List<float[]> norm = new List<float[]>();
            List<float[]> uv = new List<float[]>();

            List<float> vertices = new List<float>();
            List<uint> indices = new List<uint>();

            uint index = 0;

            foreach (string line in File.ReadLines(AssetParentDirectory + "\\Assets\\" + path))
            {
                string[] words = line.Split(' ');

                if (words[0].Equals("v"))
                {
                    // Vertex
                    pos.Add(new float[] { float.Parse(words[1], CultureInfo.InvariantCulture), float.Parse(words[2], CultureInfo.InvariantCulture), float.Parse(words[3], CultureInfo.InvariantCulture) });
                }
                else if (words[0].Equals("vt"))
                {
                    // Texcoord
                    uv.Add(new float[] { float.Parse(words[1], CultureInfo.InvariantCulture), float.Parse(words[2], CultureInfo.InvariantCulture) });
                }
                else if (words[0].Equals("vn"))
                {
                    // Normal
                    norm.Add(new float[] { float.Parse(words[1], CultureInfo.InvariantCulture), float.Parse(words[2], CultureInfo.InvariantCulture), float.Parse(words[3], CultureInfo.InvariantCulture) });
                }
                else if (words[0].Equals("f"))
                {
                    // Face
                    for (int i = 1; i <= 3; i++)
                    {
                        string[] indStr = words[i].Split('/');
                        int[] ind = new int[] { int.Parse(indStr[0]),
                                                int.Parse(indStr[1]),
                                                int.Parse(indStr[2])};

                        vertices.AddRange(pos[ind[0] - 1]); // Append pos
                        vertices.AddRange(norm[ind[2] - 1]); // Append norm
                        vertices.AddRange(uv[ind[1] - 1]); // Append uv

                        indices.Add(index);
                        index++;

                    }
                }
            }

            return new Mesh(vertices.ToArray(), indices.ToArray());
        }

        public static Mesh LoadMeshGLTF(string path, int meshIndex = 0)
        {
            var importer = new AssimpContext();
            Scene scene = importer.ImportFile(AssetParentDirectory + "\\Assets\\" + path);

            if (scene != null && scene.HasMeshes)
            {
                Assimp.Mesh m = scene.Meshes[meshIndex];
                uint[] indices = m.GetUnsignedIndices();
                float[] vertices = buildArrayVec3(m.Vertices);
                float[] texcoords = buildArrayVec2(m.TextureCoordinateChannels[0]);
                float[] normals = buildArrayVec3(m.Normals);

                Mesh res = new Mesh(vertices, texcoords, normals, indices);
                return res;
            }
            else
            {
                throw new FileNotFoundException();
            }
        }

        public static AnimatedMesh LoadAnimatedMesh(string path, int meshIndex = 0)
        {
            var importer = new AssimpContext();
            Scene scene = importer.ImportFile(AssetParentDirectory + "\\Assets\\" + path);

            if (scene != null && scene.HasMeshes)
            {
                Assimp.Mesh m = scene.Meshes[meshIndex];
                int nVertices = m.Vertices.Count;

                uint[] indices = m.GetUnsignedIndices();
                float[] vertices = buildArrayVec3(m.Vertices);
                float[] texcoords = buildArrayVec2(m.TextureCoordinateChannels[0]);
                float[] normals = buildArrayVec3(m.Normals);

                float[] weights = new float[nVertices * 4];
                uint[] boneIds = new uint[nVertices * 4];

                if (!m.HasBones)
                    throw new ArgumentException("Animated mesh has to contain bones!");

                // Assign IDs to bone names
                Dictionary<string, uint> boneNameToID = new Dictionary<string, uint>();
                Dictionary<string, Matrix4> boneOffsets = new Dictionary<string, Matrix4>();
                uint id = 0;
                foreach (Bone b in m.Bones)
                {
                    boneNameToID.Add(b.Name, id++);
                    boneOffsets.Add(b.Name, ToOpenTKMatrix(b.OffsetMatrix));
                }
                int numBones = (int)id;

                // Load Bone Hierarchy
                BoneNode root = LoadBoneHierarchy(scene.RootNode, boneNameToID, boneOffsets);


                // Vertex bone weights
                List<Tuple<uint, float>>[] vertexBoneWeights = new List<Tuple<uint, float>>[nVertices];
                for (int i = 0; i < nVertices; i++)
                    vertexBoneWeights[i] = new List<Tuple<uint, float>>();

                foreach(Assimp.Bone b in m.Bones)
                {
                    foreach (Assimp.VertexWeight w in b.VertexWeights)
                    {
                        vertexBoneWeights[w.VertexID].Add(new Tuple<uint, float>(boneNameToID[b.Name], w.Weight));
                        if (vertexBoneWeights[w.VertexID].Count > 4)
                            Console.WriteLine("Warning: More than 4 bone weights per vertex not supported!");
                    }
                }

                for (int i = 0; i < nVertices; i++)
                {
                    int j = 0;
                    foreach (Tuple<uint,float> w in vertexBoneWeights[i])
                    {
                        weights[4 * i + j] = w.Item2;
                        boneIds[4 * i + j] = w.Item1;
                        j++;
                        if (j >= 4)
                            break;
                    }
                }

                // Load Animation
                Animation anim = new Animation(numBones, (float)scene.Animations[1].DurationInTicks);

                foreach (NodeAnimationChannel channel in scene.Animations[1].NodeAnimationChannels)
                {
                    if (!boneNameToID.ContainsKey(channel.NodeName))
                        continue;
                    uint boneId = boneNameToID[channel.NodeName];

                    foreach(VectorKey pos in channel.PositionKeys)
                    {
                        Vector3D p = pos.Value;
                        anim.AddPosKeyframe(boneId, (float)pos.Time, new Vector3(p.X, p.Y, p.Z));
                    }

                    foreach (QuaternionKey rot in channel.RotationKeys)
                    {
                        Assimp.Quaternion q = rot.Value;
                        anim.AddRotKeyframe(boneId, (float)rot.Time, new OpenTK.Mathematics.Quaternion(q.X, q.Y, q.Z, q.W));
                    }

                    foreach (VectorKey scale in channel.ScalingKeys)
                    {
                        Vector3D p = scale.Value;
                        anim.AddScaleKeyframe(boneId, (float)scale.Time, new Vector3(p.X, p.Y, p.Z));
                    }
                }

                AnimatedMesh res = new AnimatedMesh(vertices, texcoords, normals, weights, boneIds, indices, root, anim);
                return res;
            }
            else
            {
                throw new FileNotFoundException();
            }
        }

        private static BoneNode LoadBoneHierarchy(Node n, Dictionary<string,uint> boneNameToID, Dictionary<string,Matrix4> offsets)
        {
            if (boneNameToID.ContainsKey(n.Name))
            {
                List<BoneNode> children = new List<BoneNode>();

                foreach (Node c in n.Children)
                {
                    BoneNode childnode = LoadBoneHierarchy(c, boneNameToID, offsets);
                    if (childnode != null)
                        children.Add(childnode);
                }

                BoneNode bone = new BoneNode(children, offsets[n.Name], boneNameToID[n.Name], ToOpenTKMatrix(n.Transform));
                return bone;
            }
            else
            {
                if (!n.HasChildren)
                    return null;

                BoneNode passthrough;
                foreach (Node c in n.Children)
                {
                    passthrough = LoadBoneHierarchy(c, boneNameToID, offsets);
                    if (passthrough != null)
                        return passthrough;
                }
                return null;
            }
        }

        private static Matrix4 ToOpenTKMatrix(Assimp.Matrix4x4 m)
        {
            Matrix4 result = Matrix4.Identity;
            result.M11 = m.A1; result.M12 = m.A2; result.M13 = m.A3; result.M14 = m.A4;
            result.M21 = m.B1; result.M22 = m.B2; result.M23 = m.B3; result.M24 = m.B4;
            result.M31 = m.C1; result.M32 = m.C2; result.M33 = m.C3; result.M34 = m.C4;
            result.M41 = m.D1; result.M42 = m.D2; result.M43 = m.D3; result.M44 = m.D4;
            return result;
        }

        private static float[] buildArrayVec3(List<Assimp.Vector3D> vertices)
        {
            float[] result = new float[3 * vertices.Count];
            for(int i = 0; i < vertices.Count; i++)
            {
                result[3 * i] = vertices[i].X;
                result[3 * i + 1] = vertices[i].Y;
                result[3 * i + 2] = vertices[i].Z;
            }
            return result;
        }

        private static float[] buildArrayVec2(List<Assimp.Vector3D> vertices)
        {
            float[] result = new float[2 * vertices.Count];
            for (int i = 0; i < vertices.Count; i++)
            {
                result[2 * i] = vertices[i].X;
                result[2 * i + 1] = vertices[i].Y;
            }
            return result;
        }


    }
}
