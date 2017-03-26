#version 450

layout (location = 0) in vec2 in_Position;
layout (location = 1) in vec4 inGradientPos;

out gl_PerVertex
{
    vec4 gl_Position;
    float gl_PointSize;
};

void main () 
{
    gl_PointSize = 8.0;
    gl_Position = vec4(in_Position.xy, 1.0, 1.0);
}