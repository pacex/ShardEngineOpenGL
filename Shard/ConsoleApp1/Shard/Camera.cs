﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;

namespace Shard
{
    class Camera : GameObject
    {
        public float Fovy;
        public Vector3 Up;
        public float Near, Far;


        public Camera() : base() {
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
            Vector2i windowSize = DisplayOpenGL.GetInstance().Window.Size;
            return GetProjMatrix((float)windowSize.X / (float)windowSize.Y);
        }

        public void SetAsMain()
        {
            DisplayOpenGL.GetInstance().MainCamera = this;
        }
    }
}