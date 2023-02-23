using Shard;

namespace MissileCommand
{
    class ArsenalSprite : GameObject
    {

        public override void initialize()
        {


            this.TransformOld.X = 200.0f;
            this.TransformOld.Y = 100.0f;
            this.TransformOld.SpritePath = Bootstrap.getAssetManager().getAssetPath("missile.png");

        }

        public override void update()
        {
            Bootstrap.getDisplay().addToDraw(this);
        }

    }
}
