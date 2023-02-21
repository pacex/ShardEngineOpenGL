using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard
{
    class VisualGameObject : GameObject
    {
        public Mesh Mesh;
        public Texture Texture;

        public VisualGameObject(Mesh mesh, Texture texture = null)
        {
            this.Mesh = mesh;
            this.Texture = texture;
        }

        public override void drawUpdate()
        {
            base.drawUpdate();
            if (Mesh != null)
            {

                Bootstrap.GetDisplayOpenGL().Model = Transform.ToMatrix();
                Shader.ApplyDefaultShader(Texture);
                Mesh.Draw();
                Shader.Reset();
            }
        }
    }
}
