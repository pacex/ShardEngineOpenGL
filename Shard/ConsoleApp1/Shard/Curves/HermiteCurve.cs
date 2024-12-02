using OpenTK.Compute.OpenCL;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using Shard.Shard.Graphics;
using Shard.Shard.Sound;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard.Shard.Curves
{
    class HermiteCurve
    {
        public HermiteCurve(Vector3 p0, Vector3 v0, Vector3 p1, Vector3 v1)
        {
            P0 = p0;
            V0 = v0;
            P1 = p1;
            V1 = v1;
        }

        public Vector3 P0 { get; private set; }
        public Vector3 V0 { get; private set; }
        public Vector3 P1 { get; private set; }
        public Vector3 V1 { get; private set; }


        public Vector3 GetPosition(float t)
        {
            float tSquared = t * t;
            float tCubed = tSquared * t;
            return tCubed * (2 * P0 + V0 - 2 * P1 + V1) + tSquared * (-3 * P0 - 2 * V0 + 3 * P1 - V1) + t * V0 + P0;
        }

        public Vector3 GetTangent(float t)
        {
            float tSquared = t * t;
            return 3 * tSquared * (V0 + V1 + 2 * P0 - P1) - 2 * t * (2 * V0 + V1 + 3 * P0 - 3 * P1) + V0;
        }

        public void Draw(Color4 col, int n = 16)
        {
            float[] vertices = new float[(n + 1) * 3];
            uint[] indices = new uint[n * 2];


            for (uint i = 0; i <= n; i++)
            {
                float t = i / (float)n;
                Vector3 p = GetPosition(t);
                vertices[i * 3 + 0] = p.X; vertices[i * 3 + 1] = p.Y; vertices[i * 3 + 2] = p.Z;
                if (i < n)
                {
                    indices[i * 2 + 0] = i; indices[i * 2 + 1] = i + 1;
                }
            }

            int vertexBufferObject, vertexArrayObject;
            vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayObject);
            vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(0);
            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            DisplayOpenGL.GetInstance().Model = Matrix4.Identity;

            Shader.ApplyWireframeShader(col);
            GL.BindVertexArray(vertexArrayObject);
            GL.Disable(EnableCap.DepthTest);
            GL.DrawElements(PrimitiveType.Lines, indices.Length, DrawElementsType.UnsignedInt, indices);
            GL.BindVertexArray(0);
            GL.Enable(EnableCap.DepthTest);
            Shader.Reset();

            GL.DeleteBuffer(vertexBufferObject);
            GL.DeleteVertexArray(vertexArrayObject);
        }
    }
}
