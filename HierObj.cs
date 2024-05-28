using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using static System.Net.Mime.MediaTypeNames;
using OpenTK.Graphics.OpenGL4;
using System.Windows.Media.Media3D;

namespace EPQui
{

    public abstract class HierObj
    {
        public Vector3 Position;
        public Vector3 objectScale;
        public Vector3 objectRotation;
        public Vector4 lightColor;
        public Matrix4 objectModel;
        public Mesh mesh;
        public Shader shaderProgram;
        public Shader clickProgram;
        public List<HierObj> children = new List<HierObj>();
        public HierObj parent;
        public string name = "new Obj"; 
        public abstract void Update(List<LightContainer> lights, Camera camera);
        public abstract void UpdateClick(Camera camera, int value, int value2);
        public abstract void destroy();
    }
    public class material
    {
       public List<Texture> textures = new List<Texture>() {
                 new Texture("Res/planks.png", "diffuse", 0, PixelFormat.Rgba)
            };
        public Vector2 texOff = new Vector2(0);
        public Vector2 texScale = new Vector2(1);
    }
}
