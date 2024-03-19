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
        private VisualGameObject character;
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

            Mesh colMesh = ObjLoader.LoadMeshObj("GLTest\\level2_3.obj");
            Physics.GetInstance().AddStaticMesh(colMesh, Vector3.Zero);

            Mesh levelMesh = ObjLoader.LoadMeshObj("GLTest\\level2_3.obj");
            level = new VisualGameObject(levelMesh, 
                new Texture("GLTest\\texture_level2.png", TextureWrapMode.MirroredRepeat, TextureMinFilter.NearestMipmapLinear, TextureMagFilter.Nearest, 0, 3));
            level.Transform.Translation = new Vector3(0.0f, 0.0f, 0.0f);
            GameObjectManager.CreateGameObject(level);

            
            AnimatedMesh gltfTest = ObjLoader.LoadAnimatedMesh("GLTest\\Character\\RobotB.fbx", 1);
            character = new VisualGameObject(gltfTest,
                new Texture("GLTest\\CesiumMan\\CesiumMan_img0.jpg", TextureWrapMode.MirroredRepeat, TextureMinFilter.NearestMipmapLinear, TextureMagFilter.Nearest, 0, 3));
            character.Transform.Translation = Vector3.UnitZ * 2.7f;
            character.Transform.Scale = Vector3.One * 0.4f;
            GameObjectManager.CreateGameObject(character);

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
