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

    class Mesh
    {

        public float[] Vertices { get; protected set; }
        public uint[] Indices { get; protected set; }

        private static Mesh fullscreenQuad;
        private static int fQvao;
        private static int fQvbo;
        public static void DrawFullscreenQuad()
        {
            if (fullscreenQuad == null)
            {
                fullscreenQuad = new Mesh();
                fQvao = GL.GenVertexArray();
                GL.BindVertexArray(fQvao);

                fQvbo = GL.GenBuffer();

                // Vertex Attributes: (0) vec3 pos, (1) vec3 normal, (2) vec4 color, (3) vec2 uv
                GL.BindBuffer(BufferTarget.ArrayBuffer, fQvbo);
                GL.BufferData(BufferTarget.ArrayBuffer, fullscreenQuad.Vertices.Length * sizeof(float), fullscreenQuad.Vertices, BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
                GL.EnableVertexAttribArray(0);

                GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
                GL.EnableVertexAttribArray(1);

                GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));
                GL.EnableVertexAttribArray(2);

                GL.BindVertexArray(0);
                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            }

            GL.BindVertexArray(fQvao);
            GL.DrawElements(PrimitiveType.Triangles, fullscreenQuad.Indices.Length, DrawElementsType.UnsignedInt, fullscreenQuad.Indices);
            GL.BindVertexArray(0);
        }

        public Mesh() : this(new float[] {  // Vertices
                                1.0f,  1.0f,  0.0f,     0.0f, 0.0f, -1.0f,      1.0f, 1.0f, // 0
                                1.0f,  -1.0f, 0.0f,     0.0f, 0.0f, -1.0f,      1.0f, 0.0f, // 1
                                -1.0f, -1.0f, 0.0f,     0.0f, 0.0f, -1.0f,      0.0f, 0.0f, // 2
                                -1.0f, 1.0f,  0.0f,     0.0f, 0.0f, -1.0f,      0.0f, 1.0f  // 3
                            },
                            new uint[]{  // Indices
                                0, 3, 1,
                                1, 3, 2
                            })
        {
        }

        public Mesh(float[] vert, uint[] ind)
        {
            Vertices = vert;
            Indices = ind;
        }

        public Mesh(float[] vert, float[] texcoord, float[] normal, uint[] ind)
        {
            int nVerts = vert.Length / 3;
            if (texcoord.Length != nVerts * 2 || normal.Length != nVerts * 3)
            {
                throw new ArgumentException("Non-matching sizes of vertex attribute arrays!");
            }
            float[] combinedVerts = new float[nVerts * 8];

            for (int i = 0; i < nVerts; i++)
            {
                combinedVerts[8 * i] = vert[3 * i];
                combinedVerts[8 * i + 1] = vert[3 * i + 1];
                combinedVerts[8 * i + 2] = vert[3 * i + 2];

                combinedVerts[8 * i + 3] = normal[3 * i];
                combinedVerts[8 * i + 4] = normal[3 * i + 1];
                combinedVerts[8 * i + 5] = normal[3 * i + 2];

                combinedVerts[8 * i + 6] = texcoord[2 * i];
                combinedVerts[8 * i + 7] = texcoord[2 * i + 1];
            }

            Vertices = combinedVerts;
            Indices = ind;
        }

        public List<Collider> ExportTriangleColliders(Vector3 offset)
        {
            List<Collider> colliders = new List<Collider>();

            for (int i = 0; i < Indices.Length; i += 3)
            {
                Vector3 v0, v1, v2;
                v0 = new Vector3(Vertices[Indices[i] * 8 + 0], Vertices[Indices[i] * 8 + 1], Vertices[Indices[i] * 8 + 2]);
                v1 = new Vector3(Vertices[Indices[i + 1] * 8 + 0], Vertices[Indices[i + 1] * 8 + 1], Vertices[Indices[i + 1] * 8 + 2]);
                v2 = new Vector3(Vertices[Indices[i + 2] * 8 + 0], Vertices[Indices[i + 2] * 8 + 1], Vertices[Indices[i + 2] * 8 + 2]);

                Collider c = new ColliderTriangle(v0, v1, v2);
                c.Position = offset;
                colliders.Add(c);
            }

            return colliders;
        }

    }
}
