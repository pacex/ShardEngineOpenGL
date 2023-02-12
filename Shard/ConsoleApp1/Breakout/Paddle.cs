using SDL2;

using Shard;

namespace GameBreakout
{
    class Paddle : GameObject, InputListener, CollisionHandler
    {
        bool left, right;
        int wid;


        public override void initialize()
        {

            this.TransformOld.X = 500.0f;
            this.TransformOld.Y = 800.0f;
            this.TransformOld.SpritePath = Bootstrap.getAssetManager().getAssetPath("test.png");
            this.TransformOld.Scaley = 0.5f;
            this.TransformOld.Scalex = 1.5f;


            Bootstrap.getInput().addListener(this);

            left = false;
            right = false;

            setPhysicsEnabled();

            MyBody.Mass = 1000;
            MyBody.MaxForce = 20;
            MyBody.Drag = 0.1f;

            MyBody.addRectCollider();

            addTag("Paddle");

            wid = Bootstrap.getDisplay().getWidth();
        }

        public void handleInput(InputEvent inp, string eventType)
        {



            if (eventType == "KeyDown")
            {
                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_D)
                {
                    right = true;
                }

                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_A)
                {
                    left = true;
                }

            }
            else if (eventType == "KeyUp")
            {
                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_D)
                {
                    right = false;
                }

                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_A)
                {
                    left = false;
                }


            }



        }

        public override void update()
        {
            Bootstrap.getDisplay().addToDraw(this);
        }

        public override void physicsUpdate()
        {

            double boundsx;

            if (left)
            {
                MyBody.addForce(this.TransformOld.Forward, -1 * 2000f);
            }


            if (right)
            {
                MyBody.addForce(this.TransformOld.Forward, 2000f);
            }


            if (this.TransformOld.X < 0)
            {
                this.TransformOld.translate(-1 * TransformOld.X, 0);
            }


            boundsx = wid - (this.TransformOld.X + this.TransformOld.Wid);

            if (boundsx < 0)
            {
                this.TransformOld.translate(boundsx, 0);
            }





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
            return "Paddle: [" + TransformOld.X + ", " + TransformOld.Y + ", " + TransformOld.Wid + ", " + TransformOld.Ht + "]";
        }

    }
}
