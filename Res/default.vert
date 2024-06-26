#version 330 core
    layout (location = 0) in vec3 aPos;
    layout (location = 1) in vec2 aTPos;
     uniform mat4 camMatrix;
     uniform mat4 model;
     uniform vec2 textureOff = vec2(0);
     uniform vec2 textureSca = vec2(1);

out DATA
{
	vec2 texCoord;
    mat4 projection;
} data_out;


void main()
{
   gl_Position = model * vec4(aPos,1.0f);
   data_out.texCoord =textureSca *( aTPos + textureOff);
   data_out.projection = camMatrix;
   
}