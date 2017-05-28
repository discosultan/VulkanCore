#version 450

layout (location = 0) in vec2 in_Position;
layout (location = 1) in vec2 in_Velocity;
layout (location = 2) in vec4 in_Color;

layout (location = 0) out vec4 out_Color;

out gl_PerVertex
{
    vec4 gl_Position;
    float gl_PointSize;
};

void main () 
{
    gl_Position = vec4(in_Position, 1.0, 1.0);
    gl_PointSize = 4.0;
    out_Color = in_Color;
}
