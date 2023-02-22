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

            sensitivity = 0.01f;

            camera.Transform.Translation = Transform.Translation + Vector3.UnitZ * height;
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
            base.physicsUpdate();
        }

        public override void drawUpdate()
        {
            base.drawUpdate();
        }
    }
}
