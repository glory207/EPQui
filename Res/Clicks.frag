#version 330 core

out vec4 FragColor; 
uniform int objectId;  

uniform int objectLength;

void main()
{

    	float color2 = (float(objectId) / float(objectLength) );


	float color = (float(objectId) / 255.0);

    	FragColor = vec4(color,0,0,1);

}