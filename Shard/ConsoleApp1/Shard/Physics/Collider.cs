using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard.Shard.Physics
{
    enum ColliderMask
    {
        None = 0b0000,
        Player = 0b0001,
        Camera = 0b0010
    }

    abstract class Collider
    {
        public Box3 Bounds { get => bounds; }
        protected Box3 bounds;

        public ColliderMask Mask { get => mask; }
        protected ColliderMask mask;

        public Vector3 Position;

        public Box3 TranslatedBounds()
        {
            return Bounds.Translated(Position);
        }
        public abstract Collider CopyOffset(Vector3 offset);
        public abstract bool Intersects(Collider other);
        public abstract Vector3 Response(Collider other);
        public abstract void Draw(Color4 col);
    }
}
