using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using OpenTK.Input;

namespace Shard.GLTest
{
    class Player : GameObject
    {


        private float height;
        private Camera camera;
        private float sensitivity;

        public Player() : base()
        {
            
        }

        public override void initialize()
        {
            base.initialize();

            camera = new Camera();
            camera.SetAsMain();
            height = 1.2f;

            sensitivity = 0.002f;

            camera.Transform.Translation = Transform.Translation + Vector3.UnitZ * height;

            setPhysicsEnabled();

            MyBody.addRectCollider(0.6f, 0.6f);
            MyBody.ReflectOnCollision = false;
            MyBody.StopOnCollision = true;
        }

        public override void update() 
        {
            camera.Transform.Translation = Transform.Translation + Vector3.UnitZ * height;

            Vector2 mouseDelta = DisplayOpenGL.GetInstance().Window.MouseState.Delta;

            camera.Transform.Rotate(Quaternion.FromAxisAngle(Vector3.UnitZ, mouseDelta.X * sensitivity));
            camera.Transform.Rotate(Quaternion.FromAxisAngle(camera.Transform.Left, -mouseDelta.Y * sensitivity));

            

        }
        public override void physicsUpdate()
        {
            Vector2 f = camera.Transform.Forward.Xy.Normalized();
            Vector2 l = camera.Transform.Left.Xy.Normalized();

            System.Numerics.Vector2 forward = new System.Numerics.Vector2(f.X, f.Y);
            System.Numerics.Vector2 left = new System.Numerics.Vector2(l.X, l.Y);

            float force = 0.02f;

            if (DisplayOpenGL.GetInstance().Window.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.W))
            {
                MyBody.addForce(forward, force);
            }
            if (DisplayOpenGL.GetInstance().Window.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.S))
            {
                MyBody.addForce(-forward, force);
            }
            if (DisplayOpenGL.GetInstance().Window.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.A))
            {
                MyBody.addForce(left, force);
            }
            if (DisplayOpenGL.GetInstance().Window.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.D))
            {
                MyBody.addForce(-left, force);
            }
        }

        public override void drawUpdate()
        {
            base.drawUpdate();
            MyBody.debugDraw();
        }
    }
}
