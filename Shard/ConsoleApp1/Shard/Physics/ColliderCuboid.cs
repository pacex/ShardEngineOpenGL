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

        public ColliderCuboid(Box3 bounds, ColliderMask mask = ColliderMask.Player)
        {
            this.bounds = bounds;
            Position = Vector3.Zero;

            this.mask = mask;
        }

        public override Collider CopyOffset(Vector3 offset)
        {
            ColliderCuboid c = new ColliderCuboid(Bounds, Mask);
            c.Position = Position + offset;
            return c;
        }

        public override bool Intersects(Collider other)
        {
            if (TranslatedBounds().Contains(other.TranslatedBounds()) && (Mask & other.Mask) != 0x0)
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
            static float nonNeg(float a)
            {
                return a >= 0.0f ? a : float.MaxValue; 
            }

            if (!Intersects(other))
                return Vector3.Zero;

            if (other is ColliderCuboid)
            {
                Box3 t1, t2;
                t1 = TranslatedBounds();
                t2 = other.TranslatedBounds();

                float[] edgeToEdge = {  nonNeg(t2.Max.X - t1.Min.X), nonNeg(t1.Max.X - t2.Min.X),
                                        nonNeg(t2.Max.Y - t1.Min.Y), nonNeg(t1.Max.Y - t2.Min.Y),
                                        nonNeg(t2.Max.Z - t1.Min.Z), nonNeg(t1.Max.Z - t2.Min.Z) };

                float minElement = edgeToEdge.Min();
                int minIndex = Array.IndexOf(edgeToEdge, minElement);

                switch (minIndex)
                {
                    case 0:
                        return -Vector3.UnitX * minElement;
                    case 1:
                        return Vector3.UnitX * minElement;
                    case 2:
                        return -Vector3.UnitY * minElement;
                    case 3:
                        return Vector3.UnitY * minElement;
                    case 4:
                        return -Vector3.UnitZ * minElement;
                    case 5:
                        return Vector3.UnitZ * minElement;
                }
                
                return Vector3.Zero;
            }
            else
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
