﻿/*
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
    class GameObject : CollisionHandler
    {
        private Transform3D transformOld;
        private Transform3DNew transform;
        private bool transient;
        private bool toBeDestroyed;
        private bool visible;
        private PhysicsBody myBody;
        private List<string> tags;

        protected Game runningGame { get; private set; }

        public void addTag(string str)
        {
            if (tags.Contains(str))
            {
                return;
            }

            tags.Add(str);
        }

        public void removeTag(string str)
        {
            tags.Remove(str);
        }

        public bool checkTag(string tag)
        {
            return tags.Contains(tag);
        }

        public String getTags()
        {
            string str = "";

            foreach (string s in tags)
            {
                str += s;
                str += ";";
            }

            return str;
        }

        public void setPhysicsEnabled()
        {
            MyBody = new PhysicsBody(this);
        }


        public bool queryPhysicsEnabled()
        {
            if (MyBody == null)
            {
                return false;
            }
            return true;
        }

        internal Transform3D TransformOld
        {
            get => transformOld;
        }

        internal Transform3DNew Transform
        {
            get => transform;
        }

        internal Transform Transform2D
        {
            get => (Transform)transformOld;
        }


        public bool Visible
        {
            get => visible;
            set => visible = value;
        }
        public bool Transient { get => transient; set => transient = value; }
        public bool ToBeDestroyed { get => toBeDestroyed; set => toBeDestroyed = value; }
        internal PhysicsBody MyBody { get => myBody; set => myBody = value; }

        public virtual void initialize()
        {
        }

        public virtual void update()
        {

        }

        public virtual void physicsUpdate()
        {
        }

        public virtual void prePhysicsUpdate()
        {
        }

        public virtual void drawUpdate()
        {
        }

        public GameObject()
        {
            GameObjectManager.getInstance().addGameObject(this);

            transformOld = new Transform3D(this);
            transform = new Transform3DNew();
            visible = false;

            runningGame = Bootstrap.getRunningGame();

            ToBeDestroyed = false;
            tags = new List<string>();

            this.initialize();

        }

        public void checkDestroyMe()
        {

            if (!transient)
            {
                return;
            }

            /*
            if (TransformOld.X > 0 && TransformOld.X < Bootstrap.getDisplay().getWidth())
            {
                if (TransformOld.Y > 0 && TransformOld.Y < Bootstrap.getDisplay().getHeight())
                {
                    return;
                }
            }*/


            ToBeDestroyed = true;

        }

        public virtual void killMe()
        {
            PhysicsManager.getInstance().removePhysicsObject(myBody);

            myBody = null;
            transformOld = null;
        }

        public virtual void onCollisionEnter(PhysicsBody x)
        {
            
        }

        public virtual void onCollisionExit(PhysicsBody x)
        {
            
        }

        public virtual void onCollisionStay(PhysicsBody x)
        {
            
        }
    }
}
