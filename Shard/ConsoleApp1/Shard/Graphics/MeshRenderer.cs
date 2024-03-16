using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shard.Shard.GameObjects;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Shard.Shard.Graphics
{
    class MeshRenderer : Component
    {
        

        private int vertexArrayObject;
        private int vertexBufferObject;

        public Mesh Mesh { get; set; }
        public Texture Texture { get; set; }

        public MeshRenderer(Mesh mesh, Texture texture, GameObject host) : base(host)
        {
            Mesh = mesh;
            Texture = texture;

            vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayObject);

            vertexBufferObject = GL.GenBuffer();

            
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, Mesh.Vertices.Length * sizeof(float), Mesh.Vertices, BufferUsageHint.StaticDraw);
            
            if (Mesh is AnimatedMesh)
            {
                // Vertex Attributes, animated mesh
                // [0..2       , 3..5           , 6..7       , 8..11           , 12..15           ]
                // (0) vec3 pos, (1) vec3 normal, (2) vec2 uv, (3) vec4 weights, (4) uint4 boneIds
                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 16 * sizeof(float), 0);
                GL.EnableVertexAttribArray(0);

                GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 16 * sizeof(float), 3 * sizeof(float));
                GL.EnableVertexAttribArray(1);

                GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 16 * sizeof(float), 6 * sizeof(float));
                GL.EnableVertexAttribArray(2);

                GL.VertexAttribPointer(3, 4, VertexAttribPointerType.Float, false, 16 * sizeof(float), 8 * sizeof(float));
                GL.EnableVertexAttribArray(3);

                GL.VertexAttribPointer(4, 4, VertexAttribPointerType.Float, false, 16 * sizeof(float), 12 * sizeof(float));
                GL.EnableVertexAttribArray(4);
            }
            else
            {
                // Vertex Attributes, static mesh
                // [0..2       , 3..5           , 6..7      ]
                // (0) vec3 pos, (1) vec3 normal, (2) vec2 uv
                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
                GL.EnableVertexAttribArray(0);

                GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
                GL.EnableVertexAttribArray(1);

                GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));
                GL.EnableVertexAttribArray(2);
            }
            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }
        public override void Draw()
        {
            if (Mesh != null)
            {
                Bootstrap.Display.Model = Host.Transform.ToMatrix();

                if (Mesh is AnimatedMesh) {
                    AnimatedMesh aMesh = (AnimatedMesh)Mesh;

                    // TODO: remove debug code
                    float[] boneMatrices = new float[aMesh.BoneHierarchy.NumBones * 16];
                    float t = 24f * (Bootstrap.GetCurrentMillis() % 875) / 1000.0f;
                    aMesh.BoneHierarchy.ComputeBoneMatrices(ref boneMatrices, Matrix4.Identity, aMesh.Animation, t);
                    Console.WriteLine((long)t);
                    Shader.ApplyAnimatedShader(Texture, boneMatrices);
                }
                else
                    Shader.ApplyDefaultShader(Texture);

                GL.BindVertexArray(vertexArrayObject);
                GL.DrawElements(PrimitiveType.Triangles, Mesh.Indices.Length, DrawElementsType.UnsignedInt, Mesh.Indices);
                GL.BindVertexArray(0);
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
