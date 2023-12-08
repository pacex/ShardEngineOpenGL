using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Common;
using Shard.Shard.Graphics;
using Shard.Shard.GameObjects;
using Shard.Shard.Physics;

namespace Shard.GLTest
{
    class GameGLTest : Game
    {

        private VisualGameObject level;
        private Player player;
        private Wall wall;
        private int killCount = 0;
        private int amountToSpawn = 1;
        private float maxSpeed = 0.1125f;
        private int requriedKills = 4;


        Random rnd = new Random();
        private Vector3[] monsterSpawnLocations = { new Vector3(18.0f, 18.0f, 0.0f), new Vector3(24.0f, 18.0f, 0.0f), new Vector3(23.0f, -5.0f, 0.0f),
                                                new Vector3(25.0f, 0.0f, 0.0f), new Vector3(3.0f, -5.0f, 0.0f), new Vector3(-3.0f, 5.0f, 0.0f), new Vector3(20.0f, 13.0f, 0.0f)};

        public override void Initialize()
        {
            // Display settings
            GL.ClearColor(Color4.Black);

            DisplayOpenGL.GetInstance().Window.CursorState = CursorState.Grabbed;
            DisplayOpenGL.GetInstance().Window.WindowState = WindowState.Maximized;

            // GameObjects
            player = new Player();
            player.Transform.Translation = new Vector3(-4.0f, 1.0f, 0.0f);
            GameObjectManager.CreateGameObject(player);

            Mesh levelMesh = ObjLoader.LoadMesh("GLTest\\level2_1.obj");
            Physics.GetInstance().AddStaticMesh(levelMesh, Vector3.Zero);

            level = new VisualGameObject(levelMesh, new Texture("GLTest\\texture_level2.png", TextureWrapMode.MirroredRepeat, TextureMinFilter.NearestMipmapLinear, TextureMagFilter.Nearest, 0, 3));
            level.Transform.Translation = new Vector3(0.0f, 0.0f, 0.0f);
            GameObjectManager.CreateGameObject(level);


            wall = new Wall(new ColliderCuboid(new Box3(-1.0f, -1.0f, 0.0f, 1.0f, 1.0f, 2.0f)));
            wall.Transform.Translation = new Vector3(-6.0f, 6.0f, 0.0f);
            GameObjectManager.CreateGameObject(wall);

            wall = new Wall(new ColliderCuboid(new Box3(-1.0f, -1.0f, 0.0f, 1.0f, 1.0f, 2.0f)));
            wall.Transform.Translation = new Vector3(-8.0f, 4.0f, 0.0f);
            GameObjectManager.CreateGameObject(wall);

            wall = new Wall(new ColliderCuboid(new Box3(-1.0f, -1.0f, 0.0f, 1.0f, 1.0f, 2.0f)));
            wall.Transform.Translation = new Vector3(-8.0f, 1.5f, 0.0f);
            GameObjectManager.CreateGameObject(wall);

            wall = new Wall(new ColliderTriangle(new Vector3(0.0f, -1.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f), new Vector3(0.0f, 1.0f, 2.0f)));
            wall.Transform.Translation = new Vector3(-8.0f, -1.5f, 0.0f);
            GameObjectManager.CreateGameObject(wall);
        }

        public override void Update()
        {
            // End game
            if (DisplayOpenGL.GetInstance().Window.IsKeyPressed(Keys.Escape))
            {
                Bootstrap.EndGame();
            }        
        }

        public override void Draw()
        {

        }

        public override void GameEnd()
        {
            
        }
    }
}
