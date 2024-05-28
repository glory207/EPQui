using System; using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Mathematics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Reflection;

namespace EPQui
{

   public enum LightType
    {
        point,
        dir,
        spot
    }

    public class LightContainer : HierObj
    {
        public LightType Type = LightType.spot;
        public Matrix4 rotationMatrix = Matrix4.Identity;
        public Vector2 angle = new Vector2(0.9f, 0.1f);
        public float intencity = 1f;
        public LightContainer(Vector3 pos, Vector4 color, HierObj parent)
        {
            this.parent = parent; 
            Position = pos;
            lightColor = color;
            objectScale = new Vector3(1);
            shaderProgram = new Shader("Res/light.vert", "Res/light.frag", "Res/light.geomertry");
            clickProgram = new Shader("Res/light.vert", "Res/Clicks.frag", "Res/light.geomertry");
            mesh = new Mesh();


        }
        public override void Update(List<LightContainer> lights, Camera camera) {

            

            shaderProgram.Activate();

            rotationMatrix = Matrix4.CreateRotationZ(objectRotation.Z);
            rotationMatrix = rotationMatrix * Matrix4.CreateRotationY(objectRotation.Y);
            rotationMatrix = rotationMatrix * Matrix4.CreateRotationX(objectRotation.X);
            objectModel = rotationMatrix * Matrix4.CreateTranslation(Position);
            GL.UniformMatrix4(GL.GetUniformLocation(shaderProgram.ID, "model"),false, ref objectModel);
            GL.Uniform4(GL.GetUniformLocation(shaderProgram.ID, "lightColor"), lightColor);
            GL.Uniform3(GL.GetUniformLocation(shaderProgram.ID, "camUp"), camera.OrientationU);
            GL.Uniform3(GL.GetUniformLocation(shaderProgram.ID, "camRight"), camera.OrientationR);
            mesh.Draw(shaderProgram, camera);


        }
       public override void UpdateClick(Camera camera, int value, int value2)
        {

            clickProgram.Activate();

            rotationMatrix = Matrix4.CreateRotationZ(objectRotation.Z);
            rotationMatrix = rotationMatrix * Matrix4.CreateRotationY(objectRotation.Y);
            rotationMatrix = rotationMatrix * Matrix4.CreateRotationX(objectRotation.X);
            objectModel =  rotationMatrix * Matrix4.CreateTranslation(Position);
            GL.Uniform1(GL.GetUniformLocation(clickProgram.ID, "objectId"), value);
            GL.Uniform1(GL.GetUniformLocation(clickProgram.ID, "objectLength"), value2);
            GL.UniformMatrix4(GL.GetUniformLocation(clickProgram.ID, "model"), false, ref objectModel);
            GL.Uniform4(GL.GetUniformLocation(clickProgram.ID, "lightColor"), lightColor);
            GL.Uniform3(GL.GetUniformLocation(clickProgram.ID, "camUp"), camera.OrientationU);
            GL.Uniform3(GL.GetUniformLocation(clickProgram.ID, "camRight"), camera.OrientationR);
            mesh.DrawToClick(clickProgram, camera);
        }
        public override void destroy() {
            shaderProgram.Delete();
        }

    }

}
