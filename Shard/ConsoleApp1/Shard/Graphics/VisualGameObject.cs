using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shard.Shard.GameObjects;

namespace Shard.Shard.Graphics
{
    /*
     * A GameObject with a mesh and a texture that draws itself automatically.
     * 
     */

    class VisualGameObject : GameObject
    {
        public VisualGameObject(Mesh mesh, Texture texture = null) : base()
        {
            AddComponent(new MeshRenderer(mesh, texture, this));
        }
    }
}
