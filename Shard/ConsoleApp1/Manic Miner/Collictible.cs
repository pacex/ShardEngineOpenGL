using Shard;
using System;
using System.Drawing;

namespace ManicMiner
{
    class Collectible : GameObject, CollisionHandler
    {

        public override void initialize()
        {
            setPhysicsEnabled();
            
            addTag ("Collectible");
            MyBody.addRectCollider((int)TransformOld.X, (int)TransformOld.Y, 10, 10);
            MyBody.PassThrough = true;

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
            if (x.Parent.checkTag ("MinerWilly")) {
                this.ToBeDestroyed = true;
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
            return "Collectible: [" + TransformOld.X + ", " + TransformOld.Y + "]";
        }


    }
}
