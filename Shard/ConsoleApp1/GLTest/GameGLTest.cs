using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace Shard
{
    class GameGLTest : Game, InputListener
    {

        private float[] vertices;
        private uint[] indices;
        private Shader shader;
        private Mesh mesh;

        public void handleInput(InputEvent inp, string eventType)
        {
            
        }

        public override void initialize()
        {
            vertices = new float[] {
                0.5f,  0.5f, 0.0f,  // top right
                0.5f, -0.5f, 0.0f,  // bottom right
                -0.5f, -0.5f, 0.0f,  // bottom left
                -0.5f,  0.5f, 0.0f   // top left
            };

            indices = new uint[] {
                0, 1, 3,   // first triangle
                1, 2, 3    // second triangle
            };

            shader = Shader.GetDefaultShader();

            mesh = new Mesh(MeshPreset.UnitQuad);
        }

        public override void update()
        {
            
        }

        public override void draw()
        {
            shader.Use();
            mesh.Draw();
        }
    }
}
