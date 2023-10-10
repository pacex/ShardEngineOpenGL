﻿/*
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

namespace Shard
{
    class Bootstrap
    {
        public static string DEFAULT_CONFIG = "config.cfg";


        private static Game runningGame;
        private static DisplayOpenGL displayEngine;
        private static Sound soundEngine;
        private static InputSystem input;
        private static PhysicsManager phys;
        private static AssetManagerBase asset;

        private static int targetFrameRate;
        private static int millisPerFrame;
        private static double deltaTime;
        private static string baseDir;
        private static Dictionary<string,string> enVars;
        private static bool endGameFlag = false;

        public static bool checkEnvironmentalVariable(string id) {
            return enVars.ContainsKey(id);
        }

        
        public static string getEnvironmentalVariable(string id) {
            if (checkEnvironmentalVariable(id)) {
                return enVars[id];
            }

            return null;
        }

        public static string getBaseDir() {
            return baseDir;
        }

        public static void setup()
        {
            string workDir = Environment.CurrentDirectory;
            baseDir = Directory.GetParent(workDir).Parent.Parent.Parent.Parent.FullName;;

            setupEnvironmentalVariables(baseDir + "\\" + "envar.cfg");
            setup(baseDir + "\\" + DEFAULT_CONFIG);

        }

        public static void setupEnvironmentalVariables (String path) {
                Console.WriteLine("Path is " + path);

                Dictionary<string, string> config = BaseFunctionality.getInstance().readConfigFile(path);

                enVars = new Dictionary<string,string>();

                foreach (KeyValuePair<string, string> kvp in config)
                {
                    enVars[kvp.Key] = kvp.Value;
                }
        }
        public static double getDeltaTime()
        {
            return deltaTime;
        }

        public static Display getDisplay()
        {
            return displayEngine;
        }

        public static DisplayOpenGL GetDisplayOpenGL()
        {
            return (DisplayOpenGL)displayEngine;
        }

        public static Sound getSound()
        {
            return soundEngine;
        }

        public static InputSystem getInput()
        {
            return input;
        }

        public static AssetManagerBase getAssetManager() {
            return asset;
        }

        public static Game getRunningGame()
        {
            return runningGame;
        }

        private static void setup(string path)
        {
            Console.WriteLine ("Path is " + path);

            Dictionary<string, string> config = BaseFunctionality.getInstance().readConfigFile(path);
            Type t;
            object ob;
            bool bailOut = false;

            phys = PhysicsManager.getInstance();
            displayEngine = DisplayOpenGL.GetInstance();
            displayEngine.initialize();

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
                    case "sound":
                        soundEngine = (Sound)ob;
                        break;
                    case "asset":
                        asset = (AssetManagerBase)ob;
                        asset.registerAssets();
                        break;
                    case "game":
                        runningGame = (Game)ob;
                        targetFrameRate = runningGame.getTargetFrameRate();
                        millisPerFrame = 1000 / targetFrameRate;
                        break;
                    case "input":
                        input = (InputSystem)ob;
                        input.initialize();
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

        public static long getCurrentMillis()
        {
            return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        public static void endGame()
        {
            endGameFlag = true;
        }

        static void Main(string[] args)
        {
            long timeInMillisecondsStart, timeInMillisecondsEnd;
            long interval;
            int sleep;
            bool physUpdate = false;
            bool physDebug = false;



            // Setup the engine.
            setup();


            // Start the game running.
            runningGame.initialize();

            phys.GravityModifier = 0.1f;
            // This is our game loop.

            if (getEnvironmentalVariable("physics_debug") == "1")
            {
                physDebug = true;
            }

            while (!endGameFlag)
            {
                timeInMillisecondsStart = getCurrentMillis();
                
                // Clear the screen.
                Bootstrap.getDisplay().clearDisplay();

                // Update 
                runningGame.update();
                // Input

                if (runningGame.isRunning() == true)
                {

                    // Get input, which works at 50 FPS to make sure it doesn't interfere with the 
                    // variable frame rates.
                    input.getInput();

                    // Update runs as fast as the system lets it.  Any kind of movement or counter 
                    // increment should be based then on the deltaTime variable.
                    GameObjectManager.getInstance().update();

                    // This will update every 20 milliseconds or thereabouts.  Our physics system aims 
                    // at a 50 FPS cycle.
                    if (phys.willTick())
                    {
                        GameObjectManager.getInstance().prePhysicsUpdate();
                    }

                    // Update the physics.  If it's too soon, it'll return false.   Otherwise 
                    // it'll return true.
                    physUpdate = phys.update();

                    if (physUpdate)
                    {
                        // If it did tick, give every object an update
                        // that is pinned to the timing of the physics system.
                        GameObjectManager.getInstance().physicsUpdate();
                    }

                    if (physDebug) {
                        phys.drawDebugColliders();
                    }

                    // Execute display predraw
                    displayEngine.preDraw();

                    // Let Game draw to the screen
                    runningGame.draw();

                    // Let GameObjects draw to the screen
                    GameObjectManager.getInstance().drawUpdate();

                }

                // Render the screen.
                Bootstrap.getDisplay().display();

                // Timing
                timeInMillisecondsEnd = getCurrentMillis();
                interval = timeInMillisecondsEnd - timeInMillisecondsStart;

                sleep = (int)(millisPerFrame - interval);
                if (sleep >= 0)
                {
                    Thread.Sleep(sleep);
                }


                deltaTime = (getCurrentMillis() - timeInMillisecondsStart) / 1000.0f;

                Console.WriteLine(deltaTime);

            } 


        }

        private static void RenderThread()
        {

            // Render Loop
            while (!endGameFlag)
            {

            }
        }

    }
}
