using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.GL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using System.ComponentModel;

namespace Shard
{
    class WindowGL : NativeWindow
    {
        private Color4 clearColor;

        Shader shader;

        public WindowGL(Color4 clearColor) : base(NativeWindowSettings.Default)
        {
            this.clearColor = clearColor;
        }

        public void Initialize()
        {
            base.Context?.MakeCurrent();
            OnResize(new ResizeEventArgs(base.Size));
            GL.ClearColor(clearColor);


            shader = new Shader("Shaders/default.vert", "Shaders/default.frag");
        }

        public void Display()
        {

            // Debug code Hello triangle
            float[] vertices = {
                -0.5f, -0.5f, 0.0f, //Bottom-left vertex
                 0.5f, -0.5f, 0.0f, //Bottom-right vertex
                 0.0f,  0.5f, 0.0f  //Top vertex
            };

            int vertexArrayObject;
            vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayObject);

            int vertexBufferObject;
            vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);



            shader.Use();
            GL.BindVertexArray(vertexArrayObject);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

            


            base.Context.SwapBuffers();
        }

        public void ProcessWindowEvents()
        {
            NativeWindow.ProcessWindowEvents(base.IsEventDriven);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            shader.Dispose();
        }

    }
}
