using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;

namespace Shard
{
    class Transform3DNew
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
        public Quaternion Rotation { 
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
            set { 
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

        public Transform3DNew()
        {
            translation = Vector3.Zero;
            rotation = Quaternion.Identity;
            scale = Vector3.One;
            updateMatrix();
        }

        public Transform3DNew(Vector3 t, Quaternion r, Vector3 s)
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
            Matrix4 m;

            // Rotation
            Matrix4.CreateFromQuaternion(Rotation, out m);

            // Scale
            m = m * Matrix4.CreateScale(Scale);

            // Translation
            m.M41 = Translation.X; m.M42 = Translation.Y; m.M43 = Translation.Z;

            Matrix = m;
            InverseMatrix = m.Inverted();
        }
    }
}
