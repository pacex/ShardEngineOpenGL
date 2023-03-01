using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;

namespace Shard
{
    abstract class Collider3D
    {
        private CollisionHandler gameObject;
        Vector3 centre;
        Vector3 boundingBoxMin;
        Vector3 boundingBoxMax;
        //private bool rotateAtOffset;                           

        public abstract void recalculate();
        public Collider3D(CollisionHandler gob)
        {

            gameObject = gob;

        }

        internal CollisionHandler GameObject { get => gameObject; set => gameObject = value; }
        //public bool RotateAtOffset { get => rotateAtOffset; set => rotateAtOffset = value; }

        public bool areColliding(Collider3D c)
        {
            return areColliding(c, Vector2.Zero);
        }

        public bool areColliding(Collider3D c, Vector2 offset)
        {
            if (c is ColliderCube)
            {
                return areColliding((ColliderCube)c, offset);
            }
            if (c is ColliderSphere)
            {
                return areColliding((ColliderSphere)c, offset);
            }

            return false;
        }
        public abstract bool areColliding(ColliderCube c);
        public abstract bool areColliding(ColliderSphere c);
        public abstract bool areColliding(Vector3 c);
        public abstract bool areColliding(ColliderCube c, Vector2 offset);
        public abstract bool areColliding(ColliderSphere c, Vector2 offset);
        public abstract bool areColliding(Vector3 c, Vector2 offset);
        public abstract float getMinX();

        public abstract float getMaxX();
        public abstract float getMinY();


        public abstract float getMaxY();


        public abstract float getMinZ();

        public abstract float getMaxZ();
       
        //public abstract Vector3? checkCollision(ColliderRect c);

        //public abstract Vector3? checkCollision(Vector3 c);

        //public abstract Vector3? checkCollision(ColliderCircle c);

        //public virtual Vector3? checkCollision(Collider c)
        //{

        //    if (c is ColliderRect)
        //    {
        //        return checkCollision((ColliderRect)c);
        //    }

        //    if (c is ColliderCircle)
        //    {
        //        return checkCollision((ColliderCircle)c);
        //    }

        //    Debug.getInstance().log("Bug");
        //    // Not sure how we got here but c'est la vie
        //    return null;
        //}

        public abstract void DrawMe(Color col);
        public abstract void calculateBoundingBox();
        //public abstract float[] getMinAndMaxX();
        //public abstract float[] getMinAndMaxY();

        public abstract Vector3 getCentre();
      
        public abstract float getCentreX();

        public abstract float getCentreY();
       

        public abstract float getCentreZ();
       

    }
}

