using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using Shard.Shard.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard.Shard.Physics
{
    class ColliderTriangle : Collider
    {
        private Vector3[] p;
        private Vector3 n;

        public ColliderTriangle(Vector3 p1, Vector3 p2, Vector3 p3) 
        {
            p = new Vector3[] {p1, p2, p3};
            n = Vector3.Cross(p2 - p1, p3 - p1).Normalized();

            bounds = new Box3(p1, p1);
            bounds.Inflate(p2);
            bounds.Inflate(p3);

            Position = Vector3.Zero;
        }

        public override Collider CopyOffset(Vector3 offset)
        {
            ColliderTriangle c = new ColliderTriangle(p[0], p[1], p[2]);
            c.Position = Position + offset;
            return c;
        }

        public override bool Intersects(Collider other)
        {
            if (TranslatedBounds().Contains(other.TranslatedBounds()))
            {
                if (other is ColliderCuboid)
                {
                    // Assume cuboid centered on origin
                    Vector3 v0, v1, v2, c, e;
                    c = other.Bounds.Center + other.Position;
                    v0 = p[0] + Position - c;
                    v1 = p[1] + Position - c;
                    v2 = p[2] + Position - c;
                    e = other.Bounds.HalfSize;

                    // Triangle edge vectors
                    Vector3 f0, f1, f2;
                    f0 = v1 - v0;
                    f1 = v2 - v1;
                    f2 = v0 - v2;

                    // Cuboid face normals
                    Vector3 u0, u1, u2;
                    u0 = Vector3.UnitX;
                    u1 = Vector3.UnitY;
                    u2 = Vector3.UnitZ;

                    // Potential seperating axes
                    Vector3[] axes = new Vector3[] {    n, u0, u1, u2,
                                                        Vector3.Cross(u0, f0), Vector3.Cross(u0, f1), Vector3.Cross(u0, f2),
                                                        Vector3.Cross(u1, f0), Vector3.Cross(u1, f1), Vector3.Cross(u1, f2),
                                                        Vector3.Cross(u2, f0), Vector3.Cross(u2, f1), Vector3.Cross(u2, f2)};

                    for(int i = 0; i < axes.Length; i++)
                    {
                        Vector3 axis = axes[i];
                        float r =   e.X * Math.Abs(Vector3.Dot(u0, axis)) +
                                    e.Y * Math.Abs(Vector3.Dot(u1, axis)) +
                                    e.Z * Math.Abs(Vector3.Dot(u2, axis));

                        float p0 = Vector3.Dot(v0, axis);
                        float p1 = Vector3.Dot(v1, axis);
                        float p2 = Vector3.Dot(v2, axis);

                        if (Math.Max(-Math.Max(p0, Math.Max(p1, p2)), Math.Min(p0, Math.Min(p1, p2))) > r)
                        {
                            return false;
                        }
                    }
                    return true;
                }
                else
                    throw new NotImplementedException();
            }
            return false;
        }

        public override Vector3 Response(Collider other)
        {
            return Vector3.Zero;
        }

        public override void Draw(Color4 col)
        {
            float[] vertices = new float[] {    p[0].X + Position.X, p[0].Y + Position.Y, p[0].Z + Position.Z,     
                                                p[1].X + Position.X, p[1].Y + Position.Y, p[1].Z + Position.Z,
                                                p[2].X + Position.X, p[2].Y + Position.Y, p[2].Z + Position.Z};

            uint[] indices = new uint[] { 0, 1, 0, 2, 1, 2 };

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
