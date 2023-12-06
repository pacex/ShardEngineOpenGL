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
        }

        public Vector3 Position { get => collider.Position; }

        private Collider collider;
        public Collider Collider { get => collider; }

        /* Move Host by v.
         * If target position is obstructed, resolve ubstruction.
         * Returns true if collision needs to be resolved.
         * */
        public bool MoveAndSlide(Vector3 v)
        {
            Collider c = collider.CopyOffset(v);
            if (Bootstrap.Physics.IntersectsStatic(c))
                return true;

            collider.Position += v;
            Host.Transform.Translation = collider.Position;
            return false;
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