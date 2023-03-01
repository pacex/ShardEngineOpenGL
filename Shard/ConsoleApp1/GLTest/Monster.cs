using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace Shard.GLTest
{
    class Monster : GameObject
    {
        private static AnimatedMesh mesh;
        private static Texture texture;


        public override void initialize()
        {
            base.initialize();
            if (texture == null) { texture = new Texture("GLTest\\monster_idle.png", TextureWrapMode.MirroredRepeat, TextureMinFilter.NearestMipmapLinear, TextureMagFilter.Nearest, 0, 3); }
            if (mesh == null) { mesh = ObjLoader.LoadMesh("GLTest\\billboard.obj").ToAnimatedMesh(texture, 6, 8.0f); }
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

            Matrix4 v = DisplayOpenGL.GetInstance().View;
            DisplayOpenGL.GetInstance().Model = Matrix4.CreateScale(2.0f);
            //DisplayOpenGL.GetInstance().View = Matrix4.Identity;
            mesh.Draw();

            DisplayOpenGL.GetInstance().View = v;
        }
    }
}
