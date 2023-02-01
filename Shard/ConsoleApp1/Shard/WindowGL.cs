using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard
{
    class WindowGL : NativeWindow
    {
        public WindowGL() : base(NativeWindowSettings.Default)
        {
        }

        public void Initialize()
        {
            base.Context?.MakeCurrent();
            OnResize(new ResizeEventArgs(base.Size));
        }

        public void Display()
        {
            base.Context.SwapBuffers();
        }

        public void ProcessWindowEvents()
        {
            NativeWindow.ProcessWindowEvents(base.IsEventDriven);
        }

    }
}
