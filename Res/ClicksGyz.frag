#version 330 core

out vec4 FragColor;
in float part; 

void main()
{
    	FragColor = vec4(0,part/255,0,1);
}