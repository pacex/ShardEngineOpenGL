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
        private Vector2 dim;

        public Wall(Vector2 size) : base() {
            dim = size;   
        }

        public void initPhys()
        {
            setPhysicsEnabled();
            MyBody.addCubeCollider(dim.X, dim.Y, 2.0f);
            MyBody.setKinematic();
        }

        public override void initialize()
        {
            base.initialize();
        }

        public override void drawUpdate()
        {
            base.drawUpdate();
        }
    }
}
