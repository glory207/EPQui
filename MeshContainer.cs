using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using static System.Net.Mime.MediaTypeNames;
using OpenTK.Graphics.OpenGL4;
using System.IO;
using System.Net;
using System.Windows.Shapes;

namespace EPQui
{
    internal class MeshContainer: HierObj,ICloneable
    {
        public material mate;

        public MeshContainer(Vector3 pos,string path,HierObj parentt) {
            parent = parentt;
            Position = pos;
            objectScale = new Vector3(1.0f);
            objectRotation = Quaternion.Identity;
            objectRotationAdded = Quaternion.Identity;

            shaderProgram = new Shader("Res/default.vert", "Res/default.frag", "Res/default.geometry");
            clickProgram = new Shader("Res/default.vert", "Res/Clicks.frag", "Res/default.geometry");

            mate = new material();
            mesh = new Mesh(path);
            name = mesh.name;
        }
        public MeshContainer()
        {
            shaderProgram = new Shader("Res/default.vert", "Res/default.frag", "Res/default.geometry");
            clickProgram = new Shader("Res/default.vert", "Res/Clicks.frag", "Res/default.geometry");
            objectRotationAdded = Quaternion.Identity;

        }
        public object Clone()
        {
            return new MeshContainer()
            {
                parent = this.parent,
                Position = Position,
            objectScale = objectScale,
            objectRotation = objectRotation,
            objectRotationAdded = objectRotationAdded,

            shaderProgram = new Shader("Res/default.vert", "Res/default.frag", "Res/default.geometry"),
            clickProgram = new Shader("Res/default.vert", "Res/Clicks.frag", "Res/default.geometry"),

            mate = (material)mate.Clone(),
            mesh = mesh,
            name = name + " copy"
            };
            
            
        }

        public override void destroy() {
            shaderProgram.Delete();
            clickProgram.Delete();
        }
        public override void Update(List<LightContainer> lights,Camera camera) {



            objectModel = Matrix4.CreateScale(objectScale + objectScaleAdded);
            rotationMatrix = Matrix4.CreateFromQuaternion(objectRotationAdded * objectRotation);
            objectModel = objectModel * rotationMatrix * Matrix4.CreateTranslation(Position + PositionAdded);

            shaderProgram.Activate();
              
	          GL.UniformMatrix4(GL.GetUniformLocation(shaderProgram.ID, "model"), false, ref objectModel);
	          GL.Uniform1(GL.GetUniformLocation(shaderProgram.ID, "lightnum"), lights.Count);
	          for (int i = 0; i < lights.Count; i++)
	        {
	        	string ii = "lightType[" + i.ToString() + "]";
	        	GL.Uniform1(GL.GetUniformLocation(shaderProgram.ID, ii), (int)lights[i].Type);

	            ii = "lightColor[" + i.ToString() + "]";
	        	GL.Uniform4(GL.GetUniformLocation(shaderProgram.ID, ii), lights[i].lightColor.Normalized());
	            ii = "lightIntensity[" + i.ToString() + "]";
	        	GL.Uniform1(GL.GetUniformLocation(shaderProgram.ID, ii), lights[i].intencity);
	        	ii = "lightPos[" + i.ToString() + "]";
	        	GL.Uniform3(GL.GetUniformLocation(shaderProgram.ID, ii), lights[i].Position + lights[i].PositionAdded);
	        	ii = "lightAng[" + i.ToString() + "]";
	        	GL.Uniform2(GL.GetUniformLocation(shaderProgram.ID, ii), lights[i].angle);
            
	        	ii = "lightRot[" + i.ToString() + "]";
                Matrix4 mt = lights[i].rotationMatrix.Inverted();
                GL.UniformMatrix4(GL.GetUniformLocation(shaderProgram.ID, ii), false ,ref mt);
            
	        }


           uint numDiffuse = 0;
           uint numSpecular = 0;
         for (int i = 0; i < mate.textures.Count(); i++)
         {
             string num = "";
             string type = mate.textures[i].type;
             if (type == "diffuse")
             {
                 num = (numDiffuse++).ToString();
             }
             else if (type == "specular")
             {
                 num = (numSpecular++).ToString();
             }
             mate.textures[i].texUnit(shaderProgram, (type + num).ToString(), (uint)i);
             mate.textures[i].Bind();
         }

         GL.Uniform1(GL.GetUniformLocation(shaderProgram.ID, "diffuseLight"), mate.diffuce);
         GL.Uniform1(GL.GetUniformLocation(shaderProgram.ID, "specularLight"), mate.specular);

         GL.Uniform2(GL.GetUniformLocation(shaderProgram.ID, "textureSca"), mate.texScale);
         GL.Uniform2(GL.GetUniformLocation(shaderProgram.ID, "textureOff"), mate.texOff);
         mesh.Draw(shaderProgram, camera);
            for (int i = 0; i < mate.textures.Count(); i++)
            {
           
                mate.textures[i].Unbind();
            }

        }
       public override void UpdateClick(Camera camera,int value,int value2)
        {


            objectModel = Matrix4.CreateScale(objectScale + objectScaleAdded);

            rotationMatrix = Matrix4.CreateFromQuaternion(objectRotationAdded * objectRotation);
            objectModel = objectModel * rotationMatrix * Matrix4.CreateTranslation(Position + PositionAdded);
            clickProgram.Activate();
            GL.Uniform1(GL.GetUniformLocation(clickProgram.ID, "objectId"), value);
            GL.Uniform1(GL.GetUniformLocation(clickProgram.ID, "objectLength"), value2);
            GL.UniformMatrix4(GL.GetUniformLocation(clickProgram.ID, "model"), false, ref objectModel);
            mesh.DrawToClick(clickProgram, camera);

        }
       public void UpdateShadow(Shader shadowMapProgram)
        {


            objectModel = Matrix4.CreateScale(objectScale + objectScaleAdded);
            rotationMatrix = Matrix4.CreateFromQuaternion(objectRotationAdded * objectRotation);
            objectModel = objectModel * rotationMatrix * Matrix4.CreateTranslation(Position + PositionAdded);
            GL.UniformMatrix4(GL.GetUniformLocation(shadowMapProgram.ID, "model"), false, ref objectModel);
            mesh.DrawToShadow();

        }

    }
}
