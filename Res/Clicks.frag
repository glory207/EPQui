#version 330 core

out int FragColor; 
uniform int objectId;  
uniform int objectLength; 

void main()
{
    	FragColor = objectId;
}