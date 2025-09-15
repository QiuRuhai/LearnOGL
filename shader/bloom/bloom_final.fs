#version 330 core
out vec4 FragColor;
in vec2 TexCoords;

uniform sampler2D scene;
uniform sampler2D bloomBlur;
uniform bool bloom;
uniform float exposure;

// 新增：放大 Bloom 观察
uniform float bloomStrength = 1.5;  // 先设 1.5~2.0 看差异

void main()
{
    const float gamma = 2.2;

    vec3 hdrColor   = texture(scene,     TexCoords).rgb;
    vec3 bloomColor = texture(bloomBlur, TexCoords).rgb;

    if (bloom) {
        hdrColor += bloomStrength * bloomColor;  // 显著一点更容易看出效果
    }

    // HDR域做曝光，再做gamma
    vec3 mapped = vec3(1.0) - exp(-hdrColor * exposure);
    mapped = pow(mapped, vec3(1.0 / gamma));

    FragColor = vec4(mapped, 1.0);
}
