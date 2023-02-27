using Shard.Shard;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OpenTK.Mathematics;
using System.Text;
using System.Threading.Tasks;

namespace Shard
{
    class ColliderSphere : Collider3D
    {
        private float radius;
        Vector3 centre;
        Vector3 boundingBoxMin;
        Vector3 boundingBoxMax;
        Transform3DNew mySphere;
        public ColliderSphere(CollisionHandler gob, Transform3DNew t) : base(gob)
        {


            mySphere = t;
            centre = t.Translation;

            calculateBoundingBox();
        }
        public override void calculateBoundingBox()
        {
            boundingBoxMax = new Vector3(radius + centre.X, radius + centre.Y, radius + centre.Z);
            boundingBoxMin = new Vector3(radius - centre.X, radius - centre.Y, radius - centre.Z);
        }

        public double distanceBetween(ColliderSphere a, ColliderSphere b) {
            return Math.Sqrt(Math.Pow(a.centre.X - b.centre.X, 2) + Math.Pow(a.centre.Y - b.centre.Y, 2) + Math.Pow(a.centre.Z - b.centre.Z, 2));
        }
        //public override bool areColliding(ColliderSphere c)
        //{
        //    return distanceBetween(this, c) =< (radius + c.getRadius());
        //}
    //public override Vector3? checkCollision(ColliderCube c)
    //    {
    //        throw new NotImplementedException();
    //    }

        public override void recalculate()
        {
            throw new NotImplementedException();
        }

        public override bool areColliding(ColliderCube c)
        {
            float xDistance = Math.Abs(centre.X - c.getCentreX());
            float yDistance = Math.Abs(centre.Y - c.getCentreX());
            float zDistance = Math.Abs(centre.Z - c.getCentreX());

            if (xDistance <= (c.getWidth() / 2 + getRadius()) && yDistance <= (c.getHeight() / 2 + getRadius()) && zDistance <= (c.getDepth() / 2 + getRadius()))
            {
                return true;
            }
            float cornerDistance_sq = ((xDistance - c.getWidth()) * (xDistance - c.getWidth())) +
                                  ((yDistance - c.getHeight()) * (yDistance - c.getHeight()) +
                                  ((yDistance - c.getDepth()) * (yDistance - c.getDepth())));

            return (cornerDistance_sq < (getRadius() * getRadius()));
        }

        public override bool areColliding(ColliderSphere c)
        {
            return distanceBetween(this, c) <= (radius + c.getRadius());
        }

        public override bool areColliding(OpenTK.Mathematics.Vector3 c)
        {
            throw new NotImplementedException();
        }
        public override void DrawMe(Color col)
        {
            throw new NotImplementedException();
        }

        public override Vector3 getCentre()
        {
            return centre;
        }

        public override float getCentreX()
        {
            return centre.X;
        }

        public override float getCentreY()
        {
            return centre.Y;
        }

        public override float getCentreZ()
        {
            return centre.Z;
        }


        public override float getMinX()
        {
            return boundingBoxMin.X;
        }
        public override float getMaxX()
        {
            return boundingBoxMax.X;
        }
        public override float getMinY()
        {
            return boundingBoxMin.Y;
        }
        public override float getMaxY()
        {
            return boundingBoxMax.Y;
        }

        public override float getMinZ()
        {
            return boundingBoxMin.Z;
        }
        public override float getMaxZ()
        {
            return boundingBoxMax.Z;
        }

        public float getRadius()
        {
            return radius;
        }
    }
}
