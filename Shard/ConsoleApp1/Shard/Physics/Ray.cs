using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard.Shard.Physics
{
    class Intersection
    {
        public float t { get; private set; }
    }

    class Ray
    {
        public Vector3 Origin { get; private set; }
        public Vector3 Direction { get; private set;}

        public Ray(Vector3 origin, Vector3 direction)
        {
            Origin = origin;
            Direction = direction;
        }
    }
}
