using System; using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;



namespace EPQui
{
    internal class EBO
    {
        
        public int ID;
        public EBO(List<uint> indices)
        {
            ID = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ID);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Count * sizeof(float), indices.ToArray(), BufferUsageHint.StreamDraw);

        }
        public void Bind() {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ID);
        }
        public void Unind() {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }
        public void Delete() {
            GL.DeleteBuffer(ID);
        }
    }
}
