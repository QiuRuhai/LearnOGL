#version 330 core
layout (location = 0) out vec3 gPosition; // 存视空间位置
layout (location = 1) out vec3 gNormal;   // 存视空间法线（单位向量）
layout (location = 2) out vec3 gAlbedo;

in vec2 TexCoords;
in vec3 FragPosVS;
in vec3 NormalVS;

void main()
{
    gPosition = FragPosVS;          // 视空间
    gNormal   = normalize(NormalVS); // 保险再归一
    gAlbedo   = vec3(0.95);
}
