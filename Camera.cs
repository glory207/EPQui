using System; using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Mathematics;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Media.Media3D;

namespace EPQui
{
    public class Camera
    {
       public FrameBuffer frame;
       public FrameBuffer frameC;
       public FrameBuffer frameE;

        public Vector3 Position;
        public Vector3 Orientation = new Vector3(-1.0f, 0.0f, 0.0f);
        public Vector3 OrientationR = new Vector3(0.0f, 0.0f, -1.0f);
        public Vector3 OrientationU = new Vector3(0.0f, 1.0f, 0.0f);
        public Vector3 Up = new Vector3(0.0f, 1.0f, 0.0f);
        public Matrix4 cameraMatrix = new Matrix4(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
        public int width;
        public int height;
        public bool firstClick = true;
        public float speed = 0.1f;
        public float sensitivity = 100.0f;
        int DeFrame;
        public Camera(int width, int height, Vector3 position,int dframe)
        {
            DeFrame = dframe;

            frameC = new FrameBuffer(width, height, PixelInternalFormat.R32i, PixelFormat.RedInteger, PixelType.Int,TextureTarget.Texture2D, DeFrame,false) { color = new Color4(0, 0, 255, 255) };
            frameE = new FrameBuffer(width, height, PixelInternalFormat.R32i, PixelFormat.RedInteger, PixelType.Int, TextureTarget.Texture2D, DeFrame,false) { color = new Color4(0, 0, 255, 255) };
            frame = new FrameBuffer(width, height, PixelInternalFormat.Rgba, PixelFormat.Rgba, PixelType.UnsignedByte, TextureTarget.Texture2D, DeFrame,true);
            updateScreenSize(width, height);
            Position = position;

            
        }
        public Matrix4 view;
        public Matrix4 projection;
        public bool perspective = true;
        public void updateMatrix(float FOVdeg, float nearPlane, float farPlane, int wi, int he) {
            this.width = wi;
            this.height = he;
            // update view matrix
            view = Matrix4.LookAt(Position, Position + Orientation, Up);
            if (perspective) projection = Matrix4.CreatePerspectiveFieldOfView((FOVdeg * MathF.PI / 180f), (width / (float)height), nearPlane, farPlane);
            else projection = Matrix4.CreateOrthographic(width/10f, height/10f, -farPlane, farPlane );
            cameraMatrix = view *projection ;
            
        }
        public void Matrix(Shader shader, string uniform) {
            GL.UniformMatrix4(GL.GetUniformLocation(shader.ID, uniform),false, ref (cameraMatrix));

        }
        public void destroy() {
            frameC.destroy();
            frame.destroy();
            frameE.destroy();
        }
        public void updateScreenSize(int widthf, int heightf) {
            this.width = widthf;
            this.height = heightf;

            frame.updateScreenSize(widthf, heightf);
            frameC.updateScreenSize(widthf, heightf);
            frameE.updateScreenSize(widthf, heightf);
        }

    }
}
