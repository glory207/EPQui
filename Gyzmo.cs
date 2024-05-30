using System;
using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Mathematics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Reflection;
using static OpenTK.Graphics.OpenGL.GL;

namespace EPQui
{
    public class Gyzmo
    {
       public Mesh mesh;
        public Shader[] shaderProgram;
        public Shader[] clickProgram;
        public Gyzmo()
        {
            mesh = new Mesh();
            shaderProgram = new Shader[] {
                new Shader("Res/Gyzmo.vert", "Res/light.frag", "Res/RotationGyzmo .geomertry"),
                new Shader("Res/Gyzmo.vert", "Res/light.frag", "Res/TransGyzmo.geomertry"),
                new Shader("Res/Gyzmo.vert", "Res/light.frag", "Res/ScaleGyzmo.geomertry")
            };
            clickProgram = new Shader[] { 
                new Shader("Res/Gyzmo.vert", "Res/ClicksGyz.frag", "Res/RotationGyzmo .geomertry"),
                new Shader("Res/Gyzmo.vert", "Res/ClicksGyz.frag", "Res/TransGyzmo.geomertry"),
                new Shader("Res/Gyzmo.vert", "Res/ClicksGyz.frag", "Res/ScaleGyzmo.geomertry")
            };
        }
        public void UpdateClick(Camera camera, Matrix4 objectModel)
        {
            for (int i = 0; i < clickProgram.Length; i++)
            {
                clickProgram[i].Activate();
                GL.UniformMatrix4(GL.GetUniformLocation(clickProgram[i].ID, "model"), false, ref objectModel);
                GL.Uniform3(GL.GetUniformLocation(clickProgram[i].ID, "camUp"), camera.OrientationU);
                GL.Uniform3(GL.GetUniformLocation(clickProgram[i].ID, "camRight"), camera.OrientationR);
                GL.Uniform3(GL.GetUniformLocation(clickProgram[i].ID, "camFr"), camera.Orientation);
                GL.Uniform3(GL.GetUniformLocation(clickProgram[i].ID, "camP"), camera.Position);
                mesh.DrawToClick(clickProgram[i], camera);
            }
        }
        public void Update(Camera camera, Matrix4 objectModel)
        {
            for (int i = 0; i < shaderProgram.Length; i++)
            {
                shaderProgram[i].Activate();
                GL.UniformMatrix4(GL.GetUniformLocation(shaderProgram[i].ID, "model"), false, ref objectModel);
                GL.Uniform3(GL.GetUniformLocation(shaderProgram[i].ID, "camUp"), camera.OrientationU);
                GL.Uniform3(GL.GetUniformLocation(shaderProgram[i].ID, "camRight"), camera.OrientationR);
                GL.Uniform3(GL.GetUniformLocation(shaderProgram[i].ID, "camFr"), camera.Orientation);
                GL.Uniform3(GL.GetUniformLocation(shaderProgram[i].ID, "camP"), camera.Position);
                mesh.DrawToClick(shaderProgram[i], camera);
            }

        }
    }
}
