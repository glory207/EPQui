using System; using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation.Text;
using StbImageSharp;
using System.IO;
using System.Windows.Shapes;

namespace EPQui
{
    public class Texture
    {
        public int ID;
        public int unit;
        public string type;
        ImageResult image;
        public string path;
        public Texture(string path, string texType, int slot,PixelFormat pixelFormat)
        {
            this.path = path;
            type = texType;
 unit = slot;
           



            StbImage.stbi_set_flip_vertically_on_load(1);

            ID = GL.GenTexture();
            GL.ActiveTexture(TextureUnit.Texture0 + 0);
           
            GL.BindTexture(TextureTarget.Texture2D, ID);
            using (Stream stream = File.OpenRead(path))
            {
                image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, pixelFormat, PixelType.UnsignedByte, image.Data);
            }

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
       
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
       
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            GL.BindTexture(TextureTarget.Texture2D, 0);
        }
        public void Bind() {
            GL.ActiveTexture(TextureUnit.Texture0 + unit);
            GL.BindTexture(TextureTarget.Texture2D, ID); }
        public void Unbind() { GL.ActiveTexture(TextureUnit.Texture0); GL.BindTexture(TextureTarget.Texture2D,0); }
        public void Delete() {
            GL.DeleteTexture(ID);
        }

        public void texUnit(Shader shader, string uniform, uint unit){
            int texUni = GL.GetUniformLocation(shader.ID, uniform);
            shader.Activate();
	        GL.Uniform1(texUni, unit);
        }

}
}
