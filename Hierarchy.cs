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

namespace EPQui
{
   public class Hierarchy: HierObj
    {

        public Mesh gridMesh;
        Shader gridshaderProgram;
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
                    str.WriteLine(((MeshContainer)children[i]).mate.textures[0].path.ToString());
                }
            }
            str.Close();
        }
        public Hierarchy(string path)
        {
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
                    tempL.mate.textures[0] = new Texture(line, "diffuse", 0, PixelFormat.Rgba);

                    children.Add(tempL);
                }
            }
            str.Close();

                gridMesh = new Mesh();
            gridshaderProgram = new Shader("Res/grid.vert", "Res/grid.frag", "Res/grid.geomertry");

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
            gridshaderProgram = new Shader("Res/grid.vert", "Res/grid.frag", "Res/grid.geomertry");
            children.Add(new LightContainer(this));
        }
        public override void Update(List<LightContainer> lights, Camera camera)
        {
            foreach (HierObj ob in children)
            {
                if (ob.GetType() == typeof(LightContainer))
                {
                    ob.Update(lights, camera);
                    lights.Add(((LightContainer)ob));
                }
            }

            foreach (HierObj ob in children)
            {
                if(ob.GetType() == typeof(MeshContainer)) ob.Update(lights, camera);
            }
            gridshaderProgram.Activate();
            Matrix4 mat = Matrix4.Identity;
            GL.UniformMatrix4(GL.GetUniformLocation(gridshaderProgram.ID, "model"), false, ref mat);
            GL.Uniform3(GL.GetUniformLocation(gridshaderProgram.ID, "camUp"), camera.Orientation);
            gridMesh.Draw(gridshaderProgram, camera);


        }
        public override void UpdateClick(Camera camera, int value, int value2)
        {
            foreach (HierObj ob in children)
            {
                ob.UpdateClick(camera, children.IndexOf(ob), children.Count);
            }
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
