﻿using System;
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
    class GameGLTest : Game
    {

        private VisualGameObject level;
        private Player player;
        private Monster monster = null;

        Random rnd = new Random();
        private Vector3[] monsterSpawnLocations = { new Vector3(18.0f, 18.0f, 0.0f), new Vector3(24.0f, 18.0f, 0.0f), new Vector3(23.0f, -5.0f, 0.0f),
                                                new Vector3(25.0f, 0.0f, 0.0f), new Vector3(3.0f, -5.0f, 0.0f), new Vector3(-3.0f, 5.0f, 0.0f), new Vector3(20.0f, 13.0f, 0.0f)};

        public override void initialize()
        {
            // Display settings
            GL.ClearColor(Color4.Black);

            DisplayOpenGL.GetInstance().Window.CursorState = CursorState.Grabbed;
            DisplayOpenGL.GetInstance().Window.WindowState = WindowState.Maximized;

            // Physics settings
            PhysicsManager.getInstance().initKinematic(new Box2(-10.0f, -10.0f, 30.0f, 30.0f), 1.0f);

            // GameObjects
            player = new Player();
            player.Transform.Translation = new Vector3(-4.0f, 1.0f, 0.7f);

            level = new VisualGameObject(ObjLoader.LoadMesh("GLTest\\level2.obj"),new Texture("GLTest\\texture_level2.png", TextureWrapMode.MirroredRepeat, TextureMinFilter.NearestMipmapLinear, TextureMagFilter.Nearest, 0, 3));
            level.Transform.Translation = new Vector3(0.0f, 0.0f, 0.0f);
      
            monster = new Monster();
            monster.Transform.Translation = new Vector3(18.0f, 18.0f, 0.0f);
            //Monster m1 = new Monster();
            //m1.Transform.Translation = new Vector3(18.0f, 18.0f, 0.0f);
            //Monster m2 = new Monster();
            //m2.Transform.Translation = new Vector3(22.0f, 18.0f, 0.0f);

            #region Wall objects

            // Walls
            Wall go;
            go = new Wall(new Vector2(2.0f, 2.0f));
            go.Transform.Translation = new Vector3(-6.0f, -6.0f, 1.0f);
            go.initPhys();
            go = new Wall(new Vector2(2.0f));
            go.Transform.Translation = new Vector3(6.0f, -6.0f, 1.0f);
            go.initPhys();
            go = new Wall(new Vector2(2.0f));
            go.Transform.Translation = new Vector3(-6.0f, 6.0f, 1.0f);
            go.initPhys();
            go = new Wall(new Vector2(2.0f));
            go.Transform.Translation = new Vector3(6.0f, 6.0f, 1.0f);
            go.initPhys();
            go = new Wall(new Vector2(6.0f));
            go.Transform.Translation = new Vector3(10.0f, 4.0f, 1.0f);
            go.initPhys();
            go = new Wall(new Vector2(6.0f));
            go.Transform.Translation = new Vector3(10.0f, -4.0f, 1.0f);
            go.initPhys();
            go = new Wall(new Vector2(34.0f, 2.0f));
            go.Transform.Translation = new Vector3(10.0f, -8.0f, 1.0f);
            go.initPhys();
            go = new Wall(new Vector2(24.0f, 2.0f));
            go.Transform.Translation = new Vector3(5.0f, 8.0f, 1.0f);
            go.initPhys();

            go = new Wall(new Vector2(2.0f, 2.0f));
            go.Transform.Translation = new Vector3(14.0f, -6.0f, 1.0f);
            go.initPhys();
            go = new Wall(new Vector2(2.0f));
            go.Transform.Translation = new Vector3(26.0f, -6.0f, 1.0f);
            go.initPhys();
            go = new Wall(new Vector2(2.0f));
            go.Transform.Translation = new Vector3(14.0f, 6.0f, 1.0f);
            go.initPhys();
            go = new Wall(new Vector2(2.0f));
            go.Transform.Translation = new Vector3(26.0f, 6.0f, 1.0f);
            go.initPhys();

            go = new Wall(new Vector2(2.0f, 12.0f));
            go.Transform.Translation = new Vector3(28.0f, 0.0f, 1.0f);
            go.initPhys();
            go = new Wall(new Vector2(2.0f, 12.0f));
            go.Transform.Translation = new Vector3(-8.0f, 0.0f, 1.0f);
            go.initPhys();

            go = new Wall(new Vector2(2.0f, 16.0f));
            go.Transform.Translation = new Vector3(26.0f, 15.0f, 1.0f);
            go.initPhys();
            go = new Wall(new Vector2(2.0f, 16.0f));
            go.Transform.Translation = new Vector3(14.0f, 15.0f, 1.0f);
            go.initPhys();

            go = new Wall(new Vector2(2.0f));
            go.Transform.Translation = new Vector3(24.0f, 8.0f, 1.0f);
            go.initPhys();

            go = new Wall(new Vector2(2.0f));
            go.Transform.Translation = new Vector3(24.0f, 22.0f, 1.0f);
            go.initPhys();

            go = new Wall(new Vector2(2.0f));
            go.Transform.Translation = new Vector3(16.0f, 22.0f, 1.0f);
            go.initPhys();

            go = new Wall(new Vector2(10.0f, 2.0f));
            go.Transform.Translation = new Vector3(20.0f, 24.0f, 1.0f);
            go.initPhys();

            // Crates
            go = new Wall(new Vector2(4.1f, 1.1f));
            go.Transform.Translation = new Vector3(20.0f, 4.0f, 1.0f);
            go.initPhys();

            #endregion

        }

        public override void update()
        {
            if (monster == null)
            {
                // Spawn new monster
                monster = new Monster();
                monster.Transform.Translation = new Vector3(monsterSpawnLocations[rnd.Next(monsterSpawnLocations.Length)]);
            }

            // Update monster target position
            monster.targetPosition(player.getPlayerPos());

            //Check if dead
            if (monster.isDead())
            {
                monster = null;
            }
            

            



            // End game
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
