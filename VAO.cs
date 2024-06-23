using System; using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace EPQui
{
    public class VAO
    {



        public int ID;
        public VAO() {
            ID = GL.GenVertexArray();
        }
        public VAO(int i) {
            ID = i;
        }
        public void LinkAttrib(VBO VBO, uint layout, int numComponents, int stride, int offset) {
            VBO.Bind();

            GL.VertexAttribPointer(layout,numComponents,VertexAttribPointerType.Float,false,stride, offset * sizeof(float));
            GL.EnableVertexAttribArray(layout);
            VBO.Unind();
        }
        public void Bind()
        {
            GL.BindVertexArray(ID);
        }
        public void Unbind()
        {
            GL.BindVertexArray(0);
        }
        public void Delete()
        {
            GL.DeleteVertexArray(ID);
        }

    }
}
