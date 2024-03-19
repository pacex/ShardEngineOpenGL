using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;

namespace Shard.Shard.GameObjects
{
    class Transform
    {
        public Vector3 Translation
        {
            get { return translation; }
            set
            {
                translation = value;
                updateMatrix();
            }
        }
        private Vector3 translation;
        public Quaternion Rotation
        {
            get { return rotation; }
            set
            {
                rotation = value;
                updateMatrix();
            }
        }
        private Quaternion rotation;
        public Vector3 Scale
        {
            get { return scale; }
            set
            {
                scale = value;
                updateMatrix();
            }
        }
        private Vector3 scale;

        public Vector3 Forward
        {
            get
            { return InverseMatrix.Row0.Xyz; }
        }
        public Vector3 Left
        {
            get
            { return InverseMatrix.Row1.Xyz; }
        }
        public Vector3 Up
        {
            get
            { return InverseMatrix.Row2.Xyz; }
        }


        public Matrix4 Matrix { get; private set; }
        public Matrix4 InverseMatrix { get; private set; }

        public Transform()
        {
            translation = Vector3.Zero;
            rotation = Quaternion.Identity;
            scale = Vector3.One;
            updateMatrix();
        }

        public Transform(Vector3 t, Quaternion r, Vector3 s)
        {
            translation = t;
            rotation = r;
            scale = s;
            updateMatrix();
        }


        public void Rotate(Quaternion q)
        {
            Rotation *= q;
        }

        public void Translate(Vector3 t)
        {
            Translation += t;
        }

        public Matrix4 ToMatrix()
        {
            return Matrix;
        }

        private void updateMatrix()
        {
            // Rotation
            Matrix4 rot = Matrix4.CreateFromQuaternion(Rotation);

            // Scale
            Matrix4 scale = Matrix4.CreateScale(Scale);

            // Translation
            Matrix4 trans = Matrix4.CreateTranslation(Translation);

            Matrix = trans * rot * scale;
            InverseMatrix = Matrix.Inverted();
        }
    }
}
