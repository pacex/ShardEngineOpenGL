using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Shard
{
    class GameGLTest : Game, InputListener
    {

        private float[] vertices;
        private uint[] indices;
        private Mesh mesh;

        private float rot;

        Matrix4 mat;


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


            mesh = new Mesh();

            Vector2i windowSize = Bootstrap.GetDisplayOpenGL().Window.Size;

            Bootstrap.GetDisplayOpenGL().Projection = Matrix4.CreatePerspectiveFieldOfView(0.4f * (float)Math.PI, (float)windowSize.X / (float)windowSize.Y, 0.1f, 256.0f);
            Bootstrap.GetDisplayOpenGL().View = Matrix4.LookAt(new Vector3(-3,0,1), Vector3.Zero, Vector3.UnitZ);

            //Bootstrap.GetDisplayOpenGL().Projection = Matrix4.Identity;
            //Bootstrap.GetDisplayOpenGL().View = Matrix4.Identity;
            Bootstrap.GetDisplayOpenGL().Model = Matrix4.Identity;

        }

        public override void update()
        {
            rot += 0.01f;
            mat = Transform3D.buildTransformMatrix(Vector3.Zero, new Vector3(0.0f, 0.0f, rot), Vector3.One);
        }

        public override void draw()
        {
            Bootstrap.GetDisplayOpenGL().Model = mat;

            Shader.SetDefaultShader();
            mesh.Draw();
        }
    }
}
