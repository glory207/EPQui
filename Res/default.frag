#version 330 core
    out vec4 FragColor;
     uniform vec4 lightColor[50];
     uniform vec3 lightPos[50];
     uniform vec3 lightRot[50];
     uniform int lightnum;
     uniform vec3 camPos;
    in vec3 norm;
    in vec3 crntPos;
    in vec2 TPos;
    uniform sampler2D diffuse0;
    uniform sampler2D specular0;

  
    vec4 pointLight(vec3 lPos,vec4 lColor)
{	
	vec3 lightVec = lPos - crntPos;

	float dist = length(lightVec);
	float a = 3.0;
	float b = 0.7;
	float inten = 50.0f / (a * dist * dist + b * dist + 1.0f);

	float ambient = 0.07f;

	vec3 norm = normalize(norm);
	vec3 lightDirection = normalize(lightVec);
	float diffuse = max(dot(norm, lightDirection), 0.0f);

	float specularLight = 0.50f;
	vec3 viewDirection = normalize(camPos - crntPos);
	vec3 reflectionDirection = reflect(-lightDirection, norm);
	float specAmount = pow(max(dot(viewDirection, reflectionDirection), 0.0f), 16);
	float specular = specAmount * specularLight;

	return (texture(diffuse0, TPos) * (diffuse * inten + ambient) + texture(specular0, TPos).r * specular * inten) * lColor;
}



vec4 direcLight(vec3 lPos,vec4 lColor)
{
	// ambient lighting
	float ambient = 0.20f;
	float inten = 1;
	// diffuse lighting
	vec3 norm = normalize(norm);
	vec3 lightDirection = normalize(vec3(1.0f, 1.0f, 0.0f));
	float diffuse = max(dot(norm, lightDirection), 0.0f);

	// specular lighting
	float specularLight = 0.50f;
	vec3 viewDirection = normalize(camPos - crntPos);
	vec3 reflectionDirection = reflect(-lightDirection, norm);
	float specAmount = pow(max(dot(viewDirection, reflectionDirection), 0.0f), 16);
	float specular = specAmount * specularLight;

	return (texture(diffuse0, TPos) * (diffuse * inten + ambient) + texture(specular0, TPos).r * specular * inten) * lColor;
}

vec4 spotLight(vec3 lPos,vec3 lRot,vec4 lColor)
{
	// controls how big the area that is lit up is
	float outerCone = 0.90f;
	float innerCone = 0.95f;

	// ambient lighting
	float ambient = 0.20f;

	// diffuse lighting
	vec3 norm = normalize(norm);
	//vec3 lightDirection = normalize(lPos - crntPos);
	vec3 lightDirection = normalize(lRot);
	float diffuse = max(dot(norm, lightDirection), 0.0f);

	// specular lighting
	float specularLight = 0.50f;
	vec3 viewDirection = normalize(camPos - crntPos);
	vec3 reflectionDirection = reflect(-lightDirection, norm);
	float specAmount = pow(max(dot(viewDirection, reflectionDirection), 0.0f), 16);
	float specular = specAmount * specularLight;

	// calculates the intensity of the crntPos based on its angle to the center of the light cone
	float angle = dot(vec3(0.0f, -1.0f, 0.0f), -lightDirection);
	float inten = clamp((angle - outerCone) / (innerCone - outerCone), 0.0f, 1.0f);

	return (texture(diffuse0, TPos) * (diffuse * inten + ambient) + texture(specular0, TPos).r * specular * inten) * lColor;
}

    void main()
    {
    
	vec4 CombinedLight;
	for(int i = 0 ; i < lightnum; i++)
	{
	CombinedLight += pointLight(lightPos[i],lightColor[i]);
	}
    FragColor = CombinedLight;


    }
    

