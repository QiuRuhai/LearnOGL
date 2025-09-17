#version 330 core
layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aNormal;
layout (location = 2) in vec2 aTexCoords;

out vec3 FragPosVS;   // <- 改名强调是 View Space
out vec2 TexCoords;
out vec3 NormalVS;

uniform bool invertedNormals;
uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main()
{
    vec4 worldPos = model * vec4(aPos, 1.0);

    // 视空间位置
    vec4 viewPos  = view * worldPos;
    FragPosVS = viewPos.xyz;

    TexCoords = aTexCoords;

    // 视空间法线（对非均匀缩放必须用逆转置）
    mat3 normalMatrix = mat3(transpose(inverse(view * model)));
    vec3 n = invertedNormals ? -aNormal : aNormal;
    NormalVS = normalize(normalMatrix * n);

    gl_Position = projection * viewPos;
}
