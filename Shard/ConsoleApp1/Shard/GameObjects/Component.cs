using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard.Shard.GameObjects
{
    abstract class Component
    {
        private GameObject host;

        public GameObject Host { get => host; }

        public Component(GameObject parent)
        {
            host = parent;
        }

        public abstract void Initialize();
        public abstract void Update();
        public abstract void Draw();
        public abstract void OnDestroy();
    }
}
