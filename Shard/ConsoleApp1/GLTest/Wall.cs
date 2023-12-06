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
        private StaticBody staticBody;
        public StaticBody StaticBody { get { return staticBody; } }

        public Wall(Collider collider) : base()
        {
            staticBody = new StaticBody(collider, this);
            AddComponent(staticBody);
        }
    }
}
