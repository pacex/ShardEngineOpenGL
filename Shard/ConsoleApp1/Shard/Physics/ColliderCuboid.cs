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
        public Box3 Bounds;

        public ColliderCuboid(Box3 bounds)
        {
            Bounds = bounds;
        }

        public override void Draw(Vector3 pos, Color4 col)
        {
            float[] vertices = new float[] {    Bounds.Min.X + pos.X, Bounds.Min.Y + pos.Y, Bounds.Min.Z + pos.Z,     Bounds.Min.X + pos.X, Bounds.Min.Y + pos.Y, Bounds.Max.Z + pos.Z,
                                                Bounds.Min.X + pos.X, Bounds.Max.Y + pos.Y, Bounds.Min.Z + pos.Z,     Bounds.Min.X + pos.X, Bounds.Max.Y + pos.Y, Bounds.Max.Z + pos.Z,
                                                Bounds.Max.X + pos.X, Bounds.Min.Y + pos.Y, Bounds.Min.Z + pos.Z,     Bounds.Max.X + pos.X, Bounds.Min.Y + pos.Y, Bounds.Max.Z + pos.Z,
                                                Bounds.Max.X + pos.X, Bounds.Max.Y + pos.Y, Bounds.Min.Z + pos.Z,     Bounds.Max.X + pos.X, Bounds.Max.Y + pos.Y, Bounds.Max.Z + pos.Z };

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
