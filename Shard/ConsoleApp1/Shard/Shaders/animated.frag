#version 330 core

in vec2 vTexcoord;
in vec3 vViewNormal;

layout(location = 0) out vec4 FragColor;
layout(location = 1) out vec4 FragNormal;

uniform sampler2D texture0;

uniform int frameCount;
uniform int frameIndex;

void main()
{   
    // Remap Texcoords
    vec2 animatedTexcoord;

    float frameWidth = 1.0 / float(frameCount);
    animatedTexcoord = vec2((vTexcoord.x + float(frameIndex)) * frameWidth, vTexcoord.y);

    // Sample Texture
    vec4 texColor = texture(texture0, animatedTexcoord);

    if (texColor.a <= 0.0){
        discard;
    }

    FragColor = vec4(texColor.rgb, 1.0);
    FragNormal = vec4(vViewNormal * 0.5 + vec3(0.5), 1.0);
}