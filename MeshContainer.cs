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
using System.Windows.Controls;
using System.Windows.Automation.Text;
using System.Windows.Media.Media3D;
using Quaternion = OpenTK.Mathematics.Quaternion;

namespace EPQui
{
    internal class MeshContainer : HierObj, ICloneable
    {
        public material mate;
        public MeshContainer(Vector3 pos, string path, HierObj parentt)
        {
            parent = parentt;
            Position = pos;
            objectScale = new Vector3(1.0f);
            objectRotation = Quaternion.Identity;
            objectRotationAdded = Quaternion.Identity;

            mate = new material();
            mesh = new Mesh(path);
            name = mesh.name;
        }
        public MeshContainer()
        {
            objectRotationAdded = Quaternion.Identity;

        }
        public object Clone()
        {
            return new MeshContainer()
            {
                parent = this.parent,
                Position = Position + PositionAdded,
                objectScale = objectScale + objectScaleAdded,
                objectRotation = objectRotation + objectRotationAdded,
                objectRotationAdded = Quaternion.Identity,

                mate = (material)mate.Clone(),
                mesh = mesh,
                name = name + " copy"
            };


        }
        public override void PreUpdate()
        {

            objectModel = Matrix4.CreateScale(objectScale + objectScaleAdded);
            rotationMatrix = Matrix4.CreateFromQuaternion(objectRotationAdded * objectRotation);
            objectModel = objectModel * rotationMatrix * Matrix4.CreateTranslation(Position + PositionAdded);

        }
        public override void Update(List<LightContainer> lights, Camera camera)
        {



        }
        public void undate(Shader shader)
        {



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
                mate.textures[i].texUnit(shader, (type + num).ToString());
            }

            GL.Uniform1(GL.GetUniformLocation(shader.ID, "diffuseLight"), mate.diffuce);
            GL.Uniform1(GL.GetUniformLocation(shader.ID, "specularLight"), mate.specular);
            GL.Uniform1(GL.GetUniformLocation(shader.ID, "noTex"), mate.textures.Count());

            GL.Uniform2(GL.GetUniformLocation(shader.ID, "textureSca"), mate.texScale);
            GL.Uniform2(GL.GetUniformLocation(shader.ID, "textureOff"), mate.texOff);
            GL.UniformMatrix4(GL.GetUniformLocation(shader.ID, "model"), false, ref objectModel);
            mesh.Draw();
        }
        public override void UpdateClick(Camera camera, Shader shader)
        {


          //  clickProgram.Activate();
          //  GL.Uniform1(GL.GetUniformLocation(clickProgram.ID, "objectId"), value);
          //  GL.UniformMatrix4(GL.GetUniformLocation(clickProgram.ID, "model"), false, ref objectModel);
          //  GL.Uniform3(GL.GetUniformLocation(clickProgram.ID, "camPos"), camera.Position);
          //  camera.Matrix(clickProgram, "camMatrix");
            mesh.Draw();

        }

        public override void destroy()
        {
           
        }
    }
}
