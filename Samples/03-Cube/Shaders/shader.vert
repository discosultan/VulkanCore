#version 450

layout (location = 0) in vec3 in_Position;

layout (binding = 0) uniform PerFrame
{
    mat4 World;
    mat4 View;
    mat4 Projection;
};

out gl_PerVertex
{
    vec4 gl_Position;
};

void main() {
    gl_Position = Projection * View * World * vec4(in_Position.xyz, 1.0);
}