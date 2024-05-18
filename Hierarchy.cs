using System; using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Mathematics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace EPQui
{
   public class Hierarchy
    {
        public List<HierObj> Hobj = new List<HierObj>();

        public Mesh gridMesh;
        Shader gridshaderProgram;

        public Hierarchy()
        {
            
            gridMesh = new Mesh();
            gridshaderProgram = new Shader("Res/grid.vert", "Res/grid.frag", "Res/grid.geomertry");

        }
        public void Update(Camera camera)
        {
            List<LightContainer> lights = new List<LightContainer>();
            foreach (HierObj ob in Hobj)
            {
                if (ob.GetType() == typeof(LightContainer))
                {
                    ob.Update(camera);
                    lights.Add((LightContainer)ob);
                }
            }

            foreach (HierObj ob in Hobj)
            {
                if (ob.GetType() == typeof(MeshContainer)) ob.Update(lights, camera);
            }
            gridshaderProgram.Activate();
            Matrix4 mat = Matrix4.Identity;
            GL.UniformMatrix4(GL.GetUniformLocation(gridshaderProgram.ID, "model"), false, ref mat);
            GL.Uniform4(GL.GetUniformLocation(gridshaderProgram.ID, "lightColor"), 1, 1, 1, 1);
            GL.Uniform3(GL.GetUniformLocation(gridshaderProgram.ID, "camUp"), camera.Orientation);
            gridMesh.Draw(gridshaderProgram, camera);
        }
        public void UpdateClick(Camera camera)
        {
            foreach (HierObj ob in Hobj)
            {
                ob.UpdateClick(camera, Hobj.IndexOf(ob), Hobj.Count);
            }
        }
        public void AddMesh(HierObj hierObj)
        {
            Hobj.Add(hierObj);
        }
        public void DeleteObj(int pos)
        {
            Hobj[pos].destroy();
            Hobj.RemoveAt(pos);
        }
        public void destroy()
        {
            foreach (HierObj ob in Hobj)
            {
                ob.destroy();
            }
            gridshaderProgram.Delete();
        }
    }
}
