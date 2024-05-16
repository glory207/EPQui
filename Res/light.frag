#version 330 core

out vec4 FragColor;
in vec4 color;
uniform vec4 lightColor;

void main()
{
	FragColor = lightColor;
}