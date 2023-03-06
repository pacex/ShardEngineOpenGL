using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Shard.GLTest
{
    class Rifle
    {
       public void FireRifle(Vector2 direction)
        {
            Bullet b = new Bullet();
            b.FireMe(direction);
        }
    }
}
