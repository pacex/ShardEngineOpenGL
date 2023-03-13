using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Common;

namespace Shard.GLTest
{
    class Bullet: GameObject
    {
        private float acc;
        Transform3DNew playerTransform;
        private VisualGameObject visualBullet;
        public Bullet(Vector3 translation): base()
        {
            this.Transform.Translation = translation;
        }
        public override void initialize()
        {
            base.initialize();
            setPhysicsEnabled();
           
            acc = 0.3f;
           // visualBullet = new VisualGameObject(ObjLoader.LoadMesh("GLTest\\bullet.obj"), new Texture("GLTest\\texture_level2.png", TextureWrapMode.MirroredRepeat, TextureMinFilter.NearestMipmapLinear, TextureMagFilter.Nearest, 0, 3));
            MyBody.addCubeCollider(1.0f,1.0f,1.0f);
            
            
            addTag("Bullet");
            MyBody.MaxForce = 10.0f;
           // MyBody.PassThrough = true;
           // this.Transient = true;
           // visualBullet.Transform.Translation = new Vector3(0.0f, 0.0f, 0.0f);
        }
        public override void update()
        {
            if (MyBody.getForce().Equals(0f))
            {
                killMe();
            }
        }
        public void FireMe(System.Numerics.Vector2 direction)
        {
            float force = acc;
            MyBody.addForce(direction, force);
        }
        public override void onCollisionEnter(PhysicsBody x)
        {
            base.onCollisionEnter(x);
           
            
           
            
          
            killMe();
        }
        
        public override void drawUpdate()
        {
            base.drawUpdate();
        }
    }
}

    
