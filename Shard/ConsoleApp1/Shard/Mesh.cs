using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Shard
{

    class Mesh
    {

        private int vertexArrayObject;

        private float[] vertBuf;
        private float[] normBuf;
        private float[] colBuf;
        private float[] uvBuf;

        private uint[] indices;

        public Mesh()
        {
            /* CUBE (broken)
            Vector3[] vertices = new Vector3[] {
                new Vector3(-1, -1, -1),
                new Vector3(1, -1, -1),
                new Vector3(1, 1, -1),
                new Vector3(-1, 1, -1),
                new Vector3(-1, -1, 1),
                new Vector3(1, -1, 1),
                new Vector3(1, 1, 1),
                new Vector3(-1, 1, 1)
            };

            Vector2[] texcoords = new Vector2[]{
                new Vector2(0, 0),
                new Vector2(1, 0),
                new Vector2(1, 1),
                new Vector2(0, 1)
            };

            Vector3[] normals = new Vector3[] {
                new Vector3(0, 0, 1),
                new Vector3(1, 0, 0),
                new Vector3(0, 0, -1),
                new Vector3(-1, 0, 0),
                new Vector3(0, 1, 0),
                new Vector3(0, -1, 0)
            };

            indices = new uint[]{
                0, 1, 3, 3, 1, 2,
                1, 5, 2, 2, 5, 6,
                5, 4, 6, 6, 4, 7,
                4, 0, 7, 7, 0, 3,
                3, 2, 7, 7, 2, 6,
                4, 5, 0, 0, 5, 1
            };

            int[] texInds = new int[]{ 0, 1, 3, 3, 1, 2 };

            vertBuf = new float[18 * 6];
            for (int i = 0; i < 36; i++)
            {
                vertBuf[i * 3 + 0] = vertices[indices[i]].X;
                vertBuf[i * 3 + 1] = vertices[indices[i]].Y;
                vertBuf[i * 3 + 2] = vertices[indices[i]].Z;
            }

            colBuf = new float[24 * 6];
            for (int i = 0; i < 36; i++)
            {
                colBuf[i * 4 + 0] = 1.0f;
                colBuf[i * 4 + 1] = 1.0f;
                colBuf[i * 4 + 2] = 1.0f;
                colBuf[i * 4 + 3] = 1.0f;
            }

            uvBuf = new float[12 * 6];
            for (int i = 0; i < 36; i++)
            {
                uvBuf[i * 2 + 0] = texcoords[texInds[i % 4]].X;
                uvBuf[i * 2 + 1] = texcoords[texInds[i % 4]].Y;
            }

            normBuf = new float[18 * 6];
            for (int i = 0; i < 36; i++)
            {
                normBuf[i * 3 + 0] = normals[indices[i / 6]].X;
                normBuf[i * 3 + 1] = normals[indices[i / 6]].Y;
                normBuf[i * 3 + 2] = normals[indices[i / 6]].Z;
            }
            
            */

            vertBuf = new float[] {
                1.0f,  1.0f, 0.0f,  // top right
                 1.0f, -1.0f, 0.0f,  // bottom right
                -1.0f, -1.0f, 0.0f,  // bottom left
                -1.0f,  1.0f, 0.0f   // top left
            };

            normBuf = new float[] {
                0.0f,  0.0f, -1.0f,  // top right
                 0.0f, 0.0f, -1.0f,  // bottom right
                0.0f, 0.0f, -1.0f,  // bottom left
                0.0f,  0.0f, -1.0f   // top left
            };

            colBuf = new float[] {
                1.0f,  1.0f, 1.0f, 1.0f,  // top right
                1.0f, 1.0f, 1.0f, 1.0f,  // bottom right
                1.0f, 1.0f, 1.0f, 1.0f,  // bottom left
                1.0f,  1.0f, 1.0f, 1.0f   // top left
            };

            uvBuf = new float[] {
                1.0f,  1.0f,   // top right
                 1.0f, 0.0f,   // bottom right
                0.0f, 0.0f,   // bottom left
                0.0f,  1.0f,    // top left
            };

            indices = new uint[]{  // note that we start from 0!
                0, 1, 3,   // first triangle
                1, 2, 3    // second triangle
            };

            init();
        }

        private void init()
        {
            vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayObject);

            int[] vertexBufferObject = new int[4];
            GL.GenBuffers(4, vertexBufferObject);

            // Vertex Attributes: (0) vec3 pos, (1) vec3 normal, (2) vec4 color, (3) vec2 uv

            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject[0]);
            GL.BufferData(BufferTarget.ArrayBuffer, vertBuf.Length * sizeof(float), vertBuf, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject[1]);
            GL.BufferData(BufferTarget.ArrayBuffer, normBuf.Length * sizeof(float), normBuf, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(1);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject[2]);
            GL.BufferData(BufferTarget.ArrayBuffer, colBuf.Length * sizeof(float), colBuf, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(2, 4, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(2);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject[3]);
            GL.BufferData(BufferTarget.ArrayBuffer, uvBuf.Length * sizeof(float), uvBuf, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(3, 2, VertexAttribPointerType.Float, false, 0, 0);
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
