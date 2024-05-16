﻿using System; using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Mathematics;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Media.Media3D;

namespace EPQui
{
    internal class Camera
    {
       public FrameBuffer frameBuffer;
       public FrameBuffer ClickBuffer;

        public Vector3 Position;
        public Vector3 Orientation = new Vector3(1.0f, 0.0f, 0.0f);
        public Vector3 OrientationR = new Vector3(0.0f, 0.0f, 1.0f);
        public Vector3 OrientationU = new Vector3(0.0f, 1.0f, 0.0f);
        public Vector3 Up = new Vector3(0.0f, 1.0f, 0.0f);
        public Matrix4 cameraMatrix = new Matrix4(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
        public int width;
        public int height;
        public bool firstClick = true;
        public float speed = 0.1f;
        public float sensitivity = 100.0f;
        public Camera(int width, int height, Vector3 position)
        {
            ClickBuffer = new FrameBuffer(width, height) { color = new Color4(0,0,0,255)};
            
            frameBuffer = new FrameBuffer(width, height);
            updateScreenSize(width, height);
            Position = position;

            
        }
        public void updateMatrix(float FOVdeg, float nearPlane, float farPlane, int wi, int he) {
            Matrix4 view = new Matrix4(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
            Matrix4 projection = new Matrix4(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
            this.width = wi;
            this.height = he;
            // update view matrix
            view = Matrix4.LookAt(Position, Position + Orientation, Up);
            projection = Matrix4.CreatePerspectiveFieldOfView((FOVdeg * MathF.PI/180f), (width / (float)height), nearPlane, farPlane);
            cameraMatrix = view *projection ;
            
           // ClickBuffer.Bind();
            frameBuffer.Bind();
        }
        public void Matrix(Shader shader, string uniform) {
            GL.UniformMatrix4(GL.GetUniformLocation(shader.ID, uniform),false, ref (cameraMatrix));

        }
        // public void inputs(GLFWwindow* window);
        public void destroy() {
            ClickBuffer.destroy();
            frameBuffer.destroy();
        }
       public void update()
        {

            frameBuffer.update();
            ClickBuffer.Bind();
        }
       public int updateClicks(int x, int y)
        {
            
           return ClickBuffer.update(x,y);
        }
        public void updateScreenSize(int widthf, int heightf) {
            this.width = widthf;
            this.height = heightf;

            frameBuffer.updateScreenSize(widthf, heightf);
            ClickBuffer.updateScreenSize(widthf, heightf);
        }

    }
}