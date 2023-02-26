using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;

namespace Shard.GLTest
{
    class Wall : GameObject
    {
        private Vector2 size = Vector2.One;

        public Wall(Vector2 size) : base() {
            this.size = size;
        }

        public override void initialize()
        {
            base.initialize();
            setPhysicsEnabled();
            MyBody.addRectCollider(size.X, size.Y);
            MyBody.Kinematic = false;
        }
    }
}
