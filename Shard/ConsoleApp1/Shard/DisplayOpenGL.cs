using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;

namespace Shard
{
    class DisplayOpenGL : Display
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

        private DisplayOpenGL() {
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

        public override void clearDisplay()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            Window.ProcessWindowEvents();
        }

        public void preDraw()
        {
            if (MainCamera != null)
            {
                View = MainCamera.GetViewMatrix();
                Projection = MainCamera.GetProjMatrix();
            }
        }

        public override void display()
        {
            Window.Display();
        }

        public override void initialize()
        {

            Window = new WindowGL(Color4.Black);
            Window.Initialize();
        }

        public override void showText(string text, double x, double y, int size, int r, int g, int b)
        {
            //throw new NotImplementedException();
        }

        public override void showText(char[,] text, double x, double y, int size, int r, int g, int b)
        {
            //throw new NotImplementedException();
        }
    }
}
