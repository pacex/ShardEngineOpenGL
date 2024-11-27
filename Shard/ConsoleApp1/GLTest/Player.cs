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
using Shard.Shard.Physics;

namespace Shard.GLTest
{
    class Player : GameObject
    {
        public Player() : base()
        {
            AddComponent(new PlayerComponent(this));
            AddComponent(new DynamicBody(new ColliderCuboid(new Box3(-0.4f, -0.4f, 0.0f, 0.4f, 0.4f, 1.6f)), this));
        }
    }
}
