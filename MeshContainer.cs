using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using static System.Net.Mime.MediaTypeNames;
using OpenTK.Graphics.OpenGL4;
using System.Windows.Media.Media3D;

namespace EPQui
{
    internal class MeshContainer: HierObj
    {
       public MeshContainer(Vector3 pos,string path) {
            Position = pos;
            objectScale = new Vector3(1.0f);
            objectRotation = new Vector3(0);

            shaderProgram = new Shader();
            clickProgram = new Shader("Res/default.vert", "Res/Clicks.frag", "Res/default.geometry");
            Texture[] textures = {
                 new Texture("Res/planks.png", "diffuse", 0, PixelFormat.Rgba),
                 new Texture("Res/planksSpec.png", "specular", 1, PixelFormat.Red)
            };
            List<Texture> tex = textures.ToList();

            mesh = new Mesh(path, tex);
        }
       public MeshContainer() {
            Position = new Vector3(0);
            objectRotation = new Vector3(0);
            objectScale = new Vector3(1);
            shaderProgram = new Shader();
            clickProgram = new Shader("Res/default.vert", "Res/Clicks.frag", "Res/default.geometry");
        } 
       public override void destroy() {
            shaderProgram.Delete();
            clickProgram.Delete();
        }
        public override void Update(List<LightContainer> lights,Camera camera) {
            

             
            objectModel =  Matrix4.CreateRotationZ(objectRotation.Z);
            objectModel = objectModel * Matrix4.CreateRotationY(objectRotation.Y);
            objectModel = objectModel * Matrix4.CreateRotationX(objectRotation.X);
            objectModel = objectModel * Matrix4.CreateScale(objectScale);
            objectModel = objectModel * Matrix4.CreateTranslation(Position);

            shaderProgram.Activate();
              
	          GL.UniformMatrix4(GL.GetUniformLocation(shaderProgram.ID, "model"), false, ref objectModel);
	          GL.Uniform1(GL.GetUniformLocation(shaderProgram.ID, "lightnum"), lights.Count);
	          for (int i = 0; i < lights.Count; i++)
	        {
	        	string ii = "lightColor[" + i.ToString() + "]";
	        	GL.Uniform4(GL.GetUniformLocation(shaderProgram.ID, ii), lights[i].lightColor);
	        	ii = "lightPos[" + i.ToString() + "]";
	        	GL.Uniform3(GL.GetUniformLocation(shaderProgram.ID, ii), lights[i].Position);
            
	        	ii = "lightRot[" + i.ToString() + "]";
	        	GL.Uniform3(GL.GetUniformLocation(shaderProgram.ID, ii), lights[i].objectRotation);
            
	        }
            mesh.Draw(shaderProgram, camera);




        }
       public override void UpdateClick(Camera camera,int value,int value2)
        {

            objectModel = Matrix4.CreateRotationZ(objectRotation.Z);
            objectModel = objectModel * Matrix4.CreateRotationY(objectRotation.Y);
            objectModel = objectModel * Matrix4.CreateRotationX(objectRotation.X);
            objectModel = objectModel * Matrix4.CreateScale(objectScale);
            objectModel = objectModel * Matrix4.CreateTranslation(Position);

            clickProgram.Activate();
            GL.Uniform1(GL.GetUniformLocation(clickProgram.ID, "objectId"), value);
            GL.Uniform1(GL.GetUniformLocation(clickProgram.ID, "objectLength"), value2);
            GL.UniformMatrix4(GL.GetUniformLocation(clickProgram.ID, "model"), false, ref objectModel);
            mesh.DrawToClick(clickProgram, camera);

        }

        public override void Update(Camera camera)
        {
            
        }
    }
}
