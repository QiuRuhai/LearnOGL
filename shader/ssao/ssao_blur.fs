#version 330 core
out float FragColor;
in vec2 TexCoords;

uniform sampler2D ssaoInput;

void main()
{
    vec2 texel = 1.0 / vec2(textureSize(ssaoInput, 0));
    float sum = 0.0;
    for (int x = -2; x <= 2; ++x)        // 注意：<=2 变成 5×5
    for (int y = -2; y <= 2; ++y)
        sum += texture(ssaoInput, TexCoords + vec2(x, y) * texel).r;

    FragColor = sum / 25.0;
}
