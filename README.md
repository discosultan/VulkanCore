# VulkanCore [![AppVeyor](https://img.shields.io/appveyor/ci/gruntjs/grunt.svg)]() [![NuGet Pre Release](https://img.shields.io/nuget/vpre/VulkanCore.svg)]()

Vulkan 1.0 bindings for .NET Standard 1.4

- [Introduction](#introduction)
- [Building](#building)
- [Samples](#samples)
- [Testing](#testing)
- [Related Work](#related-work)

## Introduction

VulkanCore is intended to be a thin cross-platform object-oriented wrapper around the Vulkan C API.

**Why yet another set of bindings?** While most of the alternatives use a generator-based approach, these bindings do not. There are a couple of repercussions from that:

Pros:
- Full control over the API including high quality code documentation.
- Easier to contribute. No need to understand the generator.
- Allows more easily to make exceptions and do specialized optimizations.

Cons:
- Requires manual work after every change to the Vulkan API.
- Difficult to modify the fundamentals. Impossible to simply regenerate everything.
- More susceptible to human error.


## Building

TODO

## Samples

TODO

## Testing

In order to provide a certain level of *functional correctness*, the project aims to achieve *full statement coverage* for the *core API*. Note that it's difficult to test some of the vendor specific extensions due to the requirements for specialized hardware/drivers - therefore, covering them at this point is not planned.

## Related Work

- [VulkanSharp](https://github.com/mono/VulkanSharp)
- [SharpVulkan](https://github.com/jwollen/SharpVulkan)
- [SharpVk](https://github.com/FacticiusVir/SharpVk)