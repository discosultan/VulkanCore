REM Make the directory batch file resides in as the working directory.
pushd %~dp0

glslangvalidator -V shader.vert -o shader.vert.spv
glslangvalidator -V shader.frag -o shader.frag.spv

popd
