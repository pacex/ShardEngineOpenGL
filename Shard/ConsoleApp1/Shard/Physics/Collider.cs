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
        public abstract void Draw(Vector3 pos, Color4 col);
    }
}
