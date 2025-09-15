#version 330 core
out vec4 FragColor;
in vec2 TexCoords;

uniform sampler2D tex;
uniform bool applyTonemap = false;  // HDR时建议开
uniform float exposure = 1.0;

void main() {
    vec3 c = texture(tex, TexCoords).rgb;
    if (applyTonemap) {
        c = vec3(1.0) - exp(-c * exposure); // 简单Reinhard曝光
        c = pow(c, vec3(1.0/2.2));          // gamma
    }
    FragColor = vec4(c, 1.0);
}
