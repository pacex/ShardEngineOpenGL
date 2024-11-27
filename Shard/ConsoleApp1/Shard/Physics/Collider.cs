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
        public Box3 Bounds { get => bounds; }
        protected Box3 bounds;

        public uint Mask { get => mask; }
        protected uint mask;

        public Vector3 Position;

        public Box3 TranslatedBounds()
        {
            return Bounds.Translated(Position);
        }
        public abstract Collider CopyOffset(Vector3 offset);
        public abstract bool Intersects(Collider other);
        public abstract Vector3 Response(Collider other, out Vector3 normal);
        public abstract void Draw(Color4 col);
    }
}
