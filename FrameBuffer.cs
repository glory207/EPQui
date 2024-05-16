using System;
using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Mathematics;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Media.Media3D;
using System.Windows.Input;

namespace EPQui
{
    internal class FrameBuffer
    {
        float[] rectangleVertices =
{
	// Coords    // texCoords
	 1.0f, -1.0f,  1.0f, 0.0f,
    -1.0f,  1.0f,  0.0f, 1.0f,
    -1.0f, -1.0f,  0.0f, 0.0f,

     1.0f,  1.0f,  1.0f, 1.0f,
    -1.0f,  1.0f,  0.0f, 1.0f,
     1.0f, -1.0f,  1.0f, 0.0f,
};

        public Shader framebufferProgram;
        public VAO rectVAO;
        public VBO rectVBO;
        public int FBO;
        public int framebufferTexture;
        public int RBO;
        public int PostFBO;
        public int PostframebufferTexture;
        public int PostRBO;
        public int width;
        public int height;
        public Color4 color = new Color4(0.1f, 0.2f, 0.3f, 4.0f);

        public FrameBuffer(int widthf, int heightf)
        {

            framebufferProgram = new Shader("Res/framebuffer.vert", "Res/framebuffer.frag");
            framebufferProgram.Activate();
            GL.Uniform1(GL.GetUniformLocation(framebufferProgram.ID, "screenTexture"), 1);
            updateScreenSize( widthf, heightf);
        }
        public void Bind()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FBO);
            GL.ClearColor(color);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);
        }
        public void destroy()
        {
            GL.DeleteFramebuffer(FBO);
        }
        public void update()
        {
        
           GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, FBO);
           GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, PostFBO);
           GL.BlitFramebuffer(0, 0, width, height, 0, 0, width, height, ClearBufferMask.ColorBufferBit, BlitFramebufferFilter.Nearest);
        
        
        
        
           framebufferProgram.Activate();
           rectVAO.Bind();
           GL.Disable(EnableCap.DepthTest);
           GL.BindTexture(TextureTarget.Texture2D, PostframebufferTexture);
           GL.Uniform1(GL.GetUniformLocation(framebufferProgram.ID, "screenTexture"), 1);
           GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
        
           GL.DeleteFramebuffer(FBO);
           GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        

        }

        public int update(int widthf, int heightf)
        {

            GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, FBO);
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, PostFBO);
            GL.BlitFramebuffer(0, 0, width, height, 0, 0, width, height, ClearBufferMask.ColorBufferBit, BlitFramebufferFilter.Nearest);




            framebufferProgram.Activate();
            rectVAO.Bind();
            GL.Disable(EnableCap.DepthTest);
            GL.BindTexture(TextureTarget.Texture2D, PostframebufferTexture);
            GL.Uniform1(GL.GetUniformLocation(framebufferProgram.ID, "screenTexture"), 1);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);

            byte[] pixel = new byte[4];
            GL.ReadPixels(widthf, heightf, 1, 1, PixelFormat.Rgba, PixelType.UnsignedByte, pixel);
           // Debug.WriteLine(pixel[0].ToString() +" "+ pixel[1].ToString() +" "+ pixel[2].ToString());

            GL.DeleteFramebuffer(FBO);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

            return pixel[0];
        }
        public void updateScreenSize(int widthf, int heightf)
        {
            this.width = widthf;
            this.height = heightf;
            int samples = 4;

            rectVAO = new VAO();
            rectVBO = new VBO(rectangleVertices.ToList());
            rectVAO.LinkAttrib(rectVBO, 0, 2, 4 * sizeof(float), 0);
            rectVAO.LinkAttrib(rectVBO, 1, 2, 4 * sizeof(float), 2 * sizeof(float));
            // Create Frame Buffer Object

            FBO = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FBO);

            // Create Framebuffer Texture

            framebufferTexture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2DMultisample, framebufferTexture);
            GL.TexImage2DMultisample(TextureTargetMultisample.Texture2DMultisample, samples, PixelInternalFormat.Rgba, width, height, true);
            GL.TexParameter(TextureTarget.Texture2DMultisample, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2DMultisample, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2DMultisample, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge); // Prevents edge bleeding
            GL.TexParameter(TextureTarget.Texture2DMultisample, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge); // Prevents edge bleeding
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2DMultisample, framebufferTexture, 0);

            // Create Render Buffer Object

            RBO = GL.GenRenderbuffer();
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, RBO);
            GL.RenderbufferStorageMultisample(RenderbufferTarget.Renderbuffer, samples, RenderbufferStorage.Depth24Stencil8, width, height);
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment, RenderbufferTarget.Renderbuffer, RBO);




            // Create Frame Buffer Object

            PostFBO = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, PostFBO);

            // Create Framebuffer Texture

            PostframebufferTexture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, PostframebufferTexture);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, PostframebufferTexture, 0);



        }
    }
}
