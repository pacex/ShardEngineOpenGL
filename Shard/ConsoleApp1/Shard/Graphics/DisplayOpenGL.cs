using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;

namespace Shard.Shard.Graphics
{
    class DisplayOpenGL
    {

        private static DisplayOpenGL instance = null;

        public static DisplayOpenGL GetInstance()
        {
            if (instance == null)
            {
                instance = new DisplayOpenGL();
            }
            return instance;
        }

        private DisplayOpenGL()
        {
            Model = Matrix4.Identity;
            View = Matrix4.Identity;
            Projection = Matrix4.Identity;
            MainCamera = null;
        }

        public WindowGL Window { get; private set; }

        public Matrix4 Model;
        public Matrix4 View;
        public Matrix4 Projection;

        public Camera MainCamera;

        public void ClearDisplay()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        public void ProcessWindowEvents()
        {
            Window.ProcessWindowEvents();
        }

        public void PreDraw()
        {
            if (MainCamera != null)
            {
                View = MainCamera.GetViewMatrix();
                Projection = MainCamera.GetProjMatrix();
            }
        }

        public void Display()
        {
            Window.Display();
        }

        public void Initialize()
        {

            Window = new WindowGL(Color4.Black);
            Window.Initialize();
        }
    }
}
