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

        public VisualGameObject(Mesh mesh) {
            this.Mesh = mesh;
        }

        public override void drawUpdate()
        {
            base.drawUpdate();
            if (Mesh != null)
            {

                Bootstrap.GetDisplayOpenGL().Model = Transform.ToMatrix();
                Shader.ApplyDefaultShader();
                Mesh.Draw();
                Shader.Reset();
            }
        }
    }
}
