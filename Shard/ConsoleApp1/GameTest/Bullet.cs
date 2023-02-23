using Shard;
using System;
using System.Drawing;

namespace GameTest
{
    class Bullet : GameObject, CollisionHandler
    {
        private Spaceship origin;

        public void setupBullet(Spaceship or, float x, float y)
        {
            this.TransformOld.X = x;
            this.TransformOld.Y = y;
            this.TransformOld.Wid = 10;
            this.TransformOld.Ht = 10;

            this.origin = or;

            setPhysicsEnabled();

            MyBody.addRectCollider((int)x, (int)y, 10, 10);

            addTag("Bullet");

            //            MyBody.addCircleCollider((int)x, (int)y, 5);

            MyBody.Mass = 100;
            MyBody.MaxForce = 50;
            //            MyBody.addTorque(0.001f);

            MyBody.PassThrough = true;

        }

        public override void initialize()
        {
            this.Transient = true;
        }

        public override void physicsUpdate()
        {
            MyBody.addForce(this.TransformOld.Forward, 100.0f);
        }

        public override void update()
        {
            Random r = new Random();
            Color col = Color.FromArgb(r.Next(0, 256), r.Next(0, 256), 0);


            Bootstrap.getDisplay().drawLine(
                (int)TransformOld.X,
                (int)TransformOld.Y,
                (int)TransformOld.X + 10,
                (int)TransformOld.Y + 10,
                col);

            Bootstrap.getDisplay().drawLine(
                (int)TransformOld.X + 10,
                (int)TransformOld.Y,
                (int)TransformOld.X,
                (int)TransformOld.Y + 10,
                col);


        }

        public void onCollisionEnter(PhysicsBody x)
        {
            if (x.Parent.checkTag("Spaceship") == false)
            {
                Debug.Log("Boom! " + x);
                ToBeDestroyed = true;
            }
        }

        public void onCollisionExit(PhysicsBody x)
        {
        }

        public void onCollisionStay(PhysicsBody x)
        {
        }

        public override string ToString()
        {
            return "Bullet: " + TransformOld.X + ", " + TransformOld.X;
        }
    }
}
