using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard.GLTest
{
    class Bullet: GameObject
    {
        private float acc;
        public Bullet(): base()
        {
            
        }
        public override void initialize()
        {
            base.initialize();
            MyBody.addSphereCollider(0.1f);
            MyBody.MaxForce = 10.0f;
            acc = 3.0f;
            setPhysicsEnabled();
        }
        public void FireMe(System.Numerics.Vector2 direction)
        {
            float force = acc;
            MyBody.addForce(direction, force);
        }
    }
}
