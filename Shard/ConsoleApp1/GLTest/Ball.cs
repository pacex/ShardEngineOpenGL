using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using Shard.Shard.GameObjects;
using Shard.Shard.Graphics;
using Shard.Shard.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard.GLTest
{
    class Ball : GameObject
    {
        public Ball() : base()
        {
            AddComponent(new RigidBody(this));
            AddComponent(new DynamicBody(new ColliderCuboid(new Box3(-0.3f, -0.3f, -0.3f, 0.3f, 0.3f, 0.3f)), this));
            AddComponent(new MeshRenderer(ObjLoader.LoadMeshObj("GLTest\\sphere.obj"),
                new Texture("GLTest\\texture_floor1.png", TextureWrapMode.MirroredRepeat, TextureMinFilter.NearestMipmapLinear, TextureMagFilter.Nearest, 0, 3), this));
        }
    }
}
