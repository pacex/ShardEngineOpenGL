using OpenTK.Mathematics;
using Shard.Shard.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard.Shard.Physics
{
    class DynamicBody : Component
    {
        public DynamicBody(Collider collider, GameObject parent) : base(parent)
        {
            this.collider = collider;
        }

        private Vector3 position;
        public Vector3 Position { get => position; }

        private Collider collider;
        public Collider Collider { get => collider; }

        /* Move Host by v.
         * If target position is obstructed, resolve ubstruction.
         * Returns true if collision needs to be resolved.
         * */
        public bool MoveAndSlide(Vector3 v)
        {
            position += v;
            Host.Transform.Translation = position;
            return false;
        }

        public override void Initialize()
        {
            position = Host.Transform.Translation;
        }

        public override void OnDestroy()
        {
            
        }

        public override void Update()
        {
            position = Host.Transform.Translation;
        }

        public override void Draw()
        {
            if (Bootstrap.PhysDebug)
            {
                collider.Draw(position, Color4.Green);
            }
        }
    }
}
