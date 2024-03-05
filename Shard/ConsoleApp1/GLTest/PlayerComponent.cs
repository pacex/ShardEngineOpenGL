using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Shard.Shard.GameObjects;
using Shard.Shard.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard.GLTest
{
    internal class PlayerComponent : Component
    {
        private float height;
        private Camera camera;
        private float sensitivity;

        private Player player;

        public PlayerComponent(GameObject parent) : base(parent)
        {

        }

        public override void Initialize()
        {
            player = (Player)Host;

            // Camera Setup
            camera = new Camera();
            camera.SetAsMain();
            height = 1.6f;

            sensitivity = 0.002f;

            camera.Transform.Translation = Host.Transform.Translation + Vector3.UnitZ * height;
        }

        public override void Update()
        {
            // Interpolate camera position
            Vector3 goal = Host.Transform.Translation + Vector3.UnitZ * height;
            Vector3 current = camera.Transform.Translation;

            camera.Transform.Translation = current + (goal - current) * 0.5f;

            // Update Camera rotation
            Vector2 mouseDelta = DisplayOpenGL.GetInstance().Window.MouseState.Delta;
            camera.Transform.Rotate(Quaternion.FromAxisAngle(Vector3.UnitZ, mouseDelta.X * sensitivity));

            Vector3 euler = camera.Transform.Rotation.ToEulerAngles();
            float vrot = -mouseDelta.Y * sensitivity;
            if (Math.Abs(euler.Y + vrot) < Math.PI / 2.0f)
            {
                camera.Transform.Rotate(Quaternion.FromAxisAngle(camera.Transform.Left, -mouseDelta.Y * sensitivity));
            }

            // Player Movement
            Vector2 f = camera.Transform.Forward.Xy.Normalized();
            Vector2 l = camera.Transform.Left.Xy.Normalized();

            System.Numerics.Vector2 forward = new System.Numerics.Vector2(f.X, f.Y);
            System.Numerics.Vector2 left = new System.Numerics.Vector2(l.X, l.Y);

            float dist = 6f * Bootstrap.DeltaTime;
            bool hasMoved = false;
            Vector3 move = -Vector3.UnitZ * 0.03f;

            if (DisplayOpenGL.GetInstance().Window.IsKeyDown(Keys.W))
            {
                move += new Vector3(forward.X, forward.Y, 0.0f) * dist;
                hasMoved = true;
            }
            if (DisplayOpenGL.GetInstance().Window.IsKeyDown(Keys.S))
            {
                move += new Vector3(-forward.X, -forward.Y, 0.0f) * dist;
                hasMoved = true;
            }
            if (DisplayOpenGL.GetInstance().Window.IsKeyDown(Keys.A))
            {
                move += new Vector3(left.X, left.Y, 0.0f) * dist;
                hasMoved = true;
            }
            if (DisplayOpenGL.GetInstance().Window.IsKeyDown(Keys.D))
            {
                move += new Vector3(-left.X, -left.Y, 0.0f) * dist;
                hasMoved = true;
            }

            Host.GetComponent<Shard.Physics.DynamicBody>().MoveAndSlide(move);

        }

        public override void Draw()
        {

        }

        public override void OnDestroy()
        {
        }
    }
}
