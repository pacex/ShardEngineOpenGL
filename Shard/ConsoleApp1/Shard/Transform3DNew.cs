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

            /*
            Quaternion q = Rotation;

            float x = q.X, y = q.Y, z = q.Z, w = q.W;
            float xx = x*x, yy = y*y, zz = z*z, ww= w*w;

            // Rotation
            m.M11 = 1.0f - 2.0f*yy - 2.0f*zz;   m.M12 = 2.0f*x*y - 2.0f*w*z;        m.M13 = 2.0f*x*z + 2.0f*w*y;
            m.M21 = 2.0f*x*y + 2.0f*w*z;        m.M22 = 1.0f - 2.0f*xx - 2.0f*zz;   m.M23 = 2.0f*y*z - 2.0f*w*x;
            m.M31 = 2.0f*x*z - 2.0f*w*y;        m.M32 = 2.0f*x*y + 2.0f*w*z;        m.M33 = 1.0f - 2.0f*xx - 2.0f*yy;
            */

            Matrix4.CreateFromQuaternion(Rotation,out m);

            // Scale
            m = m * Matrix4.CreateScale(Scale);

            // Translation
            m.M41 = Translation.X; m.M42 = Translation.Y; m.M43 = Translation.Z;

            return m;
        }
    }
}
