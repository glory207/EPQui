using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public abstract void Update(Camera camera);
        public abstract void Update(List<LightContainer> lights, Camera camera);
        public abstract void UpdateClick(Camera camera, int value, int value2);
        public abstract void destroy();
    }
}
