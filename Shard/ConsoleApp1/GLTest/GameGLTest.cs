using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Common;

namespace Shard.GLTest
{
    class GameGLTest : Game, InputListener
    {

        private VisualGameObject level;
        private Player player;


        public void handleInput(InputEvent inp, string eventType)
        {
            
        }

        public override void initialize()
        {

            GL.ClearColor(Color4.Black);

            DisplayOpenGL.GetInstance().Window.CursorState = CursorState.Grabbed;
            DisplayOpenGL.GetInstance().Window.WindowState = WindowState.Maximized;

            player = new Player();
            player.Transform.Translation = new Vector3(-4.0f, 1.0f, 0.0f);

            level = new VisualGameObject(ObjLoader.LoadMesh("GLTest\\level.obj"), 
                new Texture("GLTest\\texture_level.png", TextureWrapMode.MirroredRepeat, TextureMinFilter.NearestMipmapLinear, TextureMagFilter.Nearest, 0, 32));

            level.Transform.Translation = new Vector3(0.0f, 0.0f, 0.0f);

        }

        public override void update()
        {
            if (DisplayOpenGL.GetInstance().Window.IsKeyPressed(Keys.Escape))
            {
                Bootstrap.endGame();
            }
        }

        public override void draw()
        {

        }
    }
}
