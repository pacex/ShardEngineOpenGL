using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StbImageSharp;
using OpenTK.Graphics.OpenGL;

namespace Shard
{
    class Texture
    {
        public int Handle { get; private set; }

        public Texture(string path) : this(path, TextureWrapMode.Repeat, TextureMinFilter.LinearMipmapLinear, TextureMagFilter.Linear) { }

        public Texture(string path, TextureWrapMode wrapMode, TextureMinFilter minFilter, TextureMagFilter magFilter) {

            // Load texture from file
            string assetParentDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.Parent.FullName;

            StbImage.stbi_set_flip_vertically_on_load(1); 
            ImageResult image = ImageResult.FromStream(File.OpenRead(assetParentDirectory + "\\Assets\\" + path), ColorComponents.RedGreenBlueAlpha);

            // Upload texture to GPU memory
            Handle = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, Handle);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);

            // Set texture parameters
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)wrapMode);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)wrapMode);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)minFilter);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)magFilter);

            // Generate Mipmaps
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public void Use(TextureUnit textureUnit = TextureUnit.Texture0)
        {
            GL.ActiveTexture(textureUnit);
            GL.BindTexture(TextureTarget.Texture2D, Handle);
        }

        


    }
}
