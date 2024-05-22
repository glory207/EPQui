#version 330 core

out vec4 FragColor; 
uniform int objectId; 

void main()
{

    vec3 color = vec3(float(objectId) / 255.0);


    FragColor = vec4(color, 1.0);
}