#version 330 core

in vec4 vColor;
in vec2 vTexcoord;

out vec4 FragColor;

void main()
{
    FragColor = vColor;
}