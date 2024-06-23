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
      public  FrameBuffer FBOC;

      public  Matrix4 shadowModel;
      public  Matrix4[] shadowCube = new Matrix4[6];
        public LightContainer(HierObj parent)
        {
            this.parent = parent;
            objectScale = new Vector3(1);
            lightColor = Vector4.One;
            objectRotation = Quaternion.Identity;
            objectRotationAdded = Quaternion.Identity;
            mesh = new Mesh();
            name = "light";
            FBOC = new FrameBuffer(2048, 2048, PixelInternalFormat.R32f, PixelFormat.Red, PixelType.Float, TextureTarget.TextureCubeMap, 0, false) { color = new Color4(0, 0, 0, 0) };
            FBO = new FrameBuffer(2048, 2048, PixelInternalFormat.R32f, PixelFormat.Red, PixelType.Float, TextureTarget.Texture2D, 0, false) { color = new Color4(0, 0, 0, 0) };

        }

        public void setShadowModel(Camera camera)
        {
            GL.Viewport(0, 0, 2048, 2048);
            Vector4 di = new Vector4(0, -1, 0, 0) * rotationMatrix;
            Matrix4 view;
            // Matrix4 view = objectModel;
            Matrix4 projection;
            if (Type == LightType.dir)
            {
                projection = Matrix4.CreateOrthographic(50, 50, -25, 25);
                if (di == new Vector4((0, -1, 0, 0))) di = new Vector4(0.001f, -1, 0.001f, 0);
                view = Matrix4.LookAt(Vector3.Zero, di.Xyz * 20, new Vector3(0, 1, 0));

                shadowModel = view * projection;
                FBO.Clear();
            }
            else if (Type == LightType.spot)
            {
                projection = Matrix4.CreatePerspectiveFieldOfView(angle.X * 2f, (2048 / (float)2048), 0.01f, 50f);
                if (di == new Vector4((0, -1, 0, 0))) di = new Vector4(0.001f, -1, 0.001f, 0);
                view = Matrix4.LookAt((Position + PositionAdded), (Position + PositionAdded) + di.Xyz * 20, new Vector3(0, 1, 0));

                shadowModel = view * projection;
                FBO.Clear();
            }
            else if (Type == LightType.point)
            {

                projection = Matrix4.CreatePerspectiveFieldOfView(MathF.PI / 2f, (2048 / (float)2048), 0.01f, 50f);
                shadowCube = new Matrix4[6]{
                      Matrix4.LookAt(Position + PositionAdded, (Position + PositionAdded) + new Vector3(1.0f, 0.0f, 0.0f), new Vector3(0, -1.0f, 0.0f))  * projection   ,
                      Matrix4.LookAt(Position + PositionAdded, (Position + PositionAdded) + new Vector3(-1.0f, 0.0f, 0.0f), new Vector3(0, -1.0f, 0.0f)) * projection      ,
                      Matrix4.LookAt(Position + PositionAdded, (Position + PositionAdded) + new Vector3(0.0f, 1.0f, 0.0f), new Vector3(0, 0.0f,  1.0f )) * projection      ,
                      Matrix4.LookAt(Position + PositionAdded, (Position + PositionAdded) + new Vector3(0.0f, -1.0f, 0.0f), new Vector3(0, 0.0f, -1.0f)) * projection    ,
                      Matrix4.LookAt(Position + PositionAdded, (Position + PositionAdded) + new Vector3(0.0f, 0.0f, 1.0f), new Vector3(0, -1.0f, 0.0f))  * projection        ,
                      Matrix4.LookAt(Position + PositionAdded, (Position + PositionAdded) + new Vector3(0.0f, 0.0f, -1.0f), new Vector3(0, -1.0f, 0.0f)) * projection          ,
                };

                FBOC.Clear();
            }

        }
        public void ShadowModel(int fb)
        {
            FBO.DeFrame = fb;
            FBO.update();
        }
        public override void PreUpdate()
        {
            rotationMatrix = Matrix4.CreateFromQuaternion(objectRotationAdded * objectRotation);
            objectModel = rotationMatrix * Matrix4.CreateTranslation(Position + PositionAdded);
        }

        public override void Update(List<LightContainer> lights, Camera camera) {

            


        }
       public override void UpdateClick(Camera camera, Shader shader)
        {

            mesh.Draw();
        }
        public override void destroy() {

            mesh.delete();
            mesh = null;


        }
        public LightContainer()
        {
            FBOC = new FrameBuffer(2048, 2048, PixelInternalFormat.R32f, PixelFormat.Red, PixelType.Float, TextureTarget.TextureCubeMap, 0, false) { color = new Color4(0, 0, 0, 0) };
            FBO = new FrameBuffer(2048, 2048, PixelInternalFormat.R32f, PixelFormat.Red, PixelType.Float, TextureTarget.Texture2D, 0, false) { color = new Color4(0, 0, 0, 0) };


        }
        public object Clone()
        {
            return new LightContainer()
            {
                parent = parent,
                Position = Position + PositionAdded,
                objectScale = objectScale + objectScaleAdded,
                objectRotation = objectRotation + objectRotationAdded,
                objectRotationAdded = Quaternion.Identity,
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
