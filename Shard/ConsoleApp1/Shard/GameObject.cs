/*
*
*   Anything that is going to be an interactable object in your game should extend from GameObject.  
*       It handles the life-cycle of the objects, some useful general features (such as tags), and serves 
*       as the convenient facade to making the object work with the physics system.  It's a good class, Bront.
*   @author Michael Heron
*   @version 1.0
*   
*/

using System;
using System.Collections.Generic;

namespace Shard
{
    abstract class GameObject
    {
        private Transform transform;
        private bool toBeDestroyed;

        public Transform Transform
        {
            get => transform;
        }

        public bool ToBeDestroyed { get => toBeDestroyed; }

        public abstract void Initialize();

        public abstract void Update();

        public abstract void Draw();

        public abstract void OnDestroy();

        public GameObject()
        {
            GameObjectManager.GetInstance().AddGameObject(this);

            transform = new Transform();
            toBeDestroyed = false;

            Initialize();
        }

        public void Destroy()
        {
            toBeDestroyed = true;
        }

    }
}
