using Shard.Shard.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard.Shard.Physics
{
    class RigidBody : DynamicBody
    {
        public RigidBody(Collider collider, GameObject parent) : base(collider, parent) {
            
        }
    }
}
