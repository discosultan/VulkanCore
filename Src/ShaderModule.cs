using System;
using System.Runtime.InteropServices;

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
                Result result = vkCreateShaderModule(Parent, &nativeCreateInfo, NativeAllocator, &handle);
                VulkanException.ThrowForInvalidResult(result);
                Handle = handle;
            }
        }

        /// <summary>
        /// Gets the owner of the resource.
        /// </summary>
        public Device Parent { get; }

        /// <summary>
        /// Destroy a shader module.
        /// </summary>
        public override void Dispose()
        {
            if (!Disposed) vkDestroyShaderModule(Parent, this, NativeAllocator);
            base.Dispose();
        }

        private delegate Result vkCreateShaderModuleDelegate(IntPtr device, ShaderModuleCreateInfo.Native* createInfo, AllocationCallbacks.Native* allocator, long* shaderModule);
        private static readonly vkCreateShaderModuleDelegate vkCreateShaderModule = VulkanLibrary.GetStaticProc<vkCreateShaderModuleDelegate>(nameof(vkCreateShaderModule));

        private delegate void vkDestroyShaderModuleDelegate(IntPtr device, long shaderModule, AllocationCallbacks.Native* allocator);
        private static readonly vkDestroyShaderModuleDelegate vkDestroyShaderModule = VulkanLibrary.GetStaticProc<vkDestroyShaderModuleDelegate>(nameof(vkDestroyShaderModule));
    }

    /// <summary>
    /// Structure specifying parameters of a newly created shader module.
    /// </summary>
    public unsafe struct ShaderModuleCreateInfo
    {
        /// <summary>
        /// The code that is used to create the shader module. The type and format of the code is
        /// determined from the content of the code.
        /// </summary>
        public byte[] Code;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShaderModuleCreateInfo"/> structure.
        /// </summary>
        /// <param name="code">
        /// The code that is used to create the shader module. The type and format of the code is
        /// determined from the content of the code.
        /// </param>
        public ShaderModuleCreateInfo(byte[] code)
        {
            Code = code;
        }

        [StructLayout(LayoutKind.Sequential)]
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
            native.Flags = 0;
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
