#version 330 core
out vec4 FragColor;
in vec2 TexCoords;

uniform sampler2D gPosition;
uniform sampler2D gNormal;
uniform sampler2D gAlbedo;
uniform sampler2D ssao;

struct Light {
    vec3 Position;   // <- 这里要传“视空间”的灯光位置！
    vec3 Color;
    float Linear;
    float Quadratic;
};
uniform Light light;

void main()
{
    vec3 FragPos = texture(gPosition, TexCoords).rgb;   // 视空间
    vec3 Normal  = normalize(texture(gNormal,  TexCoords).rgb);
    vec3 Diffuse = texture(gAlbedo,  TexCoords).rgb;
    float AmbientOcclusion = texture(ssao, TexCoords).r;

    // 相机在视空间原点，指向片元的视线是 -FragPos
    vec3 viewDir  = normalize(-FragPos);
    vec3 lightDir = normalize(light.Position - FragPos);

    // 光照
    vec3 ambient  = Diffuse * 0.3 * AmbientOcclusion;
    float NdotL   = max(dot(Normal, lightDir), 0.0);
    vec3  diffuse = NdotL * Diffuse * light.Color;

    vec3  halfwayDir = normalize(lightDir + viewDir);
    float spec = pow(max(dot(Normal, halfwayDir), 0.0), 8.0);
    vec3  specular = spec * light.Color;

    float distance    = length(light.Position - FragPos);
    float attenuation = 1.0 / (1.0 + light.Linear * distance + light.Quadratic * distance*distance);
    vec3 lighting = ambient + (diffuse + specular) * attenuation;

    FragColor = vec4(lighting, 1.0);
}
