#version 330 core

layout (location = 0) in vec3 aPos;

uniform mat4 model;
uniform mat4 camMatrix;
     
out DATA
{
    mat4 projection;
} data_out;

void main()
{
	gl_Position = model * vec4(aPos,1.0f);
   data_out.projection = camMatrix;
}