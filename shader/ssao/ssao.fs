#version 330 core
out float FragColor;
in vec2 TexCoords;

uniform sampler2D gPosition; // 视空间位置
uniform sampler2D gNormal;   // 视空间法线
uniform sampler2D texNoise;
uniform vec3 samples[64];

int   kernelSize = 64;
float radius     = 0.5;
float bias       = 0.025;

uniform mat4 projection;

void main()
{
    vec3 fragPos = texture(gPosition, TexCoords).xyz;    // 视空间
    vec3 normal  = normalize(texture(gNormal,   TexCoords).xyz);

    // 自适应屏幕大小：用 GBuffer 尺寸/4 作为噪声平铺
    vec2 noiseScale = vec2(textureSize(gPosition, 0)) / 4.0;
    vec3 randomVec = normalize(texture(texNoise, TexCoords * noiseScale).xyz);

    // TBN：切换到以 normal 为 z 轴的局部坐标
    vec3 tangent   = normalize(randomVec - normal * dot(randomVec, normal));
    vec3 bitangent = cross(normal, tangent);
    mat3 TBN       = mat3(tangent, bitangent, normal);

    float occlusion = 0.0;
    for (int i = 0; i < kernelSize; ++i)
    {
        vec3 samplePos = TBN * samples[i];     // 切到视空间
        samplePos = fragPos + samplePos * radius;

        vec4 offset = projection * vec4(samplePos, 1.0);
        offset.xyz /= max(offset.w, 1e-6);     // 透视除法
        offset.xyz = offset.xyz * 0.5 + 0.5;   // NDC -> [0,1]

        float sampleDepth = texture(gPosition, offset.xy).z; // 视空间 z

        float rangeCheck = smoothstep(0.0, 1.0, radius / (abs(fragPos.z - sampleDepth) + 1e-4));
        occlusion += (sampleDepth >= samplePos.z + bias ? 1.0 : 0.0) * rangeCheck;
    }
    FragColor = 1.0 - (occlusion / float(kernelSize));
}
