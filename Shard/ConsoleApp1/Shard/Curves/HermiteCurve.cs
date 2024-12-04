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
            computeArcDistanceLookup();
        }

        public HermiteCurve(HermiteCurve before, Vector3 p1, Vector3 v1)
        {
            P0 = before.P1;
            V0 = before.V1;
            P1 = p1;
            V1 = v1;
            computeArcDistanceLookup();
        }

        public HermiteCurve(Vector3 p0, Vector3 v0, HermiteCurve after)
        {
            P0 = p0;
            V0 = v0;
            P1 = after.P0;
            V1 = after.V0;
            computeArcDistanceLookup();
        }

        public HermiteCurve(HermiteCurve before, HermiteCurve after)
        {
            P0 = before.P1;
            V0 = before.V1;
            P1 = after.P0;
            V1 = after.V0;
            computeArcDistanceLookup();
        }

        public Vector3 P0 { get; private set; }
        public Vector3 V0 { get; private set; }
        public Vector3 P1 { get; private set; }
        public Vector3 V1 { get; private set; }

        public float ArcLength { get; private set; }

        private float[] tLookup;

        private void computeArcDistanceLookup(uint n = 64, uint m = 64)
        {
            float[] arcDistanceLookup = new float[m];
            Vector3 pPrev = P0;
            uint j;
            for (j = 1; j <= m; j++)
            {
                Vector3 p = GetPosition((float)j / (float)m);
                float step = (p - pPrev).Length;
                if (j > 1)
                    arcDistanceLookup[j - 1] = arcDistanceLookup[j - 2] + step;
                else
                    arcDistanceLookup[j - 1] = step;
                pPrev = p;
            }
            ArcLength = arcDistanceLookup[m - 1];

            tLookup = new float[n];
            j = 1;
            uint i = 1;
            while (j <= m)
            {
                float currentArcDistance = (i * ArcLength) / (float)n;
                while (arcDistanceLookup[j-1] >= currentArcDistance)
                {
                    float arcDPrev = j > 1 ? arcDistanceLookup[j - 2] : 0;
                    float alpha = (currentArcDistance - arcDPrev) / (arcDistanceLookup[j - 1] - arcDPrev);
                    tLookup[i - 1] = alpha * ((float)j / (float)m) + (1.0f - alpha) * ((float)(j-1) / (float)m);
                    i++;
                    currentArcDistance = (i * ArcLength) / (float)n;
                }

                j++;
            }
        }


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
