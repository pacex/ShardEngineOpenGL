﻿/*
*
*   The physics body class does... a lot.  It handles the computation of internal values such as 
*       the min and max values for X and Y (used by the Sweep and Prune algorithm, as well as 
*       collision detection in general).  It registers and processes the colliders that belong to 
*       an object.  It handles the application of forces and torque as well as drag and angular drag.
*       It lets an object add colliders, and then exposes those colliders for narrow phase collision 
*       detection.  It handles some naive default collision responses such as a simple reflection
*       or 'stop on collision'.
*       
*   Important to note though that while this is called a PhysicsBody, no claims are made for the 
*       *accuracy* of the physics.  If you are planning to do anything that requires the physics
*       calculations to be remotely correct, you're going to have to extend the engine so it does 
*       that.  All I'm interested in here is showing you how it's *architected*. 
*       
*   This is also the subsystem which I am least confident about people relying on, because it is 
*       virtually untestable in any meaningful sense.  I spent three days trying to track down a 
*       bug that mean that an object would pass through another one at a rate of approximately
*       once every half hour...
*       
*   @author Michael Heron
*   @version 1.0
*   
*   Several substantial contributions to the code made by others:
*   @author Mårten Åsberg (see Changelog for 1.0.1)
*   
*/

using OpenTK.Core;
using OpenTK.Mathematics;
using Shard.GLTest;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;

namespace Shard
{
    class PhysicsBody
    {
        List<Collider> myColliders;
        List<Collider> collisionCandidates;
        List<Collider3D> my3DColliders;
        List<Collider3D> collision3Dcandidates;
        GameObject parent;
        CollisionHandler colh;
        Transform3DNew trans;
        private float angularDrag;
        private float drag;
        private float torque;
        private System.Numerics.Vector2 force;
        private float mass;
        private double timeInterval;
        private float maxForce, maxTorque;
        private bool kinematic;
        private bool stopOnCollision;
        private bool reflectOnCollision;
        private bool impartForce;
        private bool passThrough;
        private bool usesGravity;
        private Color debugColor;
        public Color DebugColor { get => debugColor; set => debugColor = value; }

        private float[] minAndMaxX;
        private float[] minAndMaxY;

        public void applyGravity(float modifier, System.Numerics.Vector2 dir)
        {

            System.Numerics.Vector2 gf = dir * modifier;

            addForce(gf);

        }

        public float AngularDrag { get => angularDrag; set => angularDrag = value; }
        public float Drag { get => drag; set => drag = value; }
        internal GameObject Parent { get => parent; set => parent = value; }
        internal Transform3DNew Trans { get => trans; set => trans = value; }
        public float Mass { get => mass; set => mass = value; }
        public float[] MinAndMaxX { get => minAndMaxX; set => minAndMaxX = value; }
        public float[] MinAndMaxY { get => minAndMaxY; set => minAndMaxY = value; }
        public float MaxForce { get => maxForce; set => maxForce = value; }
        public float MaxTorque { get => maxTorque; set => maxTorque = value; }
        public bool Kinematic { get => kinematic; set => kinematic = value; }
        public bool PassThrough { get => passThrough; set => passThrough = value; }
        public bool UsesGravity { get => usesGravity; set => usesGravity = value; }
        public bool StopOnCollision { get => stopOnCollision; set => stopOnCollision = value; }
        public bool ReflectOnCollision { get => reflectOnCollision; set => reflectOnCollision = value; }
        public bool ImpartForce { get => this.impartForce; set => this.impartForce = value; }

        public void drawMe()
        {
            foreach (Collider col in myColliders)
            {
                col.drawMe(DebugColor);
            }
        }

        public float[] getMinAndMax(bool x)
        {
            float min = Int32.MaxValue;
            float max = -1 * min;
            float[] tmp;

            foreach (Collider col in myColliders)
            {

                if (x)
                {
                    tmp = col.MinAndMaxX;
                }
                else
                {
                    tmp = col.MinAndMaxY;
                }


                if (tmp[0] < min)
                {
                    min = tmp[0];
                }

                if (tmp[1] > max)
                {
                    max = tmp[1];
                }
            }


            return new float[2] { min, max };
        }

        public PhysicsBody(GameObject p)
        {
            DebugColor = Color.Green;

            myColliders = new List<Collider>();
            collisionCandidates = new List<Collider>();

            Parent = p;
            Trans = p.Transform;
            colh = (CollisionHandler)p;

            AngularDrag = 0.01f;
            Drag = 0.01f;
            Drag = 0.01f;
            Mass = 1;
            MaxForce = 10;
            MaxTorque = 2;
            usesGravity = false;
            stopOnCollision = true;
            reflectOnCollision = false;

            Kinematic = false;

            MinAndMaxX = new float[2];
            MinAndMaxY = new float[2];

            timeInterval = PhysicsManager.getInstance().TimeInterval;
            //            Debug.getInstance().log ("Setting physics enabled");

            PhysicsManager.getInstance().addPhysicsObject(this);
        }

        public void setKinematic()
        {
            Kinematic = true;
            PhysicsManager.getInstance().addToKinematic(this);
        }

        public void addTorque(float dir)
        {
            if (Kinematic)
            {
                return;
            }

            torque += dir / Mass;

            if (torque > MaxTorque)
            {
                torque = MaxTorque;
            }

            if (torque < -1 * MaxTorque)
            {
                torque = -1 * MaxTorque;
            }


        }

        public void reverseForces(float prop)
        {
            if (Kinematic)
            {
                return;
            }

            force *= -prop;
        }

        public void impartForces(PhysicsBody other, float massProp)
        {
            other.addForce(force * massProp);

            recalculateColliders();

        }

        public void stopForces()
        {
            force = System.Numerics.Vector2.Zero;
        }

        public void reflectForces(System.Numerics.Vector2 impulse)
        {
            System.Numerics.Vector2 reflect = new System.Numerics.Vector2(0, 0);

            Debug.Log ("Reflecting " + impulse);

            // We're being pushed to the right, so we must have collided with the right.
            if (impulse.X > 0)
            {
                reflect.X = -1;
            }

            // We're being pushed to the left, so we must have collided with the left.
            if (impulse.X < 0)
            {
                reflect.X = -1;

            }

            // We're being pushed upwards, so we must have collided with the top.
            if (impulse.Y < 0)
            {
                reflect.Y = -1;
            }

            // We're being pushed downwards, so we must have collided with the bottom.
            if (impulse.Y > 0)
            {
                reflect.Y = -1;

            }


            force *= reflect;

            Debug.Log("Reflect is " + reflect);

        }

        public void reduceForces(float prop) {
            force *= prop;
        }

        public void addForce(System.Numerics.Vector2 dir, float force) {
            addForce(dir * force);
        }

        public void addForce(System.Numerics.Vector2 dir)
        {
            if (Kinematic)
            {
                return;
            }

            dir /= Mass;

            // Set a lower bound.
            if (dir.LengthSquared() < 0.0001)
            {
                return;
            }

            force += dir;

            // Set a higher bound.
            if (force.Length() > MaxForce)
            {
                force = System.Numerics.Vector2.Normalize(force) * MaxForce;
            }
        }

        public void recalculateColliders()
        {
            foreach (Collider col in getColliders())
            {
                col.recalculate();
            }

            MinAndMaxX = getMinAndMax(true);
            MinAndMaxY = getMinAndMax(false);
        }

        public void physicsTick(float physDeltaTime)
        {
            List<System.Numerics.Vector2> toRemove;

            float rot = 0;


            toRemove = new List<System.Numerics.Vector2>();

            rot = torque;

            if (Math.Abs(torque) < AngularDrag)
            {
                torque = 0;
            }
            else
            {
                torque -= Math.Sign(torque) * AngularDrag;
            }

            trans.Rotate(OpenTK.Mathematics.Quaternion.FromAxisAngle(OpenTK.Mathematics.Vector3.UnitZ, rot * ((float)Math.PI / 180.0f)));


            // TODO: scale application of force by physDeltaTime
            if (!kinematic && force.Length() > 0.0f)
            {
                // X direction
                if (PhysicsManager.getInstance().queryKinematic(this, new OpenTK.Mathematics.Vector2(force.X, 0.0f)))
                {
                    force.X = 0.0f;
                }
                else
                {
                    trans.Translate(new OpenTK.Mathematics.Vector3(force.X, 0.0f, 0.0f));
                }

                // Y direction
                if (PhysicsManager.getInstance().queryKinematic(this, new OpenTK.Mathematics.Vector2(0.0f, force.Y)))
                {
                    force.Y = 0.0f;

                }
                else
                {
                    trans.Translate(new OpenTK.Mathematics.Vector3(0.0f, force.Y, 0.0f));
                }
            }

			


            float mag = force.Length();
            if (mag < Drag)
            {
                stopForces();
            }
            else if (mag > 0)
            {
                this.force = (this.force / mag) * (mag - Drag);
            }



        }
        public ColliderSphere addSphereCollider(float radius)
        {
            ColliderSphere cr = new ColliderSphere((CollisionHandler)parent, parent.Transform,radius);
            add3DCollider(cr);
            return cr;

        }
        public ColliderCube addCubeCollider(float width, float height, float depth)
        {
            ColliderCube cr = new ColliderCube((CollisionHandler)parent, parent.Transform, width, height,depth);
            add3DCollider(cr);
            return cr;

        }
        public ColliderRect addRectCollider(float width, float height)
        {
            ColliderRect cr = new ColliderRect((CollisionHandler)parent, parent.Transform, width, height);

            addCollider(cr);

            return cr;
        }

        public ColliderCircle addCircleCollider()
        {
            ColliderCircle cr = new ColliderCircle((CollisionHandler)parent, parent.TransformOld);

            addCollider(cr);

            return cr;
        }

        public ColliderCircle addCircleCollider(int x, int y, int rad)
        {
            ColliderCircle cr = new ColliderCircle((CollisionHandler)parent, parent.TransformOld, x, y, rad);

            addCollider(cr);

            return cr;
        }


        public void addCollider(Collider col)
        {
            myColliders.Add(col);
        }
        public void add3DCollider(Collider3D col)
        {
            my3DColliders.Add(col);
        }

        public List<Collider> getColliders()
        {
            return myColliders;
        }
        public List<Collider3D> get3DColliders()
        {
            return my3DColliders;
        }
         
        public System.Numerics.Vector2? checkCollisions(System.Numerics.Vector2 other)
        {
            System.Numerics.Vector2? d;


            foreach (Collider c in myColliders)
            {
                d = c.checkCollision(other);

                if (d != null)
                {
                    return d;
                }
            }

            return null;
        }


        public System.Numerics.Vector2? checkCollisions(Collider other)
        {
            System.Numerics.Vector2? d;

//            Debug.Log("Checking collision with " + other);
            foreach (Collider c in myColliders)
            {
                d = c.checkCollision(other);

                if (d != null)
                {
                    return d;
                }
            }

            return null;
        }
    }
}