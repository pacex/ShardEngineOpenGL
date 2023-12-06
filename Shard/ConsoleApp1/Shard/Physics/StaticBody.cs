using OpenTK.Mathematics;
using Shard.Shard.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard.Shard.Physics
{
    class StaticBody : Component
    {
        public StaticBody(Collider collider, GameObject parent) : base(parent)
        {
            this.collider = collider;
        }

        public Vector3 Position { get => collider.Position; }

        private Collider collider;
        public Collider Collider { get => collider; }

        public override void Initialize()
        {
            collider.Position = Host.Transform.Translation;
            Bootstrap.Physics.AddStaticBody(this);
        }

        public override void OnDestroy()
        {

        }

        public override void Update()
        {

        }

        public override void Draw()
        {
            if (Bootstrap.PhysDebug)
            {
                collider.Draw(Color4.Green);
            }
        }
    }
}
