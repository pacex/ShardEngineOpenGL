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
                        0.5f,  0.5f, 0.0f,  // top right
                        0.5f, -0.5f, 0.0f,  // bottom right
                        -0.5f, -0.5f, 0.0f,  // bottom left
                        -0.5f,  0.5f, 0.0f   // top left
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

            int vertexBufferObject;
            vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);


            GL.VertexAttribPointer(Shader.GetDefaultShader().GetAttribLocation("aPosition"), 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

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
