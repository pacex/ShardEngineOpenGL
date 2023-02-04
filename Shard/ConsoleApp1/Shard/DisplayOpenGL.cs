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
        private WindowGL _window;

        public Matrix4 Model;
        public Matrix4 View;
        public Matrix4 Projection;


        public override void clearDisplay()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            _window.ProcessWindowEvents();
        }

        public override void display()
        {
            _window.Display();
        }

        public override void initialize()
        {

            _window = new WindowGL(Color4.Black);
            _window.Initialize();
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
