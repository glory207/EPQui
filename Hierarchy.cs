using System; using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Mathematics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.IO;
using System.Windows.Shapes;
using System.Diagnostics;
using Quaternion = OpenTK.Mathematics.Quaternion;
using System.Windows.Documents;

namespace EPQui
{
   public class Hierarchy: HierObj
    {

        public Mesh gridMesh;
        Shader gridshaderProgram;
        List<MeshContainer> meshContainers = new List<MeshContainer>();
        List<LightContainer> lightContainers = new List<LightContainer>();

        Shader LightClickProgram;
        Shader MeshClickProgram;
        Shader shadowABC;
        Shader LightShaderProgram;
        Shader MeshShaderProgram;
        public string path;
        
        public void save()
        {
            StreamWriter str = new StreamWriter(path);
            
            for (int i = 0; i < children.Count; i++)
            {
                if (children[i].GetType() == typeof(LightContainer))
                {
                    str.WriteLine("<LightContainer>");
                    str.WriteLine("angle:");
                    str.WriteLine(write2(((LightContainer)children[i]).angle));
                    str.WriteLine("intencity:");
                    str.WriteLine(((LightContainer)children[i]).intencity.ToString());
                    str.WriteLine("lightColor:");
                    str.WriteLine(write4(((LightContainer)children[i]).lightColor));
                    str.WriteLine("name:");
                    str.WriteLine(((LightContainer)children[i]).name.ToString());
                    str.WriteLine("objectRotation:");
                    str.WriteLine(write3(((LightContainer)children[i]).objectRotation.ToEulerAngles()));
                    str.WriteLine("objectScale:");
                    str.WriteLine(write3(((LightContainer)children[i]).objectScale));
                    str.WriteLine("Position:");
                    str.WriteLine(write3(((LightContainer)children[i]).Position));
                    str.WriteLine("Type:");
                    str.WriteLine(((int)((LightContainer)children[i]).Type).ToString());
                }else if (children[i].GetType() == typeof(MeshContainer))
                {
                    str.WriteLine("<MeshContainer>");
                    str.WriteLine("name:");
                    str.WriteLine(((MeshContainer)children[i]).name.ToString());
                    str.WriteLine("objectRotation:");
                    str.WriteLine(write3(((MeshContainer)children[i]).objectRotation.ToEulerAngles()));
                    str.WriteLine("objectScale:");
                    str.WriteLine(write3(((MeshContainer)children[i]).objectScale));
                    str.WriteLine("Position:");
                    str.WriteLine(write3(((MeshContainer)children[i]).Position));
                    str.WriteLine("mesh path:");
                    str.WriteLine(((MeshContainer)children[i]).mesh.path.ToString());
                    str.WriteLine("mate texOff:");
                    str.WriteLine(write2(((MeshContainer)children[i]).mate.texOff));
                    str.WriteLine("mate texScale:");
                    str.WriteLine(write2(((MeshContainer)children[i]).mate.texScale));
                    str.WriteLine("mate specular:");
                    str.WriteLine(((MeshContainer)children[i]).mate.specular.ToString());
                    str.WriteLine("mate diffuce:");
                    str.WriteLine(((MeshContainer)children[i]).mate.diffuce.ToString());
                    str.WriteLine("mate path:");
                    if (((MeshContainer)children[i]).mate.textures.Count == 0) str.WriteLine("emp");
                    else str.WriteLine(((MeshContainer)children[i]).mate.textures[0].path.ToString());
                }
            }
            str.Close();
        }
        public Hierarchy(string path)
        {
            LightClickProgram = new Shader("Res/shaders/Gyzmo.vert", "Res/shaders/Clicks.frag", "Res/shaders/light.geomertry");
            MeshClickProgram = new Shader("Res/shaders/default.vert", "Res/shaders/Clicks.frag", "Res/shaders/default.geometry");
            shadowABC = new Shader("Res/shaders/shadowMap.vert", "Res/shaders/shadowMap.frag");
            LightShaderProgram = new Shader("Res/shaders/Gyzmo.vert", "Res/shaders/light.frag", "Res/shaders/light.geomertry");
            MeshShaderProgram = new Shader("Res/shaders/default.vert", "Res/shaders/default.frag", "Res/shaders/default.geometry");
            this.path = path;
            StreamReader str = new StreamReader(path);
            
            string line;
            while (!str.EndOfStream)
            {
                line = str.ReadLine();
                if (line == "<LightContainer>")
                {
                    
                    LightContainer tempL = new LightContainer(this);
                    str.ReadLine();
                    line = str.ReadLine();
                    tempL.angle = parse2(line); 

                    str.ReadLine();
                    line = str.ReadLine();
                    tempL.intencity = float.Parse(line); 

                    str.ReadLine();
                    line = str.ReadLine();
                    tempL.lightColor = parse4(line); 

                    str.ReadLine();
                    line = str.ReadLine();
                    tempL.name = (line); 

                    str.ReadLine();
                    line = str.ReadLine();
                    tempL.objectRotation = Quaternion.FromEulerAngles(parse3(line)); 

                    str.ReadLine();
                    line = str.ReadLine();
                    tempL.objectScale = parse3(line); 

                    str.ReadLine();
                    line = str.ReadLine();
                    tempL.Position = parse3(line); 

                    str.ReadLine();
                    line = str.ReadLine();
                    tempL.Type =(LightType) int.Parse(line); 
                    children.Add(tempL);
                }
                else if(line == "<MeshContainer>")
                {
                    MeshContainer tempL = new MeshContainer() { parent = this};
                    str.ReadLine();
                    line = str.ReadLine();
                    tempL.name = (line);

                    str.ReadLine();
                    line = str.ReadLine();
                    tempL.objectRotation = Quaternion.FromEulerAngles(parse3(line));

                    str.ReadLine();
                    line = str.ReadLine();
                    tempL.objectScale = parse3(line);

                    str.ReadLine();
                    line = str.ReadLine();
                    tempL.Position = parse3(line);

                    str.ReadLine();
                    line = str.ReadLine();
                    tempL.mesh = new Mesh(line);

                    tempL.mate = new material();

                    str.ReadLine();
                    line = str.ReadLine();
                    tempL.mate.texOff = parse2(line);

                    str.ReadLine();
                    line = str.ReadLine();
                    tempL.mate.texScale = parse2(line);

                    str.ReadLine();
                    line = str.ReadLine();
                    tempL.mate.specular = float.Parse(line);

                    str.ReadLine();
                    line = str.ReadLine();
                    tempL.mate.diffuce = float.Parse(line);

                    str.ReadLine();
                    line = str.ReadLine();
                    if (line == "emp") tempL.mate.textures = new List<Texture>();
                    else tempL.mate.textures = new List<Texture>() { new Texture(line, "diffuse", PixelFormat.Rgba) };

                    children.Add(tempL);
                }
            }
            str.Close();

                gridMesh = new Mesh();
            gridshaderProgram = new Shader("Res/shaders/grid.vert", "Res/shaders/grid.frag", "Res/shaders/grid.geomertry");

        }
        Vector4 parse4(string line)
        {
            float x;
            float y;
            float z;
            float w;
            string temp = "";
            int start = 0;
            for (int i = start; i < line.Length; i++)
            {
                if (line[i] == ' ')
                {
                    start = i + 1; break;
                }
                else
                {
                    temp += line[i];
                }
            }
            x = float.Parse(temp);
            temp = "";
            for (int i = start; i < line.Length; i++)
            {
                if (line[i] == ' ')
                {
                    start = i + 1; break;
                }
                else
                {
                    temp += line[i];
                }
            }
            y = float.Parse(temp);
            temp = "";
            for (int i = start; i < line.Length; i++)
            {
                if (line[i] == ' ')
                {
                    break;
                }
                else
                {
                    temp += line[i];
                }
            }
            z = float.Parse(temp);
            temp = "";
            for (int i = start; i < line.Length; i++)
            {
                if (line[i] == ' ')
                {
                    break;
                }
                else
                {
                    temp += line[i];
                }
            }
            w = float.Parse(temp);
            temp = "";
            return (new Vector4(x, y, z, w));
        }
        
        Vector3 parse3(string line)
        {
            float x;
            float y;
            float z;
            string temp = "";
            int start = 0;
            for (int i = start; i < line.Length; i++)
            {
                if (line[i] == ' ')
                {
                    start = i + 1; break;
                }
                else
                {
                    temp += line[i];
                }
            }
            x = float.Parse(temp);
            temp = "";
            for (int i = start; i < line.Length; i++)
            {
                if (line[i] == ' ')
                {
                    start = i + 1; break;
                }
                else
                {
                    temp += line[i];
                }
            }
            y = float.Parse(temp);
            temp = "";
            for (int i = start; i < line.Length; i++)
            {
                if (line[i] == ' ')
                {
                    break;
                }
                else
                {
                    temp += line[i];
                }
            }
            z = float.Parse(temp);
            temp = "";
            return (new Vector3(x, y, z));
        }

        Vector2 parse2(string line)
        {
            float x;
            float y;
            string temp = "";
            int start = 0;
            for (int i = start; i < line.Length; i++)
            {
                if (line[i] == ' ')
                {
                    start = i + 1; break;
                }
                else
                {
                    temp += line[i];
                }
            }
            x = float.Parse(temp);
            temp = "";
            for (int i = start; i < line.Length; i++)
            {
                if (line[i] == ' ')
                {
                    start = i + 1; break;
                }
                else
                {
                    temp += line[i];
                }
            }
            y = float.Parse(temp);
            temp = "";
            return (new Vector2(x, y));
        }

        string write4(Vector4 vc)
        {
            return vc.X.ToString() + " " + vc.Y.ToString() + " " + vc.Z.ToString() + " " + vc.W.ToString();
        }
        
        string write3(Vector3 vc)
        {
            return vc.X.ToString() + " " + vc.Y.ToString() + " " + vc.Z.ToString();
        }

        string write2(Vector2 vc)
        {
            return vc.X.ToString() + " " + vc.Y.ToString();
        }
        public Hierarchy()
        {
            path = "Res/scenes/scene.sce";
            bool valid = false;
            do
            {
                if (File.Exists(path)) path = path.Substring(0, path.Length - 4) + "(dupe).sce";
                else valid = true;
            } while (valid == false);

            gridMesh = new Mesh();
            gridshaderProgram = new Shader("Res/shaders/grid.vert", "Res/shaders/grid.frag", "Res/shaders/grid.geomertry");
            LightClickProgram = new Shader("Res/shaders/Gyzmo.vert", "Res/shaders/Clicks.frag", "Res/shaders/light.geomertry");
            MeshClickProgram = new Shader("Res/shaders/default.vert", "Res/shaders/Clicks.frag", "Res/shaders/default.geometry");
            shadowABC = new Shader("Res/shaders/shadowMap.vert", "Res/shaders/shadowMap.frag");
            LightShaderProgram = new Shader("Res/shaders/Gyzmo.vert", "Res/shaders/light.frag", "Res/shaders/light.geomertry");
            MeshShaderProgram = new Shader("Res/shaders/default.vert", "Res/shaders/default.frag", "Res/shaders/default.geometry");
            children.Add(new LightContainer(this));
        }
        public override void PreUpdate()
        {
            foreach (HierObj ob in children)
            {
                ob.PreUpdate();
                if (ob.GetType() == typeof(LightContainer)) lightContainers.Add((LightContainer)ob);
                if (ob.GetType() == typeof(MeshContainer)) meshContainers.Add((MeshContainer)ob);
            }
        }
        public override void Update(List<LightContainer> lights, Camera camera)
        {
            LightShaderProgram.Activate();
            GL.Uniform3(GL.GetUniformLocation(LightShaderProgram.ID, "camUp"), camera.OrientationU);
            GL.Uniform3(GL.GetUniformLocation(LightShaderProgram.ID, "camRight"), camera.OrientationR);
            GL.Uniform3(GL.GetUniformLocation(LightShaderProgram.ID, "camPos"), camera.Position);
            camera.Matrix(LightShaderProgram, "camMatrix");
            foreach (HierObj ob in children)
            {
                if (ob.GetType() == typeof(LightContainer))
                {
                    GL.UniformMatrix4(GL.GetUniformLocation(LightShaderProgram.ID, "model"), false, ref ob.objectModel);
                    GL.Uniform4(GL.GetUniformLocation(LightShaderProgram.ID, "lightColor"), ob.lightColor);
                    ob.mesh.Draw();

                    lights.Add(((LightContainer)ob));
                }
            }
            MeshShaderProgram.Activate();

            GL.Uniform1(GL.GetUniformLocation(MeshShaderProgram.ID, "lightnum"), lights.Count);
            for (int i = 0; i < lights.Count; i++)
            {
                string ii = "lightType[" + i.ToString() + "]";
                GL.Uniform1(GL.GetUniformLocation(MeshShaderProgram.ID, ii), (int)lights[i].Type);

                ii = "lightColor[" + i.ToString() + "]";
                GL.Uniform4(GL.GetUniformLocation(MeshShaderProgram.ID, ii), lights[i].lightColor.Normalized());
                ii = "lightIntensity[" + i.ToString() + "]";
                GL.Uniform1(GL.GetUniformLocation(MeshShaderProgram.ID, ii), lights[i].intencity);
                ii = "lightPos[" + i.ToString() + "]";
                GL.Uniform3(GL.GetUniformLocation(MeshShaderProgram.ID, ii), lights[i].Position + lights[i].PositionAdded);
                ii = "lightAng[" + i.ToString() + "]";
                GL.Uniform2(GL.GetUniformLocation(MeshShaderProgram.ID, ii), lights[i].angle);

                ii = "lightRot[" + i.ToString() + "]";
                Matrix4 mt = lights[i].rotationMatrix.Inverted();
                GL.UniformMatrix4(GL.GetUniformLocation(MeshShaderProgram.ID, ii), false, ref mt);

                ii = "lightProjection[" + i.ToString() + "]";
                mt = lights[i].shadowModel;
                GL.UniformMatrix4(GL.GetUniformLocation(MeshShaderProgram.ID, ii), false, ref mt);

                ii = "ShadowMap[" + i.ToString() + "]";
                lights[i].FBO.BindT(MeshShaderProgram, ii);

                  ii = "ShadowMapC[" + i.ToString() + "]";
                
                  GL.ActiveTexture(TextureUnit.Texture0);
                  GL.BindTexture(TextureTarget.TextureCubeMap, lights[i].FBOC.framebufferTextureP);
                  GL.Uniform1(GL.GetUniformLocation(MeshShaderProgram.ID, ii), 2);
                  lights[i].FBOC.BindT(MeshShaderProgram, ii);
            }
            GL.Uniform3(GL.GetUniformLocation(MeshShaderProgram.ID, "camPos"), camera.Position);
            camera.Matrix(MeshShaderProgram, "camMatrix");
            foreach (HierObj ob in children)
            {
                if(ob.GetType() == typeof(MeshContainer)) ((MeshContainer)ob).undate(MeshShaderProgram);
            }


            gridshaderProgram.Activate();
            Matrix4 mat = Matrix4.Identity;
            GL.UniformMatrix4(GL.GetUniformLocation(gridshaderProgram.ID, "model"), false, ref mat);
            GL.Uniform3(GL.GetUniformLocation(gridshaderProgram.ID, "camUp"), camera.Orientation);
            gridshaderProgram.Activate();
            GL.Uniform3(GL.GetUniformLocation(gridshaderProgram.ID, "camPos"), camera.Position);
            camera.Matrix(gridshaderProgram, "camMatrix");
            gridMesh.Draw();


        }
        public override void UpdateClick(Camera camera,Shader shader)
        {
            LightClickProgram.Activate();
            GL.Uniform3(GL.GetUniformLocation(LightClickProgram.ID, "camUp"), camera.OrientationU);
            GL.Uniform3(GL.GetUniformLocation(LightClickProgram.ID, "camRight"), camera.OrientationR);
            GL.Uniform3(GL.GetUniformLocation(LightClickProgram.ID, "camPos"), camera.Position);
            camera.Matrix(LightClickProgram, "camMatrix");
            foreach (HierObj obb in children)
            {
                   
                if (obb.GetType() == typeof(LightContainer))
                {
                    LightContainer ob = (LightContainer)obb;



                    GL.Uniform1(GL.GetUniformLocation(LightClickProgram.ID, "objectId"), children.IndexOf(ob));
                    GL.UniformMatrix4(GL.GetUniformLocation(LightClickProgram.ID, "model"), false, ref ob.objectModel);
                    GL.Uniform4(GL.GetUniformLocation(LightClickProgram.ID, "lightColor"), ob.lightColor);
                    ob.mesh.Draw();
                }
            }
            MeshClickProgram.Activate();
            GL.Uniform3(GL.GetUniformLocation(MeshClickProgram.ID, "camUp"), camera.OrientationU);
            GL.Uniform3(GL.GetUniformLocation(MeshClickProgram.ID, "camRight"), camera.OrientationR);
            GL.Uniform3(GL.GetUniformLocation(MeshClickProgram.ID, "camPos"), camera.Position);
            camera.Matrix(MeshClickProgram, "camMatrix");

            foreach (HierObj obb in children)
            {
                   
                if (obb.GetType() == typeof(MeshContainer))
                {
                    MeshContainer ob = (MeshContainer)obb;


                    GL.Uniform1(GL.GetUniformLocation(MeshClickProgram.ID, "objectId"), children.IndexOf(ob));
                    GL.UniformMatrix4(GL.GetUniformLocation(MeshClickProgram.ID, "model"), false, ref ob.objectModel);
                    GL.Uniform4(GL.GetUniformLocation(MeshClickProgram.ID, "lightColor"), ob.lightColor);
                    ob.mesh.Draw();
                }
            }
        }
        public void UpdateShadow(Camera cam)
        {

            foreach (HierObj obb in children)
            {
                if (obb.GetType() == typeof(LightContainer))
                {
                    ((LightContainer)obb).setShadowModel(cam);
                    if (((LightContainer)obb).Type == LightType.point)
                    {
                     //   foreach (HierObj ob in children)
                     //   {
                     //       if (ob.GetType() == typeof(MeshContainer))
                     //       {
                     //
                     //           ((MeshContainer)ob).UpdateShadowC(((LightContainer)obb));
                     //       }
                     //   }
                    }
                    else
                    {
                        shadowABC.Activate();
                        GL.UniformMatrix4(GL.GetUniformLocation(shadowABC.ID, "lightProjection"), false, ref ((LightContainer)obb).shadowModel);
                        foreach (HierObj ob in children)
                        {
                            if (ob.GetType() == typeof(MeshContainer))
                            {

                                GL.UniformMatrix4(GL.GetUniformLocation(shadowABC.ID, "model"), false, ref ob.objectModel);
                                ((MeshContainer)ob).mesh.Draw();
                            }
                        }
                    }

                }
            }
            GL.Viewport(0, 0, cam.width, cam.height);

        }
        public override void destroy()
        {
            foreach (HierObj ob in children)
            {
                ob.destroy();
            }
            gridshaderProgram.Delete();
        }

    }
}
