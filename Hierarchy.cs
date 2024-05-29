using System; using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Mathematics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace EPQui
{
   public class Hierarchy: HierObj
    {

        public Mesh gridMesh;
        Shader gridshaderProgram;

        public Hierarchy()
        {
            
            gridMesh = new Mesh();
            gridshaderProgram = new Shader("Res/grid.vert", "Res/grid.frag", "Res/grid.geomertry");

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
