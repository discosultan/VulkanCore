REM Make the directory batch file resides in as the working directory.
pushd %~dp0

glslangvalidator -V Shader.vert -o Shader.vert.spv
glslangvalidator -V Shader.frag -o Shader.frag.spv

popd
