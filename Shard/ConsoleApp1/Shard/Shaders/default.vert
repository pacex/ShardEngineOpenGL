﻿#version 330 core

layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec3 aNormal;
layout (location = 2) in vec2 aTexcoord;

out vec3 vWorldPos;
out vec2 vTexcoord;
out vec3 vViewNormal;
out vec3 vWorldNormal;

uniform mat4 model;
uniform mat4 view;
uniform mat4 proj;

void main()
{
    vTexcoord = aTexcoord;
    vViewNormal = (view * model * vec4(aNormal, 0.0)).xyz;
    vWorldPos = (model * vec4(aPosition, 1.0)).xyz;
    vWorldNormal = (model * vec4(aNormal, 0.0)).xyz;

    gl_Position = proj * view * model * vec4(aPosition, 1.0);
}