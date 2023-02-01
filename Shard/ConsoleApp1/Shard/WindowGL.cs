using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard
{
    class WindowGL : GameWindow
    {
        public WindowGL() : base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {
        }

        public void Initialize()
        {
            base.Context?.MakeCurrent();
            OnLoad();
            OnResize(new ResizeEventArgs(base.Size));
        }

        public void Display()
        {
            SwapBuffers();
        }

        public void ProcessWindowEvents()
        {
            NativeWindow.ProcessWindowEvents(base.IsEventDriven);
        }

    }
}
