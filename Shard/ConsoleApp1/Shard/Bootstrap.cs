/*
*
*   The Bootstrap - this loads the config file, processes it and then starts the game loop
*   @author Michael Heron
*   @version 1.0
*   
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Shard.Shard.Assets;
using Shard.Shard.GameObjects;
using Shard.Shard.Graphics;
using Shard.Shard.Physics;
using Shard.Shard.Sound;

namespace Shard
{
    class Bootstrap
    {
        public static string DEFAULT_CONFIG = "config.cfg";


        private static Game runningGame;
        private static DisplayOpenGL displayEngine;
        private static Sound soundEngine;
        private static AssetManagerBase asset;
        private static Physics physics;

        private static int targetFrameRate;
        private static int millisPerFrame;
        private static float deltaTime;
        private static string baseDir;
        private static Dictionary<string, string> enVars;
        private static bool endGameFlag = false;

        private static bool physDebug = false;
        public static bool PhysDebug { get => physDebug; }

        public static Game RunningGame
        {
            get { return runningGame; }
        }

        public static DisplayOpenGL Display
        {
            get { return (DisplayOpenGL)displayEngine; }
        }

        public static Physics Physics { 
            get { return physics; }
        }

        public static float DeltaTime
        {
            get { return deltaTime; }
        }

        public static bool CheckEnvironmentalVariable(string id) {
            return enVars.ContainsKey(id);
        }

        
        public static string GetEnvironmentalVariable(string id) {
            if (CheckEnvironmentalVariable(id)) {
                return enVars[id];
            }

            return null;
        }

        public static string GetBaseDir() {
            return baseDir;
        }

        public static void Setup()
        {
            string workDir = Environment.CurrentDirectory;
            baseDir = Directory.GetParent(workDir).Parent.Parent.Parent.Parent.FullName;;

            SetupEnvironmentalVariables(baseDir + "\\" + "envar.cfg");
            Setup(baseDir + "\\" + DEFAULT_CONFIG);

        }

        public static void SetupEnvironmentalVariables (String path) {
                Console.WriteLine("Path is " + path);

                Dictionary<string, string> config = BaseFunctionality.getInstance().readConfigFile(path);

                enVars = new Dictionary<string,string>();

                foreach (KeyValuePair<string, string> kvp in config)
                {
                    enVars[kvp.Key] = kvp.Value;
                }
        }

        public static Sound GetSound()
        {
            return soundEngine;
        }

        public static AssetManagerBase GetAssetManager() {
            return asset;
        }


        private static void Setup(string path)
        {
            Console.WriteLine ("Path is " + path);

            Dictionary<string, string> config = BaseFunctionality.getInstance().readConfigFile(path);
            Type t;
            object ob;
            bool bailOut = false;

            displayEngine = DisplayOpenGL.GetInstance();
            displayEngine.Initialize();
            soundEngine = new SoundSDL();
            physics = Physics.GetInstance();
            physics.Initialize(new OpenTK.Mathematics.Box3(-30.0f, -10.0f, -2.0f, 10.0f, 30.0f, 6.0f), new OpenTK.Mathematics.Vector3(2.0f)); // Replace this after scene implementation
            asset = (AssetManagerBase)new AssetManager();
            asset.registerAssets();

            foreach (KeyValuePair<string, string> kvp in config)
            {
                t = Type.GetType("Shard." + kvp.Value);

                if (t == null)
                {
                    Debug.getInstance().log("Missing Class Definition: " + kvp.Value + " in " + kvp.Key, Debug.DEBUG_LEVEL_ERROR);
                    Environment.Exit(0);
                }

                ob = Activator.CreateInstance(t);


                switch (kvp.Key)
                {
                    case "game":
                        runningGame = (Game)ob;
                        targetFrameRate = runningGame.GetTargetFramerate();
                        millisPerFrame = 1000 / targetFrameRate;
                        break;
                }

                Debug.getInstance().log("Config file... setting " + kvp.Key + " to " + kvp.Value);
            }

            if (runningGame == null)
            {
                Debug.getInstance().log("No game set", Debug.DEBUG_LEVEL_ERROR);
                bailOut = true;
            }

            if (displayEngine == null)
            {
                Debug.getInstance().log("No display engine set", Debug.DEBUG_LEVEL_ERROR);
                bailOut = true;
            }

            if (soundEngine == null)
            {
                Debug.getInstance().log("No sound engine set", Debug.DEBUG_LEVEL_ERROR);
                bailOut = true;
            }

            if (bailOut)
            {
                Environment.Exit(0);
            }
        }

        public static long GetCurrentMillis()
        {
            return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        public static void EndGame()
        {
            endGameFlag = true;
        }

        static void Main(string[] args)
        {
            long timeInMillisecondsStart, timeInMillisecondsEnd;
            long interval;
            int sleep;



            // Setup the engine.
            Setup();


            // Start the game running.
            runningGame.Initialize();


            if (GetEnvironmentalVariable("physics_debug") == "1")
            {
                physDebug = true;
            }

            while (!endGameFlag)
            {
                timeInMillisecondsStart = GetCurrentMillis();
                
                // Clear the screen.
                Display.ClearDisplay();

                Display.ProcessWindowEvents();

                // Update 
                RunningGame.Update();

                if (runningGame.IsRunning() == true)
                {
                    // Update runs as fast as the system lets it.  Any kind of movement or counter 
                    // increment should be based then on the deltaTime variable.
                    GameObjectManager.GetInstance().Update();

                    // Execute display predraw
                    displayEngine.PreDraw();

                    // Let Game draw to the screen
                    runningGame.Draw();

                    // Let GameObjects draw to the screen
                    GameObjectManager.GetInstance().Draw();

                    // Physics Debug Draw
                    if (physDebug)
                        Physics.DebugDraw();
                }

                // Render the screen.
                Display.Display();

                // Timing
                timeInMillisecondsEnd = GetCurrentMillis();
                interval = timeInMillisecondsEnd - timeInMillisecondsStart;

                sleep = (int)(millisPerFrame - interval);
                if (sleep >= 0)
                {
                    Thread.Sleep(sleep);
                }

                // Set DeltaTime for next frame
                deltaTime = (GetCurrentMillis() - timeInMillisecondsStart) / 1000.0f;
                
            }

            runningGame.GameEnd();
        }
    }
}
