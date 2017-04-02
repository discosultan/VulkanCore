#version 450

layout (binding = 0) uniform sampler2D sampler_Diffuse;

layout (location = 0) in vec4 in_Color;

layout (location = 0) out vec4 out_Color;

void main() 
{
    out_Color.rgb = texture(sampler_Diffuse, gl_PointCoord).rgb * in_Color.rgb;
}
