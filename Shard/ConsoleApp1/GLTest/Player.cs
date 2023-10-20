using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using OpenTK.Input;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Graphics.OpenGL;
using Shard.Shard.Graphics;
using Shard.Shard.GameObjects;

namespace Shard.GLTest
{
    class Player : GameObject
    {
        public Player() : base()
        {
            AddComponent(new PlayerComponent(this));
        }
    }
}
