using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using Shard.Shard.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard.Shard.Physics
{
    class ColliderCuboid : Collider
    {

        public ColliderCuboid(Box3 bounds)
        {
            Bounds = bounds;
            Position = Vector3.Zero;
        }

        public override Collider CopyOffset(Vector3 offset)
        {
            ColliderCuboid c = new ColliderCuboid(Bounds);
            c.Position = Position + offset;
            return c;
        }

        public override bool Intersects(Collider other)
        {
            if (Bounds.Translated(Position).Contains(other.Bounds.Translated(other.Position)))
            {
                if (other is ColliderCuboid)
                    return true;
                else
                    throw new NotImplementedException();
            }
            return false;
        }

        public override Vector3 Response(Collider other)
        {
            throw new NotImplementedException();
        }

        public override void Draw(Color4 col)
        {
            float[] vertices = new float[] {    Bounds.Min.X + Position.X, Bounds.Min.Y + Position.Y, Bounds.Min.Z + Position.Z,     Bounds.Min.X + Position.X, Bounds.Min.Y + Position.Y, Bounds.Max.Z + Position.Z,
                                                Bounds.Min.X + Position.X, Bounds.Max.Y + Position.Y, Bounds.Min.Z + Position.Z,     Bounds.Min.X + Position.X, Bounds.Max.Y + Position.Y, Bounds.Max.Z + Position.Z,
                                                Bounds.Max.X + Position.X, Bounds.Min.Y + Position.Y, Bounds.Min.Z + Position.Z,     Bounds.Max.X + Position.X, Bounds.Min.Y + Position.Y, Bounds.Max.Z + Position.Z,
                                                Bounds.Max.X + Position.X, Bounds.Max.Y + Position.Y, Bounds.Min.Z + Position.Z,     Bounds.Max.X + Position.X, Bounds.Max.Y + Position.Y, Bounds.Max.Z + Position.Z };

            uint[] indices = new uint[] { 0, 1, 0, 2, 0, 4,
                                          1, 3, 1, 5,
                                          2, 3, 2, 6,
                                          3, 7,
                                          4, 5, 4, 6,
                                          5, 7,
                                          6, 7};

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
            GL.DrawElements(PrimitiveType.Lines, indices.Length, DrawElementsType.UnsignedInt, indices);
            GL.BindVertexArray(0);
            Shader.Reset();

            GL.DeleteBuffer(vertexBufferObject);
            GL.DeleteVertexArray(vertexArrayObject);
        }

    }
}
