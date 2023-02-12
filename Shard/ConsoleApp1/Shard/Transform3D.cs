/*
*
*   Our game engine functions in 2D, but all its features except for graphics can mostly be extended
*       from existing data structures.
*       
*   @author Michael Heron
*   @version 1.0
*   
*/

using OpenTK.Mathematics;

namespace Shard
{
    


    class Transform3D : Transform
    {
        private double z;
        private double rotx, roty;
        private double scalez;

        public static Matrix4 buildTransformMatrix(Vector3 translation, Vector3 rotation, Vector3 scale)
        {
            Matrix4 rx, ry, rz, s, t;

            Matrix4.CreateRotationZ(rotation.Z, out rz);
            Matrix4.CreateRotationY(rotation.Y, out ry);
            Matrix4.CreateRotationX(rotation.X, out rx);

            Matrix4.CreateScale(scale.X, scale.Y, scale.Z, out s);

            Matrix4.CreateTranslation(translation.X, translation.Y, translation.Z, out t);

            return t * (rz * ry * rx) * s;
        }

        public Transform3D(GameObject o) : base(o)
        {
            scalez = 1.0;
            z = 0.0;
            rotx = 0.0;
            roty = 0.0;
        }

        public double Z
        {
            get => z;
            set => z = value;
        }



        public double Scalez
        {
            get => scalez;
            set => scalez = value;
        }
        public double Rotx { get => rotx; set => rotx = value; }
        public double Roty { get => roty; set => roty = value; }

        public Matrix4 getMatrix()
        {
            Matrix4 rx, ry, rz, s, t;

            Matrix4.CreateRotationZ((float)Rotz, out rz);
            Matrix4.CreateRotationY((float)Roty, out ry);
            Matrix4.CreateRotationX((float)Rotx, out rx);

            Matrix4.CreateScale((float)Scalex, (float)Scaley, (float)Scalez, out s);

            Matrix4.CreateTranslation((float)X, (float)Y, (float)Z, out t);

            Matrix4 ret = t * (rz * ry * rx) * s;

            return ret;
        }
    }
}
