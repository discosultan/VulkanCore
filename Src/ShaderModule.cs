using System;
using System.Runtime.InteropServices;
using static VulkanCore.Constants;

namespace VulkanCore
{
    /// <summary>
    /// Opaque handle to a shader module object.
    /// <para>
    /// Shader modules contain shader code and one or more entry points. Shaders are selected from a
    /// shader module by specifying an entry point as part of pipeline creation. The stages of a
    /// pipeline can use shaders that come from different modules. The shader code defining a shader
    /// module must be in the SPIR-V format.
    /// </para>
    /// </summary>
    public unsafe class ShaderModule : DisposableHandle<long>
    {
        internal ShaderModule(Device parent, ref ShaderModuleCreateInfo createInfo, ref AllocationCallbacks? allocator)
        {
            Parent = parent;
            Allocator = allocator;

            fixed (byte* codePtr = createInfo.Code)
            {
                createInfo.ToNative(out ShaderModuleCreateInfo.Native nativeCreateInfo, codePtr);
                long handle;
                Result result = CreateShaderModule(Parent, &nativeCreateInfo, NativeAllocator, &handle);
                VulkanException.ThrowForInvalidResult(result);
                Handle = handle;
            }
        }

        /// <summary>
        /// Gets the owner of the resource.
        /// </summary>
        public Device Parent { get; }

        protected override void DisposeManaged()
        {
            DestroyShaderModule(Parent, this, NativeAllocator);
            base.DisposeManaged();
        }

        [DllImport(VulkanDll, EntryPoint = "vkCreateShaderModule", CallingConvention = CallConv)]
        private static extern Result CreateShaderModule(IntPtr device, ShaderModuleCreateInfo.Native* createInfo, 
            AllocationCallbacks.Native* allocator, long* shaderModule);

        [DllImport(VulkanDll, EntryPoint = "vkDestroyShaderModule", CallingConvention = CallConv)]
        private static extern void DestroyShaderModule(IntPtr device, long shaderModule, AllocationCallbacks.Native* allocator);
    }

    /// <summary>
    /// Structure specifying parameters of a newly created shader module. 
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct ShaderModuleCreateInfo
    {
        /// <summary>
        /// The code that is used to create the shader module. The type and format of the code is
        /// determined from the content of the code.        
        /// <para>
        /// Length must be a multiple of 4. If the "VK_NV_glsl_shader" extension is enabled and code
        /// references GLSL code, it can be a multiple of 1.
        /// </para>
        /// </summary>
        public byte[] Code;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShaderModuleCreateInfo"/> structure.
        /// </summary>
        /// <param name="code">
        /// The code that is used to create the shader module. The type and format of the code is
        /// determined from the content of the code.
        /// <para>
        /// Length must be a multiple of 4. If the "VK_NV_glsl_shader" extension is enabled and code
        /// references GLSL code, it can be a multiple of 1.
        /// </para>
        /// </param>
        public ShaderModuleCreateInfo(byte[] code)
        {
            Code = code;
        }

        internal struct Native
        {
            public StructureType Type;
            public IntPtr Next;
            public ShaderModuleCreateFlags Flags;
            public Size CodeSize;
            public byte* Code;
        }

        internal void ToNative(out Native native, byte* code)
        {
            native.Type = StructureType.ShaderModuleCreateInfo;
            native.Next = IntPtr.Zero;
            native.Flags = ShaderModuleCreateFlags.None;
            native.CodeSize = Code?.Length ?? 0;
            native.Code = code;
        }
    }

    // Is reserved for future use.
    internal enum ShaderModuleCreateFlags
    {
        None = 0
    }
}