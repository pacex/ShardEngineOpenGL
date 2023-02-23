#version 330 core

in vec2 vTexcoord;
in vec4 vWorldNormal;
in vec4 vWorldPos;
in vec4 vViewNormal;
in vec4 vViewPos;

out vec4 FragColor;

uniform sampler2D texture0;

void main()
{   
    // Sample Texture
    vec4 texColor = texture(texture0, vTexcoord);

    if (texColor.a <= 0.0){
        discard;
    }

    // Lighting
    vec3 ambientColor = vec3(0.3);
    vec3 directionalColor = vec3(0.6);

    vec3 lightDirection = normalize(vec3(-0.3, 0.6, -1.0));

    vec3 lighting = clamp(dot(vWorldNormal.xyz, -lightDirection), 0.0, 1.0) * directionalColor;

    FragColor = vec4(texColor.rgb * (lighting + ambientColor), 1.0);
}