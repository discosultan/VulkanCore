#version 450

layout (binding = 1) uniform sampler2D sampler_Color;

layout (location = 0) in vec2 in_TexCoord;

layout (location = 0) out vec4 out_Color;

void main() {
    vec4 color = texture(sampler_Color, in_TexCoord);

    out_Color = vec4(color.rgb, 1.0);
}
