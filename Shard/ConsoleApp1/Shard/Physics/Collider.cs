using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard.Shard.Physics
{
    abstract class Collider
    {
        public Box3 Bounds;
        public Vector3 Position;
        public abstract Collider CopyOffset(Vector3 offset);
        public abstract bool Intersects(Collider other);
        public abstract Vector3 Response(Collider other);
        public abstract void Draw(Color4 col);
    }
}
