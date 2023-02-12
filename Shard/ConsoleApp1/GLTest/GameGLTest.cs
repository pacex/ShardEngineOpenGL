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

        private VisualGameObject go1;
        private VisualGameObject go2;


        public void handleInput(InputEvent inp, string eventType)
        {
            
        }

        public override void initialize()
        {

            Vector2i windowSize = Bootstrap.GetDisplayOpenGL().Window.Size;

            Bootstrap.GetDisplayOpenGL().Projection = Matrix4.CreatePerspectiveFieldOfView(0.4f * (float)Math.PI, (float)windowSize.X / (float)windowSize.Y, 0.1f, 256.0f);
            Bootstrap.GetDisplayOpenGL().View = Matrix4.LookAt(new Vector3(-3,0,1), Vector3.Zero, Vector3.UnitZ);

            //Bootstrap.GetDisplayOpenGL().Projection = Matrix4.Identity;
            //Bootstrap.GetDisplayOpenGL().View = Matrix4.Identity;
            Bootstrap.GetDisplayOpenGL().Model = Matrix4.Identity;

            go1 = new VisualGameObject(new Mesh());
            go2 = new VisualGameObject(new Mesh());
            go2.Transform.Translation.Z = 0.5f;
            go1.Transform.Translation.Z = -0.5f;

            go2.Transform.Rotate(Quaternion.FromEulerAngles(0.0f, 0.0f, 1.0f));

        }

        public override void update()
        {
            float rot = 0.8f * (float)Bootstrap.getDeltaTime();

            go1.Transform.Rotate(Quaternion.FromAxisAngle(Vector3.UnitZ, rot));
            go2.Transform.Rotate(Quaternion.FromAxisAngle(Vector3.UnitZ, -rot));
        }

        public override void draw()
        {

        }
    }
}
