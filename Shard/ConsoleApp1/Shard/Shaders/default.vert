#version 330 core

layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec3 aNormal;
layout (location = 2) in vec2 aTexcoord;


out vec2 vTexcoord;
out vec4 vViewNormal;
out vec4 vViewPos;
out vec4 vWorldNormal;
out vec4 vWorldPos;

uniform mat4 model;
uniform mat4 view;
uniform mat4 proj;

void main()
{
    vTexcoord = aTexcoord;

    vWorldPos = model * vec4(aPosition, 1.0);
    vWorldNormal = model * vec4(aNormal, 0.0);

    vViewPos = view * vWorldPos;
    vViewNormal = view * vWorldNormal;

    gl_Position = proj * vViewPos;
}