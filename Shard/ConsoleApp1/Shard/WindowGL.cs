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

namespace Shard
{
    class WindowGL : NativeWindow
    {
        private Color4 clearColor;

        public WindowGL(Color4 clearColor) : base(NativeWindowSettings.Default)
        {
            this.clearColor = clearColor;
        }

        public void Initialize()
        {
            base.Context?.MakeCurrent();
            OnResize(new ResizeEventArgs(base.Size));
            GL.ClearColor(clearColor);
        }

        public void Display()
        {

            // Debug code Hello triangle
            float[] vertices = {
                -0.5f, -0.5f, 0.0f, //Bottom-left vertex
                 0.5f, -0.5f, 0.0f, //Bottom-right vertex
                 0.0f,  0.5f, 0.0f  //Top vertex
            };

            int vertexBufferObject;
            vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

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

    }
}
