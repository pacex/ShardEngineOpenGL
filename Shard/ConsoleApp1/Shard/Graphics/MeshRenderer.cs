using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shard.Shard.GameObjects;

namespace Shard.Shard.Graphics
{
    class MeshRenderer : Component
    {
        public Mesh Mesh { get; set; }
        public Texture Texture { get; set; }

        public MeshRenderer(Mesh mesh, Texture texture, GameObject host) : base(host)
        {
            Mesh = mesh;
            Texture = texture;
        }
        public override void Draw()
        {
            if (Mesh != null)
            {
                Bootstrap.Display.Model = Host.Transform.ToMatrix();
                Shader.ApplyDefaultShader(Texture);
                Mesh.Draw();
                Shader.Reset();
            }
        }

        public override void Initialize()
        {
        }

        public override void OnDestroy()
        {
        }

        public override void Update()
        {
        }
    }
}
