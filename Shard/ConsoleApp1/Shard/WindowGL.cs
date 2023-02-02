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


            shader = Shader.GetDefaultShader();
        }

        public void Display()
        {
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
