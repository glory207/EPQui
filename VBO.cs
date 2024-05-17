using System; using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;

namespace EPQui
{
    public class VBO
    {
        

        public int ID;
        public VBO(List<Vertex> vertices)
        {
            ID = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, ID);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Count * sizeof(float) * 6, vertices.ToArray(), BufferUsageHint.StreamDraw);
          
        }
        public VBO(List<float> vertices)
        {
            ID = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, ID);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Count * sizeof(float), vertices.ToArray(), BufferUsageHint.StreamDraw);

        }
        public void Bind() {
            GL.BindBuffer(BufferTarget.ArrayBuffer, ID);
        }
        public void Unind() {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }
        public void Delete() {
            GL.DeleteBuffer(ID);
        }
    }
}
