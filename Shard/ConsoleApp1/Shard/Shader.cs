using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using OpenTK.Graphics.OpenGL;
using OpenTK.Compute.OpenCL;
using OpenTK.Mathematics;

namespace Shard
{
    class Shader : IDisposable
    {
        private static Shader defaultShader = null;
        private static Shader wireframeShader = null;
        private static Shader animatedShader = null;

        public static Shader GetDefaultShader()
        {
            if (defaultShader == null)
                defaultShader = new Shader("Shaders/default.vert", "Shaders/default.frag");

            return defaultShader;
        }

        public static Shader GetWireframeShader()
        {
            if (wireframeShader == null)
            {
                wireframeShader = new Shader("Shaders/wireframe.vert", "Shaders/wireframe.frag");
            }
            return wireframeShader;
        }

        public static Shader GetAnimatedShader()
        {
            if (animatedShader == null)
            {
                animatedShader = new Shader("Shaders/default.vert", "Shaders/animated.frag");
            }
            return animatedShader;
        }


        public static void ApplyDefaultShader(Texture texture)
        {
            DisplayOpenGL display = Bootstrap.GetDisplayOpenGL();
           
            texture.Use(TextureUnit.Texture0);

            GetDefaultShader().Use();
            GetDefaultShader().SetSamplerTextureUnit("texture0", TextureUnit.Texture0);

            GL.UniformMatrix4(GL.GetUniformLocation(GetDefaultShader().Handle, "model"), false, ref display.Model);
            GL.UniformMatrix4(GL.GetUniformLocation(GetDefaultShader().Handle, "view"), false, ref display.View);
            GL.UniformMatrix4(GL.GetUniformLocation(GetDefaultShader().Handle, "proj"), false, ref display.Projection);

        }

        public static void ApplyWireframeShader(Color4 color)
        {
            DisplayOpenGL display = Bootstrap.GetDisplayOpenGL();

            GetWireframeShader().Use();

            GL.Uniform4(GL.GetUniformLocation(GetWireframeShader().Handle, "color"), color);
            GL.UniformMatrix4(GL.GetUniformLocation(GetWireframeShader().Handle, "model"), false, ref display.Model);
            GL.UniformMatrix4(GL.GetUniformLocation(GetWireframeShader().Handle, "view"), false, ref display.View);
            GL.UniformMatrix4(GL.GetUniformLocation(GetWireframeShader().Handle, "proj"), false, ref display.Projection);
        }

        public static void ApplyAnimatedShader(Texture texture, int frameCount, int frameIndex)
        {
            DisplayOpenGL display = Bootstrap.GetDisplayOpenGL();

            texture.Use(TextureUnit.Texture0);

            GetAnimatedShader().Use();
            GetAnimatedShader().SetSamplerTextureUnit("texture0", TextureUnit.Texture0);

            GL.Uniform1(GL.GetUniformLocation(GetAnimatedShader().Handle, "frameCount"), frameCount);
            GL.Uniform1(GL.GetUniformLocation(GetAnimatedShader().Handle, "frameIndex"), frameIndex);

            GL.UniformMatrix4(GL.GetUniformLocation(GetAnimatedShader().Handle, "model"), false, ref display.Model);
            GL.UniformMatrix4(GL.GetUniformLocation(GetAnimatedShader().Handle, "view"), false, ref display.View);
            GL.UniformMatrix4(GL.GetUniformLocation(GetAnimatedShader().Handle, "proj"), false, ref display.Projection);
        }


        public static void Reset()
        {
            GL.UseProgram(0);
        }


        public int Handle { get; private set; }

        public Shader(string vertexPath, string fragmentPath)
        {
            int vertexShader;
            int fragmentShader;

            // Read shaders from file
            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;

            string vertexShaderSource = File.ReadAllText(projectDirectory + "\\shard\\" + vertexPath);
            string fragmentShaderSource = File.ReadAllText(projectDirectory + "\\shard\\" + fragmentPath);

            // Create and bind shaders
            vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexShaderSource);

            fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragmentShaderSource);

            // Compile shaders
            GL.CompileShader(vertexShader);
            GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out int success);
            if (success == 0)
            {
                string infoLog = GL.GetShaderInfoLog(vertexShader);
                Console.WriteLine(infoLog);
            }

            GL.CompileShader(fragmentShader);
            GL.GetShader(fragmentShader, ShaderParameter.CompileStatus, out success);
            if (success == 0)
            {
                string infoLog = GL.GetShaderInfoLog(fragmentShader);
                Console.WriteLine(infoLog);
            }

            // Link shaders
            Handle = GL.CreateProgram();

            GL.AttachShader(Handle, vertexShader);
            GL.AttachShader(Handle, fragmentShader);
            GL.LinkProgram(Handle);

            GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out success);
            if (success == 0)
            {
                string infoLog = GL.GetProgramInfoLog(Handle);
                Console.WriteLine(infoLog);
            }

            // Cleanup
            GL.DetachShader(Handle, vertexShader);
            GL.DetachShader(Handle, fragmentShader);
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);
        }

        public void Use()
        {
            GL.UseProgram(Handle);
        }

        public int GetAttribLocation(string attribName)
        {
            return GL.GetAttribLocation(Handle, attribName);
        }

        public void SetSamplerTextureUnit(string samplerName, TextureUnit textureUnit)
        {
            int t0 = (int)TextureUnit.Texture0;
            GL.Uniform1(GetAttribLocation(samplerName), (int)textureUnit - t0);
        }

        // Disposal

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                GL.DeleteProgram(Handle);

                disposedValue = true;
            }
        }

        ~Shader()
        {
            GL.DeleteProgram(Handle);
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
