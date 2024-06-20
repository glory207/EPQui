using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using static System.Net.Mime.MediaTypeNames;
using OpenTK.Graphics.OpenGL4;
using System.Net;
using System.Windows.Controls;
using System.Xml.Linq;

namespace EPQui
{

    public abstract class HierObj
    {
        public Vector3 PositionAdded;
        public Vector3 Position;
        public Vector3 objectScale;
        public Vector3 objectScaleAdded;
        public Quaternion objectRotation;
        public Quaternion objectRotationAdded;
        public Vector4 lightColor;
        public Matrix4 objectModel;
        public Matrix4 rotationMatrix = Matrix4.Identity;
        public Mesh mesh;
        public List<HierObj> children = new List<HierObj>();
        public HierObj parent;
        public string name = "new Obj"; 
        public abstract void PreUpdate();
        public abstract void Update(List<LightContainer> lights, Camera camera);
        public abstract void UpdateClick(Camera camera,Shader shader);
        public abstract void destroy();

    }
    public class material:ICloneable
    {
       public List<Texture> textures = new List<Texture>() {
               //  new Texture("Res/textures/planks.png", "diffuse", PixelFormat.Rgba)
            };
        public Vector2 texOff = new Vector2(0);
        public Vector2 texScale = new Vector2(1);
        public float diffuce =1;
        public float specular =1;

        public object Clone()
        {
            return new material
            {
                texOff = texOff,
                texScale = texScale,
                textures = new List<Texture>(textures),
                diffuce = diffuce,
                specular = specular
                
            };
        }
    }
}
