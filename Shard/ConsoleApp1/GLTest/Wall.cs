using Shard.Shard.GameObjects;
using Shard.Shard.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard.GLTest
{
    class Wall : GameObject
    {
        public Wall(Collider collider) : base()
        {
            AddComponent(new StaticBody(collider, this));
        }
    }
}
