﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard.Shard.Graphics
{
    /*
     * A GameObject with a mesh and a texture that draws itself automatically.
     * 
     */

    class VisualGameObject : GameObject
    {
        public Mesh Mesh;
        public Texture Texture;

        public VisualGameObject(Mesh mesh, Texture texture = null) : base()
        {
            Mesh = mesh;
            Texture = texture;
        }

        

        public override void Draw()
        {
            if (Mesh != null)
            {

                Bootstrap.Display.Model = Transform.ToMatrix();
                Shader.ApplyDefaultShader(Texture);
                Mesh.Draw();
                Shader.Reset();
            }
        }

        public override void Initialize()
        {
        }

        public override void Update()
        {
        }

        public override void OnDestroy()
        {
        }
    }
}
