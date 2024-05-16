using System; using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using System.IO;
using System.Runtime.Intrinsics;
using System.Xml.Linq;
using System.Runtime.InteropServices;
using OpenTK.Windowing.Desktop;

namespace EPQui
{
    struct Vertex
    {
        public Vertex(Vector3 a, Vector2 b)
        {
            position = a;
            texUV = b;
        }
        public Vector3 position;
        public Vector2 texUV;
    }
    internal class Mesh
    {
       


        public string name = "empty";
        public List<Vertex> vertices = new List<Vertex>();
        public List<uint> indices = new List<uint>();
        public List<Texture> textures = new List<Texture>();
        VAO VAO = new VAO();

        public Mesh(List<Vertex> vertices, List<uint> indices, List<Texture> textures, string name) {
            this.name = name;
            this.vertices = vertices;
            this.indices = indices;
            this.textures = textures;

            VAO.Bind();
            VBO VBO = new VBO(vertices);
            EBO EBO = new EBO(indices);
            VAO.LinkAttrib(VBO, 0, 3,5 * sizeof(float), 0);
            VAO.LinkAttrib(VBO, 1, 2, 5 * sizeof(float), 3);

            VAO.Unbind();
            VBO.Unind();
            EBO.Unind();

        }
        public Mesh(string path, List<Texture> textures) {
            this.textures = textures;
            string line = "#";
            StreamReader str = new StreamReader(path);
            int num = 0;
            List<Vector3> verts = new List<Vector3>();
            List<Vector2> verTex = new List<Vector2>();
            List<Vertex> vertex = new List<Vertex>();
            List<uint> indices = new List<uint>();
            Vector3 offset = new Vector3(0);

            while (!str.EndOfStream)
            {
                line = str.ReadLine();
                if (line.StartsWith("o "))
                {


                    name = line.Substring(2);


                    num++;

                }
                else if (line.StartsWith("v "))
                {

                    float x;
                    float y;
                    float z;
                    string temp = "";
                    int start = 2;
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
                    verts.Add(new Vector3(x, y, z));
                }
                else if (line.StartsWith("vt "))
                {

                    float x;
                    float y;
                    string temp = "";
                    int start = 3;
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
                    verTex.Add(new Vector2(x, y));
                }
                else if (line.StartsWith("f "))
                {

                    int x;
                    int y;
                    int z;
                    string temp = "";
                    int start = 2;
                    for (int i = start; i < line.Length; i++)
                    {
                        if ((line[i]) == '/')
                        {
                            start = i + 1; break;
                        }
                        else
                        {
                            temp += line[i];
                        }
                    }
                    x = int.Parse(temp);
                    temp = "";
                    for (int i = start; i < line.Length; i++)
                    {
                        if ((line[i]) == '/')
                        {
                            start = i + 1; break;
                        }
                        else
                        {
                            temp += line[i];
                        }
                    }
                    y = int.Parse(temp);
                    temp = "";

                    for (int i = start; i < line.Length; i++)
                    {
                        if ((line[i]) == ' ')
                        {
                            start = i + 1; break;
                        }
                        else
                        {
                            temp += line[i];
                        }
                    }
                    z = int.Parse(temp);
                    temp = "";

                    vertex.Add(new Vertex( verts[(int)(x - 1 - offset.X)],verTex[(int)(y - 1 - offset.Y)] ));
                    indices.Add((uint)vertex.Count() - 1);

                    for (int i = start; i < line.Length; i++)
                    {
                        if ((line[i]) == '/')
                        {
                            start = i + 1; break;
                        }
                        else
                        {
                            temp += line[i];
                        }
                    }
                    x = int.Parse(temp);
                    temp = "";
                    for (int i = start; i < line.Length; i++)
                    {
                        if ((line[i]) == '/')
                        {
                            start = i + 1; break;
                        }
                        else
                        {
                            temp += line[i];
                        }
                    }
                    y = int.Parse(temp);
                    temp = "";

                    for (int i = start; i < line.Length; i++)
                    {
                        if ((line[i]) == ' ')
                        {
                            start = i + 1; break;
                        }
                        else
                        {
                            temp += line[i];
                        }
                    }
                    z = int.Parse(temp);
                    temp = "";

                    vertex.Add(new Vertex( verts[(int)(x - 1 - offset.X)],verTex[(int)(y - 1 - offset.Y)] ));
                    indices.Add((uint)vertex.Count() - 1);

                    for (int i = start; i < line.Length; i++)
                    {
                        if ((line[i]) == '/')
                        {
                            start = i + 1; break;
                        }
                        else
                        {
                            temp += line[i];
                        }
                    }
                    x = int.Parse(temp);
                    temp = "";
                    for (int i = start; i < line.Length; i++)
                    {
                        if ((line[i]) == '/')
                        {
                            start = i + 1; break;
                        }
                        else
                        {
                            temp += line[i];
                        }
                    }
                    y = int.Parse(temp);
                    temp = "";

                    for (int i = start; i < line.Length; i++)
                    {
                        if ((line[i]) == ' ')
                        {
                            start = i + 1; break;
                        }
                        else
                        {
                            temp += line[i];
                        }
                    }
                    z = int.Parse(temp);
                    temp = "";
                    vertex.Add(new Vertex( verts[(int)(x - 1 - offset.X)],verTex[(int)(y - 1 - offset.Y)] ));
                    indices.Add((uint)vertex.Count() - 1);
                }

            }

            str.Close();
            this.indices = indices;
            this.vertices = vertex;
            //bind stuff
            VAO.Bind();
            VBO VBO = new VBO(vertices);
            EBO EBO = new EBO(indices);
            VAO.LinkAttrib(VBO, 0, 3, 5 * sizeof(float), 0);
            VAO.LinkAttrib(VBO, 1, 2, 5 * sizeof(float), 3);

            VAO.Unbind();
            VBO.Unind();
            EBO.Unind();

        }
        public Mesh() {
            name = "empty";
            vertices = new List<Vertex>() { new Vertex(new Vector3(0), new Vector2(0)) ,new Vertex(new Vector3(0), new Vector2(0)) ,new Vertex(new Vector3(0), new Vector2(0)) };
            indices = new List<uint>() { 0,1,2};

            VAO.Bind();
            VBO VBO = new VBO(vertices);
            EBO EBO = new EBO(indices);
            VAO.LinkAttrib(VBO, 0, 3, 5 * sizeof(float), 0);
            VAO.LinkAttrib(VBO, 1, 2, 5 * sizeof(float), 3);

            VAO.Unbind();
            VBO.Unind();
            EBO.Unind();
        }
        public void Draw(Shader shader, Camera camera) {
             
            shader.Activate();
            VAO.Bind();
            uint numDiffuse = 0;
            uint numSpecular = 0;
         
         
            for (int i = 0; i < textures.Count(); i++)
            {
                string num = "";
                string type = textures[i].type;
                if (type == "diffuse")
                {
                    num = (numDiffuse++).ToString();
                }
                else if (type == "specular")
                {
                    num = (numSpecular++).ToString();
                }
                textures[i].texUnit(shader, (type + num).ToString(),(uint) i);
                textures[i].Bind();
            }
            GL.Uniform3(GL.GetUniformLocation(shader.ID, "camPos"), camera.Position);
            camera.Matrix(shader, "camMatrix");
            GL.DrawElements(BeginMode.Triangles, indices.Count, DrawElementsType.UnsignedInt, 0);
            for (int i = 0; i < textures.Count(); i++)
            {
         
                textures[i].Unbind();
            }
        }
        public void DrawToClick(Shader shader, Camera camera) {
            
            shader.Activate();
            VAO.Bind();

            
            camera.Matrix(shader, "camMatrix");
            GL.DrawElements(BeginMode.Triangles, indices.Count, DrawElementsType.UnsignedInt, 0);
        }



    }
}
