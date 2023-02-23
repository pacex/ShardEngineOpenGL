using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Shard
{
    class ColliderSphere : Collider3D
    {
        float radius;
        Vector3 centre;

        public override void calculateBoundingBox()
        {
            MinAndMaxX[0] = cenre
        }

        public override Vector3? checkCollision(ColliderSphere c)
        {
            double dist, depth, radsq, radsum;
            double xpen, ypen;
            Vector3 dir;
            depth = radius + c.radius - Math.Sqrt(distanceBetween(this, c));
            if (areColliding(this, c))
            {
                dir = new Vector3(X - c.x, Y - c.y, Z - c.z);
                dir = Vector3.Normalize(dir);
                dir = 
                
            }

            
            

            xpen = Math.Pow(c.X - this.X, 2);
            ypen = Math.Pow(c.Y - this.Y, 2);

            radsq = Math.Pow(c.Rad + this.Rad, 2);

            dist = xpen + ypen;


            depth = (c.Rad + Rad) - Math.Sqrt(dist);


            if (dist <= radsq)
            {
                dir = new Vector2(X - c.X, Y - c.Y);
                dir = Vector2.Normalize(dir);

                dir *= (float)depth;

                return dir;
            }

            return null;
        }
      public double distanceBetween(ColliderSphere a, ColliderSphere b) {
    return Math.Sqrt(Math.Pow(a.centre.X - b.centre.X, 2 ) + Math.Pow(a.centre.Y - b.centre.Y, 2 ) + Math.Pow(a.centre.Z - b.centre.Z, 2 ) );
}
        public bool areColliding(ColliderSphere a, ColliderSphere b)
        {
            return distanceBetween(a, b) =< (a.radius + b.radius);
        }
    public override Vector3? checkCollision(ColliderPrism c)
        {
            throw new NotImplementedException();
        }

        public override Vector3? checkCollision(Vector3 c)
        {
            throw new NotImplementedException();
        }

        public override void drawMe(Color col)
        {
            throw new NotImplementedException();
        }

        public override float[] getMinAndMaxX()
        {
            throw new NotImplementedException();
        }

        public override float[] getMinAndMaxY()
        {
            throw new NotImplementedException();
        }

        public override float[] getMinAndMaxZ()
        {
            throw new NotImplementedException();
        }

        public override void recalculate()
        {
            throw new NotImplementedException();
        }
    }
}
