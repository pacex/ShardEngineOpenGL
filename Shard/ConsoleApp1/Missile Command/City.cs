using Shard;

namespace MissileCommand
{
    class City : GameObject, CollisionHandler
    {

        public override void initialize()
        {
            this.TransformOld.SpritePath = Bootstrap.getAssetManager().getAssetPath("city.png");

            setPhysicsEnabled();

            MyBody.addCircleCollider(64, 64, 64);

            addTag("City");

            MyBody.Kinematic = true;

        }


        public override void update()
        {

            Bootstrap.getDisplay().addToDraw(this);

        }

        public void onCollisionEnter(PhysicsBody x)
        {

        }

        public void onCollisionExit(PhysicsBody x)
        {
        }

        public void onCollisionStay(PhysicsBody x)
        {
        }

        public override string ToString()
        {
            return "City: [" + TransformOld.X + ", " + TransformOld.Y + "]";
        }


    }
}
