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
    public struct Vertex
    {
       
        
        public Vertex(Vector3 a, Vector2 b)
        {
            position = a;
            texUV = b;
        }
        public Vector3 position;
        public Vector2 texUV;

    }
    public class Mesh : ICloneable
    {



        public string name = "empty";
        public string path = "empty";
        // public List<Vertex> vertices;
        // public List<uint> indices;
        VAO VAO = new VAO();
        EBO EBO;
        public void delete()
        {
            // vertices = new List<Vertex>();
            // indices = new List<uint>();
           VAO.Delete();
        }
        public Mesh(Vertex[] vertices, uint[] indices, string name) {
            this.name = name;
            //  this.vertices = vertices.ToList();
            //  this.indices = indices.ToList();
            lengthI = indices.Length;
            lengthA = vertices.Length;
            VAO.Bind();
            VBO VBO = new VBO(vertices);
            EBO = new EBO(indices);
            VAO.LinkAttrib(VBO, 0, 3, 5 * sizeof(float), 0);
            VAO.LinkAttrib(VBO, 1, 2, 5 * sizeof(float), 3);

            VAO.Unbind();
            VBO.Unind();
            EBO.Unind();
            EBO.Delete();

        }
        public Mesh(string path) {
            this.path = path;
            string line = "#";
            StreamReader str = new StreamReader(path);
            int num = 0;
            List<Vector3> verts = new List<Vector3>();
            List<Vector2> verTex = new List<Vector2>();
            List<Vertex> vertices = new List<Vertex>();
            List<uint> indices = new List<uint>();
            Vector3 offset = new Vector3(0);
            name = path.Substring(11, path.Substring(11).Length - 4);
            while (!str.EndOfStream)
            {
                line = str.ReadLine();
                if (line.StartsWith("o "))
                {


                    // name = line.Substring(2);


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
                    if (temp == "") y = 0;
                    else y = int.Parse(temp);
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

                    vertices.Add(new Vertex(verts[(int)(x - 1 - offset.X)], verTex[(int)(y - 1 - offset.Y)]));
                    indices.Add((uint)vertices.Count() - 1);

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

                    vertices.Add(new Vertex(verts[(int)(x - 1 - offset.X)], verTex[(int)(y - 1 - offset.Y)]));
                    indices.Add((uint)vertices.Count() - 1);

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
                    vertices.Add(new Vertex(verts[(int)(x - 1 - offset.X)], verTex[(int)(y - 1 - offset.Y)]));
                    indices.Add((uint)vertices.Count() - 1);
                }

            }

            str.Close();
            lengthI = indices.Count;
            lengthA = vertices.Count;
            verts = new List<Vector3>();
            verTex = new List<Vector2>();
            //bind stuff
            VAO.Bind();
            VBO VBO = new VBO(vertices.ToArray());
            VAO.LinkAttrib(VBO, 0, 3, 5 * sizeof(float), 0);
            VAO.LinkAttrib(VBO, 1, 2, 5 * sizeof(float), 3);
            EBO = new EBO(indices.ToArray());
            vertices = new List<Vertex>();
            indices = new List<uint>();

            VAO.Unbind();
            VBO.Unind();
            EBO.Unind();
            EBO.Delete();

        }
        public Mesh() {
            name = "empty";
            List<Vertex> vertices = new List<Vertex>() { new Vertex(new Vector3(0), new Vector2(0)), new Vertex(new Vector3(0), new Vector2(0)), new Vertex(new Vector3(0), new Vector2(0)) };
            List<uint> indices = new List<uint> { 0, 1, 2 };

            lengthI = indices.Count;
            lengthA = vertices.Count;
            VAO.Bind();
            VBO VBO = new VBO(vertices.ToArray());
            EBO = new EBO(indices.ToArray());
            VAO.LinkAttrib(VBO, 0, 3, 5 * sizeof(float), 0);
            VAO.LinkAttrib(VBO, 1, 2, 5 * sizeof(float), 3);

            VAO.Unbind();
            VBO.Unind();
            EBO.Unind();
            EBO.Delete();
        }
        public Mesh(int i) { }
        int lengthA;
        int lengthI;
        public void Draw() {
            VAO.Bind();
            GL.DrawElements(BeginMode.Triangles, lengthI, DrawElementsType.UnsignedInt, 0);
        }

        public object Clone()
        {
            // Step 1: Generate a new buffer
            int newVAO = GL.GenBuffer();

            // Step 2: Bind the original buffer
            GL.BindBuffer(BufferTarget.ArrayBuffer, VAO.ID);

            // Step 3: Get the size of the original buffer
            int bufferSize;
            GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out bufferSize);

            // Step 4: Allocate memory for the new buffer
            GL.BindBuffer(BufferTarget.ArrayBuffer, newVAO);
            GL.BufferData(BufferTarget.ArrayBuffer, bufferSize, IntPtr.Zero, BufferUsageHint.StaticDraw);

            // Step 5: Copy data from the original buffer to the new buffer
            GL.BindBuffer(BufferTarget.CopyReadBuffer, VAO.ID);
            GL.BindBuffer(BufferTarget.CopyWriteBuffer, newVAO);
            GL.CopyBufferSubData(BufferTarget.CopyReadBuffer, BufferTarget.CopyWriteBuffer, IntPtr.Zero, IntPtr.Zero, bufferSize);


            return new Mesh(1)
            {
                VAO = new VAO(newVAO),
                lengthA = lengthA,
                lengthI = lengthI,
            };
        }
        int DuplicateBuffer(int a, int b)
        {

            int newBuffer = GL.GenBuffer();

            // Bind the original buffer for reading
            GL.BindBuffer(BufferTarget.CopyReadBuffer, a);

            // Bind the new buffer for writing
            GL.BindBuffer(BufferTarget.CopyWriteBuffer, newBuffer);

            // Allocate memory for the new buffer and copy the data from the original buffer
            GL.BufferData(BufferTarget.CopyWriteBuffer, b, IntPtr.Zero, BufferUsageHint.StaticDraw);
            GL.CopyBufferSubData(BufferTarget.CopyReadBuffer, BufferTarget.CopyWriteBuffer, IntPtr.Zero, IntPtr.Zero, b);

            // Unbind the buffers
            GL.BindBuffer(BufferTarget.CopyReadBuffer, 0);
            GL.BindBuffer(BufferTarget.CopyWriteBuffer, 0);
            return newBuffer;
        }
    }
}
