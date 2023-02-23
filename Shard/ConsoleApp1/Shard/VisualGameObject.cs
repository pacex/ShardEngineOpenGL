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

                GameGLTest game = (GameGLTest)Bootstrap.getRunningGame();

                Bootstrap.GetDisplayOpenGL().Model = Transform.ToMatrix();
                Shader.ApplyDefaultShader(game.Texture);
                Mesh.Draw();
                Shader.Reset();
            }
        }
    }
}
