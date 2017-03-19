# VulkanCore 

[![Build status](https://ci.appveyor.com/api/projects/status/4q42ayrn28obs8rf/branch/master?svg=true)](https://ci.appveyor.com/project/discosultan/vulkancore/branch/master) [![NuGet Pre Release](https://img.shields.io/nuget/vpre/VulkanCore.svg)](https://www.nuget.org/packages/VulkanCore)

Vulkan 1.0 bindings for .NET Standard 1.3

- [Introduction](#introduction)
- [Building](#building)
- [Samples](#samples--)
- [Tests](#tests--)
- [Related Work](#related-work)

## Introduction

VulkanCore is a thin cross-platform object-oriented wrapper around the Vulkan C API.

**Why yet another set of bindings?** While most of the alternatives use a generator-based approach, these bindings do not. This means:

Pros:
- Full control over the API including high quality code documentation
- Easier to contribute - no need to understand a generator

Cons:
- Requires manual work to keep up to date with the Vulkan API registry
- Cumbersome to modify the fundamentals - impossible to simply regenerate everything

## Building

[Visual Studio 2017](https://www.visualstudio.com/vs/whatsnew/) or equivalent tooling is required to successfully compile the source. The tooling must support the *new .csproj format* and *C# 7* language features. Latest [Rider](https://www.jetbrains.com/rider/), [Visual Studio Code](https://code.visualstudio.com/) or [MonoDevelop](http://www.monodevelop.com/) should all work but have not been tested.

## Samples <img height="24" src="Doc/Windows64.png"> <img height="24" src="Doc/Android64.png">

Vulkan-capable graphics hardware and drivers are required to run the samples. Win32 samples are based on WinForms (.NET Framework) and Android ones run on Xamarin (Mono).

## [01-ClearScreen](Samples/Shared/01-ClearScreen)
<img src="Doc/ClearScreen.jpg" alt="ClearScreen" height="96px" align="right">
Sets up a window and clears it to a solid color.
<br><br>

## [02-ColoredTriangle](Samples/Shared/02-ColoredTriangle)
<img src="Doc/ColoredTriangle.jpg" alt="ColoredTriangle" height="96px" align="right">
Renders a colored triangle defined in a shader.
<br><br>

## [03-TexturedCube](Samples/Shared/03-TexturedCube)
<img src="Doc/TexturedCube.jpg" alt="TexturedCube" height="96px" align="right">
Renders a textured cube.
<br><br>

## Tests <img height="24" src="Doc/Windows64.png"> <img height="24" src="Doc/Ubuntu64.png">

In order to provide a certain level of *functional correctness*, the project aims to achieve *full statement coverage* for the *core API*. Note that it's difficult to test some of the vendor specific extensions due to the requirements for specialized hardware/drivers - therefore, covering them at this point is not planned. Tests are based on .NET Core and have been tested both on Ubuntu and Windows platforms.

## Related Work

- [VulkanSharp](https://github.com/mono/VulkanSharp)
- [SharpVulkan](https://github.com/jwollen/SharpVulkan)
- [SharpVk](https://github.com/FacticiusVir/SharpVk)
- [vk](https://github.com/mellinoe/vk)
