using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace Shard
{
    enum MeshPreset
    {
        UnitCube,
        UnitQuad
    }

    class Mesh
    {

        private int vertexArrayObject;
        private uint[] indices;

        public Mesh(MeshPreset preset)
        {
            float[] vertices;

            switch (preset)
            {
                case MeshPreset.UnitCube:
                    vertices = new float[]
                    {
                        0.0f
                    };

                    indices = new uint[]
                    {

                    };
                    init(vertices);
                    break;

                case MeshPreset.UnitQuad:
                    vertices = new float[] {
                        0.5f, 0.5f, 0.0f,   0.0f, 0.0f, -1.0f,  1.0f, 1.0f, 0.0f, 1.0f,     1.0f, 1.0f,// top right
                        0.5f, -0.5f, 0.0f,  0.0f, 0.0f, -1.0f,  1.0f, 0.0f, 0.0f, 1.0f,     1.0f, 0.0f,// bottom right
                        -0.5f, -0.5f, 0.0f, 0.0f, 0.0f, -1.0f,  0.0f, 0.0f, 0.0f, 1.0f,     0.0f, 0.0f,// bottom left
                        -0.5f, 0.5f, 0.0f,  0.0f, 0.0f, -1.0f,  0.0f, 1.0f, 0.0f, 1.0f,     0.0f, 1.0f// top left
                    };

                    indices = new uint[] {
                        0, 1, 3,   // first triangle
                        1, 2, 3    // second triangle
                    };

                    init(vertices);
                    break;
            }

        }

        public Mesh(float[] vertices, uint[] indices)
        {
            init(vertices);
            this.indices = indices;
        }


        private void init(float[] vertices)
        {
            vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayObject);

            int[] vertexBufferObject = new int[4];
            GL.GenBuffers(4, vertexBufferObject);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject[0]);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            // Vertex Attributes: (0) vec3 pos, (1) vec3 normal, (2) vec4 color, (3) vec2 uv
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 12 * sizeof(float), 0 * sizeof(float));
            GL.EnableVertexAttribArray(0);

            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 12 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            GL.VertexAttribPointer(2, 4, VertexAttribPointerType.Float, false, 12 * sizeof(float), 6 * sizeof(float));
            GL.EnableVertexAttribArray(2);

            GL.VertexAttribPointer(3, 2, VertexAttribPointerType.Float, false, 12 * sizeof(float), 10 * sizeof(float));
            GL.EnableVertexAttribArray(3);

            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        public void Draw()
        {
            GL.BindVertexArray(vertexArrayObject);
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, indices);
            GL.BindVertexArray(0);
        }
    }
}
