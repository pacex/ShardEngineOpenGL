using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Shard.GLTest
{
    class GameGLTest : Game, InputListener
    {

        private VisualGameObject go1;

        private float time;


        public void handleInput(InputEvent inp, string eventType)
        {
            
        }

        public override void initialize()
        {

            GL.ClearColor(Color4.Black);

            Vector2i windowSize = Bootstrap.GetDisplayOpenGL().Window.Size;

            //Bootstrap.GetDisplayOpenGL().Projection = Matrix4.CreatePerspectiveFieldOfView(0.4f * (float)Math.PI, (float)windowSize.X / (float)windowSize.Y, 0.1f, 256.0f);
            //Bootstrap.GetDisplayOpenGL().View = Matrix4.LookAt(new Vector3(-5,2,2), Vector3.Zero, Vector3.UnitZ);

            Camera camera = new Camera();
            camera.Transform.Translation = new Vector3(-5,2,2);
            camera.SetAsMain();

            //Bootstrap.GetDisplayOpenGL().Projection = Matrix4.Identity;
            //Bootstrap.GetDisplayOpenGL().View = Matrix4.Identity;
            Bootstrap.GetDisplayOpenGL().Model = Matrix4.Identity;

            go1 = new VisualGameObject(ObjLoader.LoadMesh("GLTest\\level.obj"), new Texture("GLTest\\texture_level.png"));
            go1.Transform.Translation = new Vector3(0.0f, 0.0f, 0.0f);

            time = 0.0f;
        }

        public override void update()
        {
            float rot = 0.8f * (float)Bootstrap.getDeltaTime();

            time += (float)Bootstrap.getDeltaTime();

            //go2.Transform.Translation.Z = (float)Math.Sin(time);

            //go1.Transform.Rotate(Quaternion.FromAxisAngle(Vector3.UnitZ, rot));

            /*
            Console.WriteLine("Forward:");
            Console.WriteLine(go2.Transform.Forward.ToString());

            Console.WriteLine("Left:");
            Console.WriteLine(go2.Transform.Left.ToString());

            Console.WriteLine("Up:");
            Console.WriteLine(go2.Transform.Up.ToString());
            */

        }

        public override void draw()
        {

        }
    }
}
