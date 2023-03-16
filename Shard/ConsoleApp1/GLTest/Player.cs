using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using OpenTK.Input;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Graphics.OpenGL;

namespace Shard.GLTest
{
    class Player : GameObject
    {

        private float height;
        private Camera camera;
        private float sensitivity;

        private AnimatedMesh shotgun;

        private float acc;

        public Player() : base()
        {
            
        }

        public override void initialize()
        {
            base.initialize();

            // Camera Setup
            camera = new Camera();
            camera.SetAsMain();
            height = 0.6f;

            sensitivity = 0.002f;

            camera.Transform.Translation = Transform.Translation + Vector3.UnitZ * height;

            // Shotgun
            shotgun = new Mesh(new float[] {  // Vertices
                                0.2f,  0.2f,  0.0f,     0.0f, 0.0f, -1.0f,      1.0f, 1.0f,
                                0.2f,  -1.0f, 0.0f,     0.0f, 0.0f, -1.0f,      1.0f, 0.0f,
                                -1.0f, -1.0f, 0.0f,     0.0f, 0.0f, -1.0f,      0.0f, 0.0f,
                                -1.0f, 0.2f,  0.0f,     0.0f, 0.0f, -1.0f,      0.0f, 1.0f
                            },
                            new uint[]{  // Indices
                                0, 3, 1,
                                1, 3, 2
                            }).ToAnimatedMesh(new Texture("GLTest\\shotgun.png", TextureWrapMode.MirroredRepeat,
                            TextureMinFilter.Nearest, TextureMagFilter.Nearest, 0, 2), 11, 11.0f);
            shotgun.Mode = AnimationMode.LoopOnCall;

            // Player physics
            setPhysicsEnabled();
            MyBody.addCubeCollider(0.6f, 0.6f,1.2f);
            MyBody.Drag = 0.03f;
            MyBody.MaxForce = 0.22f;
            acc = 0.04f;
        }
        public void FireRifle(System.Numerics.Vector2 direction)
        {
            if (shotgun.GetAnimationProgress() > 0.0f)
            {
                return;
            }

            Bullet b = new Bullet(Transform.Translation);
            b.FireMe(direction);
            shotgun.StartAnimation();
        }
        public override void update() 
        {
            // Interpolate camera position
            Vector3 goal = Transform.Translation + Vector3.UnitZ * height;
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

            // Fire bullet
            if (DisplayOpenGL.GetInstance().Window.IsMouseButtonPressed(MouseButton.Left))
            {
                Vector2 f = camera.Transform.Forward.Xy.Normalized();
                System.Numerics.Vector2 forward = new System.Numerics.Vector2(f.X, f.Y);
                FireRifle(forward);
            }


        }
        public override void physicsUpdate()
        {
            // Player Movement
            Vector2 f = camera.Transform.Forward.Xy.Normalized();
            Vector2 l = camera.Transform.Left.Xy.Normalized();

            System.Numerics.Vector2 forward = new System.Numerics.Vector2(f.X, f.Y);
            System.Numerics.Vector2 left = new System.Numerics.Vector2(l.X, l.Y);

            float force = acc;

            if (DisplayOpenGL.GetInstance().Window.IsKeyDown(Keys.W))
            {
                MyBody.addForce(forward, force);
            }
            if (DisplayOpenGL.GetInstance().Window.IsKeyDown(Keys.S))
            {
                MyBody.addForce(-forward, force);
            }
            if (DisplayOpenGL.GetInstance().Window.IsKeyDown(Keys.A))
            {
                MyBody.addForce(left, force);
            }
            if (DisplayOpenGL.GetInstance().Window.IsKeyDown(Keys.D))
            {
                MyBody.addForce(-left, force);
            }
            
        }

        public override void drawUpdate()
        {
            base.drawUpdate();

            Matrix4 v = DisplayOpenGL.GetInstance().View;
            Matrix4 p = DisplayOpenGL.GetInstance().Projection;

            DisplayOpenGL.GetInstance().Projection = Matrix4.Identity;
            DisplayOpenGL.GetInstance().View = Matrix4.Identity;
            DisplayOpenGL.GetInstance().Model = Matrix4.Identity;
            shotgun.Draw();

            DisplayOpenGL.GetInstance().View = v;
            DisplayOpenGL.GetInstance().Projection = p;

        }
        public Vector3 getPlayerPos()
        {
            return Transform.Translation;
        }
    }
}
