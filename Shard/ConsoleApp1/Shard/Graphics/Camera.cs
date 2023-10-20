using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using Shard.Shard.GameObjects;

namespace Shard.Shard.Graphics
{
    class Camera : GameObject
    {
        public float Fovy;
        public Vector3 Up;
        public float Near, Far;


        public Camera() : base()
        {
            Fovy = 0.4f * (float)Math.PI;
            Up = Vector3.UnitZ;
            Near = 0.1f;
            Far = 256.0f;
        }

        public Camera(float fovy, Vector3 up, float near, float far) : base()
        {
            Fovy = fovy;
            Up = up;
            Near = near;
            Far = far;
        }

        public Matrix4 GetViewMatrix()
        {
            return Matrix4.LookAt(Transform.Translation, Transform.Translation + Transform.Forward, Up);
        }

        public Matrix4 GetProjMatrix(float aspect)
        {
            return Matrix4.CreatePerspectiveFieldOfView(Fovy, aspect, Near, Far);
        }

        public Matrix4 GetProjMatrix()
        {
            Vector2i windowSize = Bootstrap.Display.Window.Size;
            return GetProjMatrix(windowSize.X / (float)windowSize.Y);
        }

        public void SetAsMain()
        {
            DisplayOpenGL.GetInstance().MainCamera = this;
        }

        public Matrix4 GetBillboardRotMatrix(Vector2 pos)
        {
            Vector2 dir = (Transform.Translation.Xy - pos).Normalized();

            return Matrix4.CreateFromAxisAngle(Vector3.UnitZ, (float)Math.Acos(dir.Y));
        }
    }
}
