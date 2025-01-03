﻿using OpenTK.Mathematics;
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
            SlideAngle = 0.75f;
        }

        public Vector3 Position { get => collider.Position; }

        private Collider collider;
        public Collider Collider { get => collider; }

        // Properties
        public float SlideAngle;

        /* Move Host by v.
         * If target position is obstructed, resolve ubstruction.
         * Returns true if collision needs to be resolved.
         * */
        public bool MoveAndSlide(Vector3 v, out Vector3 reflected)
        {
            Collider c = collider.CopyOffset(v);

            reflected = v;
            Vector3 response = Bootstrap.Physics.ResponseStatic(c, ref reflected);

            if (Vector3.CalculateAngle(response, Vector3.UnitZ) < SlideAngle)
                response = Vector3.UnitZ * response.Length;

            //if (Bootstrap.Physics.IntersectsStatic(c))
                //return true;

            collider.Position += v + response;
            Host.Transform.Translation = collider.Position;
            return response.LengthSquared > float.Epsilon;
        }

        public bool MoveAndSlide(Vector3 v)
        {
            Vector3 dc;
            return MoveAndSlide(v, out dc);
        }

        public override void Initialize()
        {
            collider.Position = Host.Transform.Translation;
        }

        public override void OnDestroy()
        {
            
        }

        public override void Update()
        {
            collider.Position = Host.Transform.Translation;
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
