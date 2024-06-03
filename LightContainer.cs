using System; using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Mathematics;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Windows.Media.Media3D;
using Quaternion = OpenTK.Mathematics.Quaternion;
using System.Windows.Controls;

namespace EPQui
{

   public enum LightType
    {
        point,
        dir,
        spot
    }

    public class LightContainer : HierObj, ICloneable
    {
        public LightType Type = LightType.point;
        public Vector2 angle = new Vector2(0.9f, 0.1f);
        public float intencity = 1f;

        //  public int FBO;
        //  public int framebufferTexture;
        //  int width = 2048;
      public  FrameBuffer FBO;

      public  Matrix4 shadowModel;
        public LightContainer(HierObj parent)
        {
            this.parent = parent;
            objectScale = new Vector3(1);
            lightColor = Vector4.One;
            objectRotation = Quaternion.Identity;
            objectRotationAdded = Quaternion.Identity;
            shaderProgram = new Shader("Res/Gyzmo.vert", "Res/light.frag", "Res/light.geomertry");
            clickProgram = new Shader("Res/Gyzmo.vert", "Res/Clicks.frag", "Res/light.geomertry");
            mesh = new Mesh();
            name = "light";
            FBO = new FrameBuffer(2048,2048,PixelInternalFormat.Rgba,PixelFormat.Rgba,PixelType.UnsignedByte,0,false);
            
         //  FBO = GL.GenFramebuffer();
         //  framebufferTexture = GL.GenTexture();
         //  GL.BindTexture(TextureTarget.Texture2D, framebufferTexture);
         //  GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.DepthComponent, width, width, 0, PixelFormat.DepthComponent, PixelType.UnsignedByte, IntPtr.Zero);
         //
         //  GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (float)TextureMinFilter.Nearest);
         //  GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (float)TextureMagFilter.Nearest);
         //  GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (float)TextureWrapMode.ClampToEdge);
         //  GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (float)TextureWrapMode.ClampToEdge);
         //  float[] clampColor = {1,1,1,1};
         //  GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBorderColor, clampColor);
         //  GL.BindFramebuffer(FramebufferTarget.Framebuffer, FBO);
         //  GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, TextureTarget.Texture2D, framebufferTexture, 0);
         //  GL.DrawBuffer(DrawBufferMode.None);
         //  GL.ReadBuffer(ReadBufferMode.None);
         //  GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

            
        }

        public void setShadowModel(Camera camera)
        {
            GL.Viewport(0,0, 2048/2, 2048/2);
            Vector4 di = new Vector4(0, -1, 0,0) * rotationMatrix;
            Matrix4 view = Matrix4.LookAt((Position + PositionAdded), (Position + PositionAdded) + di.Xyz, new Vector3(0, 1, 0));
            Matrix4 projection = Matrix4.CreateOrthographic(-100, 100, -100, 100);
           // Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView((45f * MathF.PI / 180f), (2048 / (float)2048), 0.1f, 100f);
          
            shadowModel = view * projection;
            FBO.Clear();
        }
        public void ShadowModel(int fb)
        {
            FBO.DeFrame = fb;
            FBO.update();
        }

        public override void Update(List<LightContainer> lights, Camera camera) {

            

            shaderProgram.Activate();

            rotationMatrix = Matrix4.CreateFromQuaternion(objectRotationAdded * objectRotation);
            objectModel = rotationMatrix * Matrix4.CreateTranslation(Position + PositionAdded);
            GL.UniformMatrix4(GL.GetUniformLocation(shaderProgram.ID, "model"),false, ref objectModel);
            GL.Uniform4(GL.GetUniformLocation(shaderProgram.ID, "lightColor"), lightColor);
            GL.Uniform3(GL.GetUniformLocation(shaderProgram.ID, "camUp"), camera.OrientationU);
            GL.Uniform3(GL.GetUniformLocation(shaderProgram.ID, "camRight"), camera.OrientationR);
            shaderProgram.Activate();
            GL.Uniform3(GL.GetUniformLocation(shaderProgram.ID, "camPos"), camera.Position);
            camera.Matrix(shaderProgram, "camMatrix");
            mesh.Draw();


        }
       public override void UpdateClick(Camera camera, int value, int value2)
        {

            clickProgram.Activate();

            rotationMatrix = Matrix4.CreateFromQuaternion(objectRotationAdded * objectRotation);
            objectModel =  rotationMatrix * Matrix4.CreateTranslation(Position + PositionAdded);
            GL.Uniform1(GL.GetUniformLocation(clickProgram.ID, "objectId"), value);
            GL.Uniform1(GL.GetUniformLocation(clickProgram.ID, "objectLength"), value2);
            GL.UniformMatrix4(GL.GetUniformLocation(clickProgram.ID, "model"), false, ref objectModel);
            GL.Uniform4(GL.GetUniformLocation(clickProgram.ID, "lightColor"), lightColor);
            GL.Uniform3(GL.GetUniformLocation(clickProgram.ID, "camUp"), camera.OrientationU);
            GL.Uniform3(GL.GetUniformLocation(clickProgram.ID, "camRight"), camera.OrientationR);
            GL.Uniform3(GL.GetUniformLocation(clickProgram.ID, "camPos"), camera.Position);
            camera.Matrix(clickProgram, "camMatrix");
            mesh.Draw();
        }
        public override void destroy() {
            shaderProgram.Delete();
        }
        public LightContainer()
        {
            FBO = new FrameBuffer(2048, 2048, PixelInternalFormat.Rgb, PixelFormat.Rgb, PixelType.UnsignedByte, 0, false);

            //   FBO = GL.GenFramebuffer();
            //   framebufferTexture = GL.GenTexture();
            //   GL.BindTexture(TextureTarget.Texture2D, framebufferTexture);
            //   GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.DepthComponent, width, width, 0, PixelFormat.DepthComponent, PixelType.UnsignedByte, IntPtr.Zero);
            //
            //   GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (float)TextureMinFilter.Nearest);
            //   GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (float)TextureMagFilter.Nearest);
            //   GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (float)TextureWrapMode.ClampToEdge);
            //   GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (float)TextureWrapMode.ClampToEdge);
            //   float[] clampColor = { 1, 1, 1, 1 };
            //   GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBorderColor, clampColor);
            //   GL.BindFramebuffer(FramebufferTarget.Framebuffer, FBO);
            //   GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, TextureTarget.Texture2D, framebufferTexture, 0);
            //   GL.DrawBuffer(DrawBufferMode.None);
            //   GL.ReadBuffer(ReadBufferMode.None);
            //   GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

        }
        public object Clone()
        {
            return new LightContainer()
            {
                parent = parent,
                Position = Position,
                objectScale = objectScale,
                objectRotation = objectRotation,
                objectRotationAdded = Quaternion.Identity,
                shaderProgram = new Shader("Res/Gyzmo.vert", "Res/light.frag", "Res/light.geomertry"),
                clickProgram = new Shader("Res/Gyzmo.vert", "Res/Clicks.frag", "Res/light.geomertry"),
                mesh = new Mesh(),
                name = "light",
                lightColor = lightColor,
                Type = Type,
                angle = angle,
                intencity = intencity,

            };


        }
    }

}
