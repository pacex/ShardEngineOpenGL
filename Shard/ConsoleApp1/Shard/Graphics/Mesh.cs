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

        private int vertexArrayObject;
        private int vertexBufferObject;

        private float[] vertices;
        private uint[] indices;

        public static void DrawFullscreenQuad()
        {
            Mesh quad = new Mesh();
            quad.Draw();
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

        public Mesh(float[] vert, uint[] ind, bool buildVao = true)
        {
            vertices = vert;
            indices = ind;

            if (!buildVao)
                return;

            vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayObject);

            vertexBufferObject = GL.GenBuffer();

            // Vertex Attributes: (0) vec3 pos, (1) vec3 normal, (2) vec4 color, (3) vec2 uv

            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));
            GL.EnableVertexAttribArray(2);

            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        public virtual void Draw()
        {
            GL.BindVertexArray(vertexArrayObject);
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, indices);
            GL.BindVertexArray(0);
        }

        public List<Collider> ExportTriangleColliders(Vector3 offset)
        {
            List<Collider> colliders = new List<Collider>();

            for (int i = 0; i < indices.Length; i += 3)
            {
                Vector3 v0, v1, v2;
                v0 = new Vector3(vertices[indices[i] * 8 + 0], vertices[indices[i] * 8 + 1], vertices[indices[i] * 8 + 2]);
                v1 = new Vector3(vertices[indices[i + 1] * 8 + 0], vertices[indices[i + 1] * 8 + 1], vertices[indices[i + 1] * 8 + 2]);
                v2 = new Vector3(vertices[indices[i + 2] * 8 + 0], vertices[indices[i + 2] * 8 + 1], vertices[indices[i + 2] * 8 + 2]);

                Collider c = new ColliderTriangle(v0, v1, v2);
                c.Position = offset;
                colliders.Add(c);
            }

            return colliders;
        }

        public AnimatedMesh ToAnimatedMesh(Texture texture, int frameCount, float animationSpeed)
        {
            return new AnimatedMesh(vertices, indices, texture, frameCount, animationSpeed);
        }
    }
}
