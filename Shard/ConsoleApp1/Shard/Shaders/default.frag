#version 330 core

in vec2 vTexcoord;

out vec4 FragColor;

uniform sampler2D texture0;

void main()
{
    FragColor = texture(texture0, vTexcoord);
    //FragColor = vec4(vTexcoord, 1.0, 1.0);
}