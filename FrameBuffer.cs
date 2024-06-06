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
using static System.Net.Mime.MediaTypeNames;
using System.Windows;
using System.Reflection;

namespace EPQui
{
    public class FrameBuffer
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
        public int rectVAO;
        public int rectVBO;
     public   int DeFrame;
        public int FBO;
        public int framebufferTexture;
        public long framebufferTextureHandle;
        public int RBO;
        public int FBOP;
        public int framebufferTextureP;
        public long framebufferTexturePHandle;
        public int RBOP;
        public int width;
        public int height;
        public Color4 color = new Color4(0.1f, 0.2f, 0.3f, 4.0f);
        PixelInternalFormat pixelInternalFormat;
        PixelFormat pixelFormat;
        PixelType pixelType;
        TextureTarget textureTarget;
        bool multisample;
        public FrameBuffer(int widthf, int heightf, PixelInternalFormat pixelInternalFormat, PixelFormat pixelFormat, PixelType pixelType, TextureTarget textureTarget, int dfr, bool multi)
        {
            multisample = multi;
            DeFrame = dfr;
            this.pixelInternalFormat = pixelInternalFormat;
            this.pixelFormat = pixelFormat;
            this.pixelType = pixelType;
            this.textureTarget = textureTarget;

            rectVAO = GL.GenVertexArray();
            rectVBO = GL.GenBuffer();
            GL.BindVertexArray(rectVAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, rectVBO);
            GL.BufferData(BufferTarget.ArrayBuffer, (rectangleVertices.Length) * sizeof(float), rectangleVertices, BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), (2 * sizeof(float)));


            framebufferProgram = new Shader("Res/framebuffer.vert", "Res/framebuffer.frag");
            framebufferProgram.Activate();
            updateScreenSize(widthf, heightf);

        }
        public void Clear()
        {
            if (multisample) GL.BindFramebuffer(FramebufferTarget.Framebuffer, FBO);
            else GL.BindFramebuffer(FramebufferTarget.Framebuffer, FBOP);
            GL.ClearColor(color);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);
        }
        public void destroy()
        {
            if (multisample) GL.DeleteFramebuffer(FBO);
            GL.DeleteFramebuffer(FBOP);

        }
        public void update()
        {
            if (multisample)
            {
                GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, FBO);
                GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, FBOP);
                GL.BlitFramebuffer(0, 0, width, height, 0, 0, width, height, ClearBufferMask.ColorBufferBit, BlitFramebufferFilter.Nearest);
                GL.BindFramebuffer(FramebufferTarget.Framebuffer, FBO);
            }

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, DeFrame);

            framebufferProgram.Activate();
            GL.BindVertexArray(rectVAO);
            GL.Disable(EnableCap.DepthTest);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            //  GL.ActiveTexture(TextureUnit.Texture0);
            //  GL.BindTexture(textureTarget, framebufferTextureP);
            BindT(framebufferProgram, "screenTexture");
            GL.Uniform1(GL.GetUniformLocation(framebufferProgram.ID, "asd"), 0);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);


        }
        public void update2(Camera camera)
        {
            if (multisample)
            {
                GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, FBO);
                GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, FBOP);
                GL.BlitFramebuffer(0, 0, width, height, 0, 0, width, height, ClearBufferMask.ColorBufferBit, BlitFramebufferFilter.Nearest);
                GL.BindFramebuffer(FramebufferTarget.Framebuffer, FBO);
            }

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, DeFrame);

            framebufferProgram.Activate();
            GL.BindVertexArray(rectVAO);
            GL.Disable(EnableCap.DepthTest);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            //  GL.ActiveTexture(TextureUnit.Texture0);
            //  GL.BindTexture(textureTarget, framebufferTextureP);
            BindT(framebufferProgram, "screenTexture2");
            GL.Uniform1(GL.GetUniformLocation(framebufferProgram.ID, "asd"), 5);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);


        }
        public void BindT(Shader shd,string uniform)
        {
            shd.Activate();
            GL.Arb.UniformHandle(GL.GetUniformLocation(shd.ID, uniform), framebufferTexturePHandle);
        }
        public int update(int widthf, int heightf)
        {
            if (multisample) GL.BindFramebuffer(FramebufferTarget.Framebuffer, FBO);
            else GL.BindFramebuffer(FramebufferTarget.Framebuffer, FBOP);
            int[] pixel = new int[1];
            GL.ReadPixels(widthf, heightf, 1, 1, pixelFormat, pixelType, pixel);
            return pixel[0];
        }
        public void updateScreenSize(int widthf, int heightf)
        {

            this.width = widthf;
            this.height = heightf;
            if (multisample)
            {

                framebufferProgram.Activate();
                GL.Uniform2(GL.GetUniformLocation(framebufferProgram.ID, "ScreenSize"), new Vector2(height, width));

                int samples = 8;

                FBO = GL.GenFramebuffer();
                GL.BindFramebuffer(FramebufferTarget.Framebuffer, FBO);

                framebufferTexture = GL.GenTexture();
                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2DMultisample, framebufferTexture);
                GL.TexImage2DMultisample(TextureTargetMultisample.Texture2DMultisample, samples, pixelInternalFormat, width, height, true);

                GL.TexParameter(TextureTarget.Texture2DMultisample, TextureParameterName.TextureMinFilter, (float)TextureMinFilter.Nearest);
                GL.TexParameter(TextureTarget.Texture2DMultisample, TextureParameterName.TextureMagFilter, (float)TextureMagFilter.Nearest);
                GL.TexParameter(TextureTarget.Texture2DMultisample, TextureParameterName.TextureWrapS, (float)TextureWrapMode.ClampToEdge);
                GL.TexParameter(TextureTarget.Texture2DMultisample, TextureParameterName.TextureWrapT, (float)TextureWrapMode.ClampToEdge);
                GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2DMultisample, framebufferTexture, 0);
                framebufferTextureHandle = GL.Arb.GetTextureHandle(framebufferTexture);
                GL.Arb.MakeTextureHandleResident(framebufferTextureHandle);


                RBO = GL.GenRenderbuffer();
                GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, RBO);
                GL.RenderbufferStorageMultisample(RenderbufferTarget.Renderbuffer, samples, RenderbufferStorage.Depth24Stencil8, width, height);
                GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment, RenderbufferTarget.Renderbuffer, RBO);



                if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferErrorCode.FramebufferComplete)
                {

                    MessageBox.Show("Framebuffer is not complete!" + height.ToString());
                    throw new Exception("Framebuffer is not complete!");
                }
            }

            FBOP = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FBOP);

            framebufferTextureP = GL.GenTexture();
            GL.BindTexture(textureTarget, framebufferTextureP);
            if (textureTarget == TextureTarget.Texture2D) GL.TexImage2D(textureTarget, 0, pixelInternalFormat, width, height, 0, pixelFormat, pixelType, IntPtr.Zero);
            else if (textureTarget == TextureTarget.TextureCubeMap)
            {
                for (int i = 0; i < 1; ++i)
                {
                    GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0, pixelInternalFormat, width, height, 0, pixelFormat, pixelType, IntPtr.Zero);
                }

                GL.TexParameter(textureTarget, TextureParameterName.TextureWrapR, (float)TextureWrapMode.ClampToEdge);
            }
            GL.TexParameter(textureTarget, TextureParameterName.TextureMinFilter, (float)TextureMinFilter.Nearest);
            GL.TexParameter(textureTarget, TextureParameterName.TextureMagFilter, (float)TextureMagFilter.Nearest);
            GL.TexParameter(textureTarget, TextureParameterName.TextureWrapS, (float)TextureWrapMode.ClampToEdge);
            GL.TexParameter(textureTarget, TextureParameterName.TextureWrapT, (float)TextureWrapMode.ClampToEdge);
            framebufferTexturePHandle = GL.Arb.GetTextureHandle(framebufferTextureP);
            GL.Arb.MakeTextureHandleResident(framebufferTexturePHandle);
            

            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, textureTarget, framebufferTextureP, 0);
            RBOP = GL.GenRenderbuffer();
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, RBOP);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.Depth24Stencil8, width, height);
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment, RenderbufferTarget.Renderbuffer, RBOP);
            if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferErrorCode.FramebufferComplete)
            {

                MessageBox.Show("Framebuffer is not complete!" + height.ToString());
                throw new Exception("Framebuffer is not complete!");
            }
        }
    }
}
