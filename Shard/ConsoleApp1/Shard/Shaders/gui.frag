#version 330 core

in vec2 vTexcoord;

out vec4 FragColor;

uniform sampler2D texture0;

void main()
{   
    // Sample Texture
    vec4 texColor = texture(texture0, vTexcoord);

    if (texColor.a <= 0.0){
        discard;
    }

    FragColor = texColor;
}