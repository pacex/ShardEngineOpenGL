using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using System.Transactions;

namespace Shard.GLTest
{
    class Monster : GameObject
    {
        private AnimatedMesh mesh;
        private static Texture textureIdle;
        private static Texture textureDie;
        long deathTimer = -1;
        Vector3 targetPos;
        private float acc;
        
        public Monster() : base()
        {
            
        }


        public override void initialize()
        {
            base.initialize();
            setPhysicsEnabled();
            if (textureIdle == null) { textureIdle = new Texture("GLTest\\monster_idle.png", TextureWrapMode.MirroredRepeat, TextureMinFilter.Nearest, TextureMagFilter.Nearest, 0, 2); }
            if (textureDie == null) { textureDie= new Texture("GLTest\\monster_die.png", TextureWrapMode.MirroredRepeat, TextureMinFilter.Nearest, TextureMagFilter.Nearest, 0, 2); }
            if (mesh == null) { mesh = ObjLoader.LoadMesh("GLTest\\billboard.obj").ToAnimatedMesh(textureIdle, 6, 8.0f); }
            MyBody.addCubeCollider(1.0f, 1.0f, 2.0f);
            MyBody.MaxForce = 0.11f;
            acc = 0.04f;
            MyBody.Drag = 0.03f;
        }

        public override void update()
        {
            if(Bootstrap.getCurrentMillis() - deathTimer > 5000 && deathTimer > 0)
            {
                ToBeDestroyed = true;
            }

            base.update();
        }

        public override void physicsUpdate()
        {
            base.physicsUpdate();

            if (deathTimer < 0)
            {
                moveToTarget();
            }
            //vector should always have direction towards player
        }

        public override void drawUpdate()
        {
            base.drawUpdate();

            Vector2 billboardForward = -Vector2.UnitX;
            Vector2 toCamera = (DisplayOpenGL.GetInstance().MainCamera.Transform.Translation.Xy - Transform.Translation.Xy).Normalized();

            float angle = -(float)Math.Atan2(Vector2.Dot(billboardForward, toCamera),
                billboardForward.X * toCamera.Y - billboardForward.Y * toCamera.X);

            Matrix4 m =  Matrix4.CreateFromAxisAngle(Vector3.UnitZ, angle) * Matrix4.CreateScale(2.0f);
            m.M41 = Transform.Translation.X; m.M42 = Transform.Translation.Y; m.M43 = Transform.Translation.Z;

            DisplayOpenGL.GetInstance().Model = m;
            mesh.Draw();

        }
        public override void onCollisionEnter(PhysicsBody x)
        {
            base.onCollisionEnter(x);
            if (x.Parent is Bullet)
            {
                x.Parent.ToBeDestroyed = true;

                triggerDeath();
                
            }
        }

        private void triggerDeath()
        {
            deathTimer = Bootstrap.getCurrentMillis();
            mesh.ChangeAnimation(textureDie, 8, 8f, AnimationMode.End);
        }

        public bool isDead()
        {
            return deathTimer >= 0;
        }

        private void moveToTarget()
        {
            System.Numerics.Vector2 player2dpos = new System.Numerics.Vector2(targetPos.X, targetPos.Y);
            System.Numerics.Vector2 monster2dpos = new System.Numerics.Vector2(Transform.Translation.X, Transform.Translation.Y);
            System.Numerics.Vector2 direction = (player2dpos - monster2dpos);

            System.Numerics.Vector2.Normalize(direction);
            MyBody.addForce(direction, acc);
        }
        public void targetPosition(Vector3 targetPos)
        {
            this.targetPos = targetPos;
        }
    }
}
