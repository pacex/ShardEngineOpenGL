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

        public WindowGL(Color4 clearColor) : base(NativeWindowSettings.Default)
        {
            this.clearColor = clearColor;
        }

        public void Initialize()
        {
            base.Context?.MakeCurrent();
            OnResize(new ResizeEventArgs(base.Size));
            GL.ClearColor(clearColor);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);
        }

        public void Display()
        {
            base.Context.SwapBuffers();
        }

        public void ProcessWindowEvents()
        {
            NativeWindow.ProcessWindowEvents(base.IsEventDriven);
        }

        public Vector2i GetWindowSize()
        {
            return base.Size;
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
        }

    }
}
