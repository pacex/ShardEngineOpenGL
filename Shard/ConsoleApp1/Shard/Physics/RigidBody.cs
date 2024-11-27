using OpenTK.Mathematics;
using Shard.Shard.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard.Shard.Physics
{
    class RigidBody : Component
    {
        public RigidBody(GameObject parent) : base(parent) {
            Velocity = Vector3.Zero;
            Gravity = new Vector3(0.0f, 0.0f, -0.5f);
            Friction = 0.98f;
        }

        public Vector3 Velocity { get; private set; }
        public Vector3 Gravity { get; private set; }
        public float Friction { get; private set; }

        public void AddForce(Vector3 force)
        {
            Velocity += force;
        }

        public override void Update()
        {
            // Apply Gravity
            Velocity += Gravity * Bootstrap.DeltaTime;

            // Apply Friction
            Velocity *= Friction;

            // Collide and Reflect
            Vector3 reflected;
            bool collided = Host.GetComponent<DynamicBody>().MoveAndSlide(Velocity, out reflected);
            if (collided)
                if (reflected.Length > 0.05f)
                    Velocity = reflected * 0.8f;
                else
                    Velocity = Vector3.Zero;
        }

        public override void Draw() {}
        public override void Initialize() {}
        public override void OnDestroy() {}
    }
}
