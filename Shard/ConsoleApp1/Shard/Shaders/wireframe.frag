#version 330 core

layout(location = 0) out vec4 FragColor;
layout(location = 1) out vec4 FragNormal;

uniform vec4 color;

void main()
{   
    FragColor = color;
    FragNormal = vec4(0.0);
}