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

namespace Shard.Shard.Graphics
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
            Context?.MakeCurrent();
            VSync = VSyncMode.On;
            OnResize(new ResizeEventArgs(Size));
            GL.ClearColor(clearColor);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.LineWidth(4);
        }

        public void Display()
        {
            Context.SwapBuffers();
        }

        public void ProcessWindowEvents()
        {
            ProcessInputEvents();
            ProcessWindowEvents(IsEventDriven);
        }

        public Vector2i GetWindowSize()
        {
            return Size;
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
            DisplayOpenGL.GetInstance().Resize();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
        }

    }
}
