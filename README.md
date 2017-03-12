# VulkanCore [![AppVeyor](https://img.shields.io/appveyor/ci/discosultan/vulkancore.svg)](https://ci.appveyor.com/project/discosultan/vulkancore) [![NuGet Pre Release](https://img.shields.io/nuget/vpre/VulkanCore.svg)](https://www.nuget.org/packages/VulkanCore)

Vulkan 1.0 bindings for .NET Standard 1.3

- [Introduction](#introduction)
- [Building](#building)
- [Samples](#samples)
- [Tests](#tests)
- [Related Work](#related-work)

## Introduction

VulkanCore is a thin cross-platform object-oriented wrapper around the Vulkan C API.

**Why yet another set of bindings?** While most of the alternatives use a generator-based approach, these bindings do not. There are a couple of repercussions from that:

Pros:
- Full control over the API including high quality code documentation.
- Easier to contribute. No need to understand the generator.

Cons:
- Requires manual work after every change to the Vulkan API.
- Difficult to modify the fundamentals. Impossible to simply regenerate everything.

## Building

[Visual Studio 2017](https://www.visualstudio.com/vs/whatsnew/) is required to successfully compile the solution. [Rider](https://www.jetbrains.com/rider/), [Visual Studio Code](https://code.visualstudio.com/) and [MonoDevelop](http://www.monodevelop.com/) should also work once they support the new *.csproj* format.

## Samples <img height="24" src="Doc/Windows64.png">

Vulkan-capable graphics hardware and drivers are required to run the samples. Currently only Win32 platform is supported but Ubuntu and Android are planned.

## [01-ClearScreen](Samples/01-ClearScreen)
<img src="Doc/ClearScreen.jpg" alt="ClearScreen" height="96px" align="right">
Sets up a window and clears it to a solid color.
<br><br>

## [02-ColoredTriangle](Samples/02-ColoredTriangle)
<img src="Doc/ColoredTriangle.jpg" alt="ColoredTriangle" height="96px" align="right">
Renders a colored triangle defined in a shader.
<br><br>

## [03-TexturedCube](Samples/03-TexturedCube)
<img src="Doc/TexturedCube.jpg" alt="TexturedCube" height="96px" align="right">
Renders a textured cube.
<br><br>

## Tests <img height="24" src="Doc/Windows64.png"> <img height="24" src="Doc/Ubuntu64.png">

In order to provide a certain level of *functional correctness*, the project aims to achieve *full statement coverage* for the *core API*. Note that it's difficult to test some of the vendor specific extensions due to the requirements for specialized hardware/drivers - therefore, covering them at this point is not planned.

## Related Work

- [VulkanSharp](https://github.com/mono/VulkanSharp)
- [SharpVulkan](https://github.com/jwollen/SharpVulkan)
- [SharpVk](https://github.com/FacticiusVir/SharpVk)
