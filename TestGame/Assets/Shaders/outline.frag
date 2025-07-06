#version 330

in vec2 fragTexCoord;
in vec4 fragColor;

out vec4 finalColor;

uniform sampler2D texture0;
uniform vec4 colDiffuse;
uniform vec2 resolution;
uniform float thickness;
uniform vec4 outlineColor;

void main()
{
    // Use both dimensions for proper aspect scaling
    float offsetX = thickness / resolution.x;
    float offsetY = thickness / resolution.y;
    vec4 center = texture(texture0, fragTexCoord);

    float alpha = center.a;

    // Sample in 8 directions for better outline detection
    float outlineAlpha = 0.0;
    outlineAlpha += texture(texture0, fragTexCoord + vec2(-offsetX, 0.0)).a;
    outlineAlpha += texture(texture0, fragTexCoord + vec2(offsetX, 0.0)).a;
    outlineAlpha += texture(texture0, fragTexCoord + vec2(0.0, -offsetY)).a;
    outlineAlpha += texture(texture0, fragTexCoord + vec2(0.0, offsetY)).a;
    // Add diagonal samples
    outlineAlpha += texture(texture0, fragTexCoord + vec2(-offsetX, -offsetY)).a;
    outlineAlpha += texture(texture0, fragTexCoord + vec2(offsetX, -offsetY)).a;
    outlineAlpha += texture(texture0, fragTexCoord + vec2(-offsetX, offsetY)).a;
    outlineAlpha += texture(texture0, fragTexCoord + vec2(offsetX, offsetY)).a;

    if (alpha < 0.1 && outlineAlpha > 0.0)
        finalColor = outlineColor;
    else
        finalColor = center * colDiffuse;
}