#version 450

layout (location = 0) out vec3 out_Color;

out gl_PerVertex
{
    vec4 gl_Position;
};

void main() {
    vec2 pos[3] = vec2[3](vec2(-0.7, 0.7), vec2(0.7, 0.7), vec2(0.0, -0.7));
    vec3 color[3] = vec3[3](vec3(1.0, 0.0, 0.0), vec3(0.0, 1.0, 0.0), vec3(1.0, 1.0, 0.0));

    out_Color = color[gl_VertexIndex];
    gl_Position = vec4(pos[gl_VertexIndex], 0.0, 1.0);
}
