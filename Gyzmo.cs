using System;
using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Mathematics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Reflection;

namespace EPQui
{
    public class Gyzmo
    {
        Mesh mesh;
        Shader shaderProgram;
        Shader clickProgram;

        public Gyzmo()
        {
            mesh = new Mesh();
            shaderProgram = new Shader("Res/Gyzmo.vert", "Res/light.frag", "Res/Gyzmo.geomertry");
             clickProgram = new Shader("Res/Gyzmo.vert", "Res/ClicksGyz.frag", "Res/Gyzmo.geomertry");
        }
        public void UpdateClick(Camera camera, Matrix4 objectModel)
        {

            clickProgram.Activate();
            GL.UniformMatrix4(GL.GetUniformLocation(clickProgram.ID, "model"), false, ref objectModel);
            GL.Uniform3(GL.GetUniformLocation(clickProgram.ID, "camUp"), camera.OrientationU);
            GL.Uniform3(GL.GetUniformLocation(clickProgram.ID, "camRight"), camera.OrientationR);
            mesh.DrawToClick(clickProgram, camera);
        }
        public void Update(Camera camera, Matrix4 objectModel)
        {

            shaderProgram.Activate();
            GL.UniformMatrix4(GL.GetUniformLocation(shaderProgram.ID, "model"), false, ref objectModel);
            GL.Uniform3(GL.GetUniformLocation(shaderProgram.ID, "camUp"), camera.OrientationU);
            GL.Uniform3(GL.GetUniformLocation(shaderProgram.ID, "camRight"), camera.OrientationR);
            mesh.DrawToClick(shaderProgram, camera);
        }
    }
}
