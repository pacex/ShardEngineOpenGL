#version 330 core

#define MAX_BONES 128

layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec3 aNormal;
layout (location = 2) in vec2 aTexcoord;
layout (location = 3) in vec4 aWeight;
layout (location = 4) in vec4 aBoneId;


out vec2 vTexcoord;
out vec3 vViewNormal;

uniform mat4 model;
uniform mat4 view;
uniform mat4 proj;

uniform mat4[MAX_BONES] boneMatrices;

void main()
{
    vTexcoord = aTexcoord;
    vViewNormal = (view * model * vec4(aNormal, 0.0)).xyz;

    vec4 animatedPos = (aWeight.x * boneMatrices[int(aBoneId.x)] + 
                        aWeight.y * boneMatrices[int(aBoneId.y)] + 
                        aWeight.z * boneMatrices[int(aBoneId.z)] + 
                        aWeight.w * boneMatrices[int(aBoneId.w)]) * vec4(aPosition, 1.0); 

    gl_Position = proj * view * model * animatedPos;
}