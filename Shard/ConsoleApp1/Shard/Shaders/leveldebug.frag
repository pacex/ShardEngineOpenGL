#version 330 core

in vec2 vTexcoord;
in vec3 vViewNormal;
in vec3 vWorldNormal;
in vec3 vWorldPos;

layout(location = 0) out vec4 FragColor;
layout(location = 1) out vec4 FragNormal;
//out vec4 FragColor;

uniform sampler2D texture0;

void main()
{   
    // Trilinear Mapping
    vec4 texColorXY = texture(texture0, 0.5 * vec2(vWorldPos.x, vWorldPos.y));

    vec4 texColorYZ = texture(texture0, 0.5 * vec2(vWorldPos.y, vWorldPos.z));

    vec4 texColorXZ = texture(texture0, 0.5 * vec2(vWorldPos.x, vWorldPos.z));

    vec3 worldNormalNorm = abs(vWorldNormal);

    vec3 w = worldNormalNorm / (worldNormalNorm.x + worldNormalNorm.y + worldNormalNorm.z);

    vec4 texColor = w.x * texColorYZ + w.y * texColorXZ + w.z * texColorXY;

    if (texColor.a <= 0.0){
        discard;
    }

    FragColor = vec4(texColor.rgb, 1.0);
    FragNormal = vec4(vViewNormal * 0.5 + vec3(0.5), 1.0);
}