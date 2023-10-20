/*
*
*   This manager class makes sure update gets called when it should on all the game objects, 
*       and also handles the pre-physics and post-physics ticks.  It also deals with 
*       transient objects (like bullets) and removing destroyed game objects from the system.
*   @author Michael Heron
*   @version 1.0
*   
*/

using System.Collections.Generic;

namespace Shard.Shard.GameObjects
{
    class GameObjectManager
    {
        private static GameObjectManager gameObjectManager;
        private List<GameObject> myObjects;

        private GameObjectManager()
        {
            myObjects = new List<GameObject>();
        }

        public static GameObjectManager GetInstance()
        {
            if (gameObjectManager == null)
            {
                gameObjectManager = new GameObjectManager();
            }

            return gameObjectManager;
        }

        public static void CreateGameObject(GameObject gob)
        {
            GetInstance().myObjects.Add(gob);
            gob.Initialize();
        }

        public void Draw()
        {
            GameObject gob;
            for (int i = 0; i < myObjects.Count; i++)
            {
                gob = myObjects[i];

                gob.Draw();
            }
        }

        public void Update()
        {
            List<int> toDestroy = new List<int>();
            GameObject gob;
            for (int i = 0; i < myObjects.Count; i++)
            {
                gob = myObjects[i];

                gob.Update();


                if (gob.ToBeDestroyed == true)
                {
                    toDestroy.Add(i);
                }
            }

            if (toDestroy.Count > 0)
            {
                for (int i = toDestroy.Count - 1; i >= 0; i--)
                {
                    gob = myObjects[toDestroy[i]];
                    myObjects[toDestroy[i]].OnDestroy();
                    myObjects.RemoveAt(toDestroy[i]);

                }
            }

            toDestroy.Clear();
        }

    }
}
