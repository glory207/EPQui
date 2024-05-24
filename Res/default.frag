#version 330 core
    out vec4 FragColor;
     uniform vec4 lightColor[50];
     uniform vec3 lightPos[50];
     uniform vec2 lightAng[50];
     uniform int lightType[50];
     uniform float lightIntensity[50];

     uniform mat4 lightRot[50];
     uniform int lightnum;
     uniform vec3 camPos;

    in vec3 norm;
    in vec3 crntPos;
    in vec2 TPos;
    uniform sampler2D diffuse0;
    uniform sampler2D specular0;

  
    vec4 pointLight(vec3 lPos,vec4 lColor, float lightI)
{	
	vec3 lightVec = lPos - crntPos;

	float dist = length(lightVec);
	float a = 3.0;
	float b = 0.7;
	float inten = lightI *100f / (a * dist * dist + b * dist + 1.0f);

	float ambient = 0.07f;

	vec3 norm = normalize(norm);
	vec3 lightDirection = normalize(lightVec);
	float diffuse = max(dot(norm, lightDirection), 0.0f);

	float specularLight = 0.50f;
	vec3 viewDirection = normalize(camPos - crntPos);
	vec3 reflectionDirection = reflect(-lightDirection, norm);
	float specAmount = pow(max(dot(viewDirection, reflectionDirection), 0.0f), 16);
	float specular = specAmount * specularLight;

	return (texture(diffuse0, TPos) * ((diffuse * inten) + ambient) + texture(specular0, TPos).r * specular * inten) * lColor;
}



vec4 direcLight(vec3 lPos,vec3 rot,vec4 lColor, float lightI)
{
	// ambient lighting
	float ambient = 0.20f;
	float inten = lightI;

	// diffuse lighting
	vec3 norm = normalize(norm);
	vec3 lightDirection = normalize(rot);
	float diffuse = max(dot(norm, lightDirection), 0.0f);

	// specular lighting
	float specularLight = 0.50f;
	vec3 viewDirection = normalize(camPos - crntPos);
	vec3 reflectionDirection = reflect(-lightDirection, norm);
	float specAmount = pow(max(dot(viewDirection, reflectionDirection), 0.0f), 16);
	float specular = specAmount * specularLight;

	return (texture(diffuse0, TPos) * ((diffuse * inten) + ambient) + texture(specular0, TPos).r * specular * inten) * lColor;
}

vec4 spotLight(vec3 lPos,vec3 rot,vec4 lColor, float lightI, float outerCone, float innerCone)
{

	// ambient lighting
	float ambient = 0.20f;

	// diffuse lighting
	vec3 norm = normalize(norm);
	vec3 lightDirection = normalize(lPos - crntPos);
	float diffuse = max(dot(norm, lightDirection), 0.0f);

	// specular lighting
	float specularLight = 0.50f;
	vec3 viewDirection = normalize(camPos - crntPos);
	vec3 reflectionDirection = reflect(-lightDirection, norm);
	float specAmount = pow(max(dot(viewDirection, reflectionDirection), 0.0f), 16);
	float specular = specAmount * specularLight;

	// calculates the intensity of the crntPos based on its angle to the center of the light cone
	float angle = dot(rot, -lightDirection);
	float inten = clamp((angle + outerCone -1.57f)/(outerCone*innerCone), 0.0f, 1.0f) * lightI;

	return (texture(diffuse0, TPos) * ((diffuse * inten) + ambient) + texture(specular0, TPos).r * specular * inten) * lColor;
}

    void main()
    {

	vec4 CombinedLight;
	for(int i = 0 ; i < lightnum; i++)
	{

	if(lightType[i] == 0){
	CombinedLight += pointLight(lightPos[i],lightColor[i],lightIntensity[i]);}
	if(lightType[i] == 1){
	vec3 rot =  (vec4(0,-1,0,0) * -lightRot[i]).xyz;
	CombinedLight += direcLight(lightPos[i], rot,lightColor[i],lightIntensity[i]);}
	if(lightType[i] == 2){
	vec3 rot =  (vec4(0,-1,0,0) * lightRot[i]).xyz;
	CombinedLight += spotLight(lightPos[i], rot,lightColor[i],lightIntensity[i],lightAng[i].x,lightAng[i].y);}



	}
    FragColor = CombinedLight;


    }
    

