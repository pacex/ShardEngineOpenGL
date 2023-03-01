
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
        public ColliderSphere(CollisionHandler gob, Transform3DNew t, float radius) : base(gob)
        {
            this.radius = radius;
            mySphere = t;
            centre = t.Translation;
            calculateBoundingBox();
        }
        public override void calculateBoundingBox()
        {
            boundingBoxMax = new Vector3(radius + centre.X, radius + centre.Y, radius + centre.Z);
            boundingBoxMin = new Vector3(radius - centre.X, radius - centre.Y, radius - centre.Z);
        }

      
       
       public override void recalculate()
        {
             calculateBoundingBox();
        }
        public override bool areColliding(ColliderCube c)
        {
            return areColliding(c, Vector2.Zero);
        }

        public override bool areColliding(ColliderSphere c)
        {
            return areColliding(c, Vector2.Zero);
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

        public override bool areColliding(ColliderCube c, Vector2 offset)
        {
            float xDistance = Math.Abs(centre.X + offset.X - c.getCentreX());
            float yDistance = Math.Abs(centre.Y + offset.Y - c.getCentreX());
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

        public override bool areColliding(ColliderSphere c, Vector2 offset)
        {
            double dis = Math.Sqrt(Math.Pow(centre.X + offset.X - c.centre.X, 2) + Math.Pow(centre.Y + offset.Y - c.centre.Y, 2) + Math.Pow(centre.Z - c.centre.Z, 2));
            return dis <= (radius + c.getRadius());
        }

        public override bool areColliding(Vector3 c, Vector2 offset)
        {
            throw new NotImplementedException();
        }
    }
}
