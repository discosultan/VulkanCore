namespace VulkanCore
{
    /// <summary>
    /// Vulkan command return codes.
    /// </summary>
    public enum Result
    {
        /// <summary>
        /// Command successfully completed.
        /// </summary>
        Success = 0,
        /// <summary>
        /// A fence or query has not yet completed.
        /// </summary>
        NotReady = 1,
        /// <summary>
        /// A wait operation has not completed in the specified time.
        /// </summary>
        Timeout = 2,
        /// <summary>
        /// An event is signaled.
        /// </summary>
        EventSet = 3,
        /// <summary>
        /// An event is unsignaled.
        /// </summary>
        EventReset = 4,
        /// <summary>
        /// A return array was too small for the result.
        /// </summary>
        Incomplete = 5,
        /// <summary>
        /// A host memory allocation has failed.
        /// </summary>
        ErrorOutOfHostMemory = -1,
        /// <summary>
        /// A device memory allocation has failed.
        /// </summary>
        ErrorOutOfDeviceMemory = -2,
        /// <summary>
        /// Initialization of an object could not be completed for implementation-specific reasons.
        /// </summary>
        ErrorInitializationFailed = -3,
        /// <summary>
        /// The logical or physical device has been lost.
        /// </summary>
        ErrorDeviceLost = -4,
        /// <summary>
        /// Mapping of a memory object has failed.
        /// </summary>
        ErrorMemoryMapFailed = -5,
        /// <summary>
        /// A requested layer is not present or could not be loaded.
        /// </summary>
        ErrorLayerNotPresent = -6,
        /// <summary>
        /// A requested extension is not supported.
        /// </summary>
        ErrorExtensionNotPresent = -7,
        /// <summary>
        /// A requested feature is not supported.
        /// </summary>
        ErrorFeatureNotPresent = -8,
        /// <summary>
        /// The requested version of Vulkan is not supported by the driver or is otherwise
        /// incompatible for implementation-specific reasons.
        /// </summary>
        ErrorIncompatibleDriver = -9,
        /// <summary>
        /// Too many objects of the type have already been created.
        /// </summary>
        ErrorTooManyObjects = -10,
        /// <summary>
        /// A requested format is not supported on this device.
        /// </summary>
        ErrorFormatNotSupported = -11,
        /// <summary>
        /// A pool allocation has failed due to fragmentation of the pool's memory. This must only be
        /// returned if no attempt to allocate host or device memory was made to accomodate the new allocation.
        /// </summary>
        ErrorFragmentedPool = -12,
        /// <summary>
        /// The surface becomes no longer available.
        /// </summary>
        ErrorSurfaceLostKhr = -1000000000,
        /// <summary>
        /// The requested window is already in use by Vulkan or another API in a manner which
        /// prevents it from being used again.
        /// </summary>
        ErrorNativeWindowInUseKhr = -1000000001,
        /// <summary>
        /// A swapchain no longer matches the surface properties exactly, but can still be used to
        /// present to the surface successfully.
        /// </summary>
        SuboptimalKhr = 1000001003,
        /// <summary>
        /// A surface has changed in such a way that it is no longer compatible with the swapchain,
        /// and further presentation requests using the swapchain will fail. Applications must query
        /// the new surface properties and recreate their swapchain if they wish to continue
        /// presenting to the surface.
        /// </summary>
        ErrorOutOfDateKhr = -1000001004,
        /// <summary>
        /// A surface has changed in such a way that it is no longer compatible with the swapchain,
        /// and further presentation requests using the swapchain will fail. Applications must query
        /// the new surface properties and recreate their swapchain if they wish to continue
        /// presenting to the surface.
        /// </summary>
        ErrorIncompatibleDisplayKhr = -1000003001,
        /// <summary>
        /// The application returned <c>true</c> from its callback and the Vulkan call being aborted
        /// returned a <see cref="Result"/>.
        /// </summary>
        ErrorValidationFailedExt = -1000011001,
        /// <summary>
        /// One or more shaders failed to compile or link. More details are reported back to the
        /// application via <see cref="Ext.DebugReportCallbackExt"/> if enabled.
        /// </summary>
        ErrorInvalidShaderNV = -1000012000,
        /// <summary>
        /// A pool memory allocation has failed. This must only be returned if no attempt to allocate
        /// host or device memory was made to accomodate the new allocation. If the failure was
        /// definitely due to fragmentation of the pool, <see cref="ErrorFragmentedPool"/> should be
        /// returned instead.
        /// </summary>
        ErrorOutOfPoolMemoryKhr = -1000069000,
        /// <summary>
        /// An external handle is not a valid handle of the specified type.
        /// </summary>
        ErrorInvalidExternalHandleKhx = 1000072003,
        ErrorNotPermittedExt = 1000174001
    }
}
