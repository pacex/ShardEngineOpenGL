using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard.Shard.Physics
{
    class Physics
    {
        // Singleton
        private static Physics physics = null;

        public static Physics GetInstance() { 
            if (physics == null)
                physics = new Physics();
            return physics;
        }

        // 


        private Physics() { }


    }
}
