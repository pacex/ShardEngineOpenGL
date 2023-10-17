using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard.Shard.Graphics
{
    /*
     * A GameObject with a mesh and a texture that draws itself automatically.
     * 
     */

    class VisualGameObject : GameObject
    {
        public Mesh Mesh;
        public Texture Texture;

        public VisualGameObject(Mesh mesh, Texture texture = null)
        {
            Mesh = mesh;
            Texture = texture;
        }

        public override void drawUpdate()
        {
            base.drawUpdate();
            if (Mesh != null)
            {

                Bootstrap.Display.Model = Transform.ToMatrix();
                Shader.ApplyDefaultShader(Texture);
                Mesh.Draw();
                Shader.Reset();
            }
        }
    }
}
