#version 330 core

in vec2 vTexcoord;

out vec4 FragColor;

uniform sampler2D diffuseTexture;
uniform sampler2D normalTexture;
uniform sampler2D depthTexture;

void main()
{   
    // Sample Texture
    vec4 color = texture(diffuseTexture, vTexcoord);

    vec4 normal = texture(normalTexture, vTexcoord);

    vec4 depth = texture(depthTexture, vTexcoord);

    if (color.a <= 0.0){
        discard;
    }

    FragColor = color;
}