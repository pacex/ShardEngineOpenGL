using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OpenTK.Mathematics;
using System.Text;
using System.Threading.Tasks;
using Shard;

namespace Shard
{

    
    class ColliderCube : Collider3D
    {
        Vector3 centre;
        Vector3 boundingBoxMin;
        Vector3 boundingBoxMax;
        private float widht;
        private float height;
        private float depth;
        private Transform3DNew myCube;
        public ColliderCube(CollisionHandler gob, Transform3DNew t) : base(gob)
        {
            

            myCube = t;
            centre = t.Translation;

            calculateBoundingBox();
        }

        public override bool areColliding(Vector3 c)
        {
            throw new NotImplementedException();
        }
        
        public override bool areColliding(ColliderCube c)
        {
            float xDistance = Math.Abs(centre.X - c.getCentreX());
            float yDistance = Math.Abs(centre.Y - c.getCentreX());
            float zDistance = Math.Abs(centre.Z - c.getCentreX());

            if (xDistance <= ((c.getWidth() + getWidth()) / 2) && yDistance <= ((c.getHeight() + getHeight()) / 2) && zDistance <= ((c.getDepth() + getDepth()) / 2))
            {
                return true;
            }
            //TODO might not work.
            //float cornerDistance_sq = ((sphereXDistance - c.getWidth()) * (sphereXDistance - c.getWidth())) +
            //                      ((sphereYDistance - c.getHeight()) * (sphereYDistance - c.getHeight()) +
            //                      ((sphereYDistance - c.getDepth()) * (sphereYDistance - c.getDepth())));

            //return (cornerDistance_sq < (radius * radius));
            return false;
        }
        public override void calculateBoundingBox()
        {
            boundingBoxMax = (centre.X + (widht / 2), centre.Y + (height / 2), centre.Z + (depth / 2));
            boundingBoxMin = (centre.X - (widht / 2), centre.Y - (height / 2), centre.Z - (depth / 2));
        }
        //public override Vector3? checkCollision(ColliderCube c)
        //{
        //    throw new NotImplementedException();
        //}

        //public override Vector3? checkCollision(Vector3 c)
        //{
        //    throw new NotImplementedException();
        //}

        public override bool areColliding(ColliderSphere c)
        {
            float xDistance = Math.Abs(centre.X - c.getCentreX());
            float yDistance = Math.Abs(centre.Y - c.getCentreX());
            float zDistance = Math.Abs(centre.Z - c.getCentreX());

            if (xDistance <= (getWidth() / 2 + c.getRadius()) && yDistance <= (getHeight() / 2 + c.getRadius()) && zDistance <= (getDepth() / 2 + c.getRadius()))
            {
                return true;
            }
            float cornerDistance_sq = ((xDistance - getWidth()) * (xDistance - getWidth())) +
                                  ((yDistance - getHeight()) * (yDistance - getHeight()) +
                                  ((yDistance - getDepth()) * (yDistance - getDepth())));

            return (cornerDistance_sq < (c.getRadius() * c.getRadius()));
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
        public float getWidth()
        {
            return widht;
        }
        public float getHeight()
        {
            return height;
        }
        public float getDepth()
        {
            return depth;
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

        public override void recalculate()
        {
            calculateBoundingBox();
        }
    }
}
