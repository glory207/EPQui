using System; using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Metadata;
using System.IO;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace EPQui
{
    public class Shader
    {
       

        public int ID;
        public Shader(string vertexFile, string fragmentFile) {

            int VertexShader;
            int FragmentShader;
            // set the shader
            string VertexShaderSource = File.ReadAllText(vertexFile);
            string FragmentShaderSource = File.ReadAllText(fragmentFile);

            // generate shader
            VertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(VertexShader, VertexShaderSource);

            FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(FragmentShader, FragmentShaderSource);


            // compile shader and error cheak
            GL.CompileShader(VertexShader);

            GL.GetShader(VertexShader, ShaderParameter.CompileStatus, out int success);
            if (success == 0)
            {
                string infoLog = GL.GetShaderInfoLog(VertexShader);
                Console.WriteLine(infoLog);
            }

            GL.CompileShader(FragmentShader);

            GL.GetShader(FragmentShader, ShaderParameter.CompileStatus, out success);
            if (success == 0)
            {
                string infoLog = GL.GetShaderInfoLog(FragmentShader);
                Console.WriteLine(infoLog);
            }


            // link the fragment and vertex shaders
            ID = GL.CreateProgram();

            GL.AttachShader(ID, VertexShader);
            GL.AttachShader(ID, FragmentShader);

            GL.LinkProgram(ID);

            GL.GetProgram(ID, GetProgramParameterName.LinkStatus, out success);
            if (success == 0)
            {
                string infoLog = GL.GetProgramInfoLog(ID);
                Console.WriteLine(infoLog);
            }


            // cleanup by deleting the shaders
            GL.DetachShader(ID, VertexShader);
            GL.DetachShader(ID, FragmentShader);
            GL.DeleteShader(FragmentShader);
            GL.DeleteShader(VertexShader);
        }
        public Shader(string vertexFile, string fragmentFile, string geometryFile) {

            int VertexShader;
            int FragmentShader;
            int GeometryShader;
            // set the shader
            string VertexShaderSource = File.ReadAllText(vertexFile);
            string FragmentShaderSource = File.ReadAllText(fragmentFile);
            string GeometryShaderSource = File.ReadAllText(geometryFile);

            // generate shader
            VertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(VertexShader, VertexShaderSource);

            FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(FragmentShader, FragmentShaderSource);

            GeometryShader = GL.CreateShader(ShaderType.GeometryShader);
            GL.ShaderSource(GeometryShader, GeometryShaderSource);

            // compile shader and error cheak
            GL.CompileShader(VertexShader);

            GL.GetShader(VertexShader, ShaderParameter.CompileStatus, out int success);
            if (success == 0)
            {
                string infoLog = GL.GetShaderInfoLog(VertexShader);
                Console.WriteLine(infoLog);
            }

            GL.CompileShader(FragmentShader);

            GL.GetShader(FragmentShader, ShaderParameter.CompileStatus, out success);
            if (success == 0)
            {
                string infoLog = GL.GetShaderInfoLog(FragmentShader);
                Console.WriteLine(infoLog);
            }

            GL.CompileShader(GeometryShader);

            GL.GetShader(GeometryShader, ShaderParameter.CompileStatus, out success);
            if (success == 0)
            {
                string infoLog = GL.GetShaderInfoLog(GeometryShader);
                Console.WriteLine(infoLog);
            }


            // link the fragment and vertex shaders
            ID = GL.CreateProgram();

            GL.AttachShader(ID, VertexShader);
            GL.AttachShader(ID, FragmentShader);
            GL.AttachShader(ID, GeometryShader);

            GL.LinkProgram(ID);

            GL.GetProgram(ID, GetProgramParameterName.LinkStatus, out success);
            if (success == 0)
            {
                string infoLog = GL.GetProgramInfoLog(ID);
                Console.WriteLine(infoLog);
            }


            // cleanup by deleting the shaders
            GL.DetachShader(ID, VertexShader);
            GL.DetachShader(ID, FragmentShader);
            GL.DetachShader(ID, GeometryShader);
            GL.DeleteShader(FragmentShader);
            GL.DeleteShader(VertexShader);
            GL.DeleteShader(GeometryShader);
        }
        public Shader() {
            int VertexShader;
            int FragmentShader;
            int GeometryShader;
            // set the shader
            string VertexShaderSource = File.ReadAllText("Res/default.vert");
            string FragmentShaderSource = File.ReadAllText("Res/default.frag");
            string GeometryShaderSource = File.ReadAllText("Res/default.geometry");

            // generate shader
            VertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(VertexShader, VertexShaderSource);

            FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(FragmentShader, FragmentShaderSource);

            GeometryShader = GL.CreateShader(ShaderType.GeometryShader);
            GL.ShaderSource(GeometryShader, GeometryShaderSource);

            // compile shader and error cheak
            GL.CompileShader(VertexShader);

            GL.GetShader(VertexShader, ShaderParameter.CompileStatus, out int success);
            if (success == 0)
            {
                string infoLog = GL.GetShaderInfoLog(VertexShader);
                Console.WriteLine(infoLog);
            }

            GL.CompileShader(FragmentShader);

            GL.GetShader(FragmentShader, ShaderParameter.CompileStatus, out success);
            if (success == 0)
            {
                string infoLog = GL.GetShaderInfoLog(FragmentShader);
                Console.WriteLine(infoLog);
            }
            
            GL.CompileShader(GeometryShader);

            GL.GetShader(GeometryShader, ShaderParameter.CompileStatus, out success);
            if (success == 0)
            {
                string infoLog = GL.GetShaderInfoLog(GeometryShader);
                Console.WriteLine(infoLog);
            }


            // link the fragment and vertex shaders
            ID = GL.CreateProgram();

            GL.AttachShader(ID, VertexShader);
            GL.AttachShader(ID, FragmentShader);
            GL.AttachShader(ID, GeometryShader);

            GL.LinkProgram(ID);

            GL.GetProgram(ID, GetProgramParameterName.LinkStatus, out success);
            if (success == 0)
            {
                string infoLog = GL.GetProgramInfoLog(ID);
                Console.WriteLine(infoLog);
            }


            // cleanup by deleting the shaders
            GL.DetachShader(ID, VertexShader);
            GL.DetachShader(ID, FragmentShader);
            GL.DetachShader(ID, GeometryShader);
            GL.DeleteShader(FragmentShader);
            GL.DeleteShader(VertexShader);
            GL.DeleteShader(GeometryShader);

        }
        public void Activate() { GL.UseProgram(ID);}
        public void Delete() { GL.DeleteProgram(ID);}
    }
}
