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

namespace Shard
{
    static class ObjLoader
    {
        public static Mesh LoadMesh(string path) {

            List<float[]> pos = new List<float[]>();
            List<float[]> norm = new List<float[]>();
            List<float[]> uv = new List<float[]>();

            List<float> vertices = new List<float>();
            List<uint> indices = new List<uint>();

            uint index = 0;

            string assetParentDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.Parent.FullName;

            foreach (string line in File.ReadLines(assetParentDirectory + "\\Assets\\" + path))
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
                    for(int i = 1; i <= 3; i++)
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
    }
}
