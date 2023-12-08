using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;

namespace Shard.Shard.Graphics
{
    class DisplayOpenGL
    {

        private static DisplayOpenGL instance = null;

        public static DisplayOpenGL GetInstance()
        {
            if (instance == null)
            {
                instance = new DisplayOpenGL();
            }
            return instance;
        }

        private DisplayOpenGL()
        {
            Model = Matrix4.Identity;
            View = Matrix4.Identity;
            Projection = Matrix4.Identity;
            MainCamera = null;
            fbo = 0;
        }

        public WindowGL Window { get; private set; }

        public Matrix4 Model;
        public Matrix4 View;
        public Matrix4 Projection;

        public Camera MainCamera;

        private int fbo;
        private int colTex;
        private int depthTex;
        private int normTex;

        public void Initialize()
        {
            // Setup Window
            Window = new WindowGL(Color4.Black);
            Window.Initialize();

            initFBO();
        }

        private void initFBO()
        {
            // Setup FBO
            Vector2i dim = Window.GetWindowSize();
            fbo = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, fbo);
            GL.Enable(EnableCap.DepthTest);

            colTex = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, colTex);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, dim.X, dim.Y, 0, PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, colTex, 0);

            normTex = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, normTex);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, dim.X, dim.Y, 0, PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment1, TextureTarget.Texture2D, normTex, 0);

            depthTex = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, depthTex);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.DepthComponent24, dim.X, dim.Y, 0, PixelFormat.DepthComponent, PixelType.Float, IntPtr.Zero);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, TextureTarget.Texture2D, depthTex, 0);

            Console.WriteLine(GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer));

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        public void Resize()
        {
            // Delete old textures and framebuffer
            GL.DeleteTexture(colTex);
            GL.DeleteTexture(normTex);
            GL.DeleteTexture(depthTex);
            GL.DeleteFramebuffer(fbo);

            initFBO();
        }

        public void ClearDisplay()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        public void ProcessWindowEvents()
        {
            Window.ProcessWindowEvents();
        }

        public void PreDraw()
        {
            if (MainCamera != null)
            {
                View = MainCamera.GetViewMatrix();
                Projection = MainCamera.GetProjMatrix();
            }

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, fbo);
            DrawBuffersEnum[] buffers = { DrawBuffersEnum.ColorAttachment0, DrawBuffersEnum.ColorAttachment1 };
            GL.DrawBuffers(2, buffers);
            ClearDisplay();

        }

        public void Display()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

            Shader.GetGUIShader().Use();

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, colTex);
            Shader.GetGUIShader().SetSamplerTextureUnit("diffuseTexture", TextureUnit.Texture0);

            GL.ActiveTexture(TextureUnit.Texture1);
            GL.BindTexture(TextureTarget.Texture2D, normTex);
            Shader.GetGUIShader().SetSamplerTextureUnit("normalTexture", TextureUnit.Texture1);

            GL.ActiveTexture(TextureUnit.Texture2);
            GL.BindTexture(TextureTarget.Texture2D, depthTex);
            Shader.GetGUIShader().SetSamplerTextureUnit("depthTexture", TextureUnit.Texture2);

            Mesh.DrawFullscreenQuad();

            Window.Display();
        }
    }
}
