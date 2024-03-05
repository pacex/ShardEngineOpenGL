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

namespace Shard.Shard.GameObjects
{
    abstract class GameObject
    {
        private Transform transform;
        private bool toBeDestroyed;
        private Dictionary<Type, Component> components;

        public Transform Transform
        {
            get => transform;
        }

        public bool ToBeDestroyed { get => toBeDestroyed; }

        public void Initialize()
        {
            foreach (Component c in components.Values)
            {
                c.Initialize();
            }
        }

        public void Update()
        {
            foreach (Component c in components.Values)
            {
                c.Update();
            }
        }

        public void Draw()
        {
            foreach (Component c in components.Values)
            {
                c.Draw();
            }
        }

        public void OnDestroy()
        {
            foreach (Component c in components.Values)
            {
                c.OnDestroy();
            }
        }

        public GameObject()
        {
            transform = new Transform();
            toBeDestroyed = false;
            components = new Dictionary<Type, Component>();
        }

        public void Destroy()
        {
            toBeDestroyed = true;
        }

        public void AddComponent(Component c)
        {
            Type t = c.GetType();
            if (components.ContainsKey(t))
            {
                throw new ArgumentException("Component of type " + t.Name + " already added!");
            }
            components.Add(t, c);
        }

        public T GetComponent<T>() where T : Component
        {
            return (T)components[typeof(T)];
        }

    }
}
