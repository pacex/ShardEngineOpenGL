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
        public Vector3 Translation;
        public Quaternion Rotation;
        public Vector3 Scale;

        public Transform3DNew()
        {
            Translation = Vector3.Zero;
            Rotation = Quaternion.Identity;
            Scale = Vector3.One;
        }

        public Transform3DNew(Vector3 t, Quaternion r, Vector3 s)
        {
            Translation = t;
            Rotation = r;
            Scale = s;
        }

        public void Rotate(Quaternion q)
        {
            Rotation *= q;
        }

        public Matrix4 ToMatrix()
        {
            Matrix4 m;

            // Rotation
            Matrix4.CreateFromQuaternion(Rotation, out m);

            // Scale
            m = m * Matrix4.CreateScale(Scale);

            // Translation
            m.M41 = Translation.X; m.M42 = Translation.Y; m.M43 = Translation.Z;

            return m;
        }
    }
}
