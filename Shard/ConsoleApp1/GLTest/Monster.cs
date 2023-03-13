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
        private static AnimatedMesh mesh;
        private static Texture texture;


        public override void initialize()
        {
            base.initialize();
            setPhysicsEnabled();
            if (texture == null) { texture = new Texture("GLTest\\monster_idle.png", TextureWrapMode.MirroredRepeat, TextureMinFilter.Nearest, TextureMagFilter.Nearest, 0, 2); }
            if (mesh == null) { mesh = ObjLoader.LoadMesh("GLTest\\billboard.obj").ToAnimatedMesh(texture, 6, 8.0f); }
            MyBody.addCubeCollider(1.0f, 1.0f, 2.0f);
        }

        public override void update()
        {
            base.update();
        }

        public override void physicsUpdate()
        {
            base.physicsUpdate();
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
                x.Parent.killMe();
                killMe();
            }
        }
    }
}
