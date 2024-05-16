using System; using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Mathematics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace EPQui
{
    internal class Hierarchy
    {
        public List<HierObj> Hobj;

        public string TempMeshe;
        public Vector4 TempLight;
        public Hierarchy()
        {
            Hobj = new List<HierObj>() {
                new MeshContainer(new Vector3(0,0,0), "Res/Cube.txt"),
                new LightContainer(new Vector3(-1.0f, 2.8f, 0.8f), new Vector4(1.0f, 0.0f, 0.0f, 1.0f)),
                new LightContainer(new Vector3(1.0f, 2.8f, 0.8f), new Vector4(0.0f, 1.0f, 0.0f, 1.0f)),
                new LightContainer(new Vector3(0.0f, 2.8f, 0.8f), new Vector4(0.0f, 0.0f, 1.0f, 1.0f)),
            };
            shouldAdd[0] = false;
            shouldAdd[1] = false;
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
            if (shouldAdd[0])
            {
                shouldAdd[0] = false;
                Hobj.Add(new MeshContainer(camera.Position, TempMeshe));
            }
            if (shouldAdd[1])
            {
                shouldAdd[1] = false;
                Hobj.Add(new LightContainer(camera.Position, TempLight));
            }
        }
        public void UpdateClick(Camera camera)
        {
            foreach (HierObj ob in Hobj)
            {
                ob.UpdateClick(camera, Hobj.IndexOf(ob) + 1);
            }
        }
        public bool[] shouldAdd = new bool[2];
        public void AddMesh(string msh)
        {
            TempMeshe = msh;
            shouldAdd[0] = true;
        }
        public void AddLight(Vector3 col)
        {
            TempLight = new Vector4(col, 1);
            shouldAdd[1] = true;
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
        }
    }
}
