namespace VulkanCore
{
    /// <summary>
    /// Structure type enumerant.
    /// </summary>
    public enum StructureType
    {
        ApplicationInfo = 0,
        InstanceCreateInfo = 1,
        DeviceQueueCreateInfo = 2,
        DeviceCreateInfo = 3,
        SubmitInfo = 4,
        MemoryAllocateInfo = 5,
        MappedMemoryRange = 6,
        BindSparseInfo = 7,
        FenceCreateInfo = 8,
        SemaphoreCreateInfo = 9,
        EventCreateInfo = 10,
        QueryPoolCreateInfo = 11,
        BufferCreateInfo = 12,
        BufferViewCreateInfo = 13,
        ImageCreateInfo = 14,
        ImageViewCreateInfo = 15,
        ShaderModuleCreateInfo = 16,
        PipelineCacheCreateInfo = 17,
        PipelineShaderStageCreateInfo = 18,
        PipelineVertexInputStateCreateInfo = 19,
        PipelineInputAssemblyStateCreateInfo = 20,
        PipelineTessellationStateCreateInfo = 21,
        PipelineViewportStateCreateInfo = 22,
        PipelineRasterizationStateCreateInfo = 23,
        PipelineMultisampleStateCreateInfo = 24,
        PipelineDepthStencilStateCreateInfo = 25,
        PipelineColorBlendStateCreateInfo = 26,
        PipelineDynamicStateCreateInfo = 27,
        GraphicsPipelineCreateInfo = 28,
        ComputePipelineCreateInfo = 29,
        PipelineLayoutCreateInfo = 30,
        SamplerCreateInfo = 31,
        DescriptorSetLayoutCreateInfo = 32,
        DescriptorPoolCreateInfo = 33,
        DescriptorSetAllocateInfo = 34,
        WriteDescriptorSet = 35,
        CopyDescriptorSet = 36,
        FramebufferCreateInfo = 37,
        RenderPassCreateInfo = 38,
        CommandPoolCreateInfo = 39,
        CommandBufferAllocateInfo = 40,
        CommandBufferInheritanceInfo = 41,
        CommandBufferBeginInfo = 42,
        RenderPassBeginInfo = 43,
        BufferMemoryBarrier = 44,
        ImageMemoryBarrier = 45,
        MemoryBarrier = 46,
        /// <summary>
        /// Reserved for internal use by the loader, layers, and ICDs.
        /// </summary>
        LoaderInstanceCreateInfo = 47,
        /// <summary>
        /// Reserved for internal use by the loader, layers, and ICDs.
        /// </summary>
        LoaderDeviceCreateInfo = 48,
        SwapchainCreateInfoKhr = 1000001000,
        PresentInfoKhr = 1000001001,
        DisplayModeCreateInfoKhr = 1000002000,
        DisplaySurfaceCreateInfoKhr = 1000002001,
        DisplayPresentInfoKhr = 1000003000,
        XlibSurfaceCreateInfoKhr = 1000004000,
        XcbSurfaceCreateInfoKhr = 1000005000,
        WaylandSurfaceCreateInfoKhr = 1000006000,
        MirSurfaceCreateInfoKhr = 1000007000,
        AndroidSurfaceCreateInfoKhr = 1000008000,
        Win32SurfaceCreateInfoKhr = 1000009000,
        DebugReportCallbackCreateInfoExt = 1000011000,
        PipelineRasterizationStateRasterizationOrderAmd = 1000018000,
        DebugMarkerObjectNameInfoExt = 1000022000,
        DebugMarkerObjectTagInfoExt = 1000022001,
        DebugMarkerMarkerInfoExt = 1000022002,
        DedicatedAllocationImageCreateInfoNV = 1000026000,
        DedicatedAllocationBufferCreateInfoNV = 1000026001,
        DedicatedAllocationMemoryAllocateInfoNV = 1000026002,
        TextureLodGatherFormatPropertiesAmd = 1000041000,
        RenderPassMultiviewCreateInfoKhx = 1000053000,
        PhysicalDeviceMultiviewFeaturesKhx = 1000053001,
        PhysicalDeviceMultiviewPropertiesKhx = 1000053002,
        ExternalMemoryImageCreateInfoNV = 1000056000,
        ExportMemoryAllocateInfoNV = 1000056001,
        ImportMemoryWin32HandleInfoNV = 1000057000,
        ExportMemoryWin32HandleInfoNV = 1000057001,
        Win32KeyedMutexAcquireReleaseInfoNV = 1000058000,
        PhysicalDeviceFeatures2Khr = 1000059000,
        PhysicalDeviceProperties2Khr = 1000059001,
        FormatProperties2Khr = 1000059002,
        ImageFormatProperties2Khr = 1000059003,
        PhysicalDeviceImageFormatInfo2Khr = 1000059004,
        QueueFamilyProperties2Khr = 1000059005,
        PhysicalDeviceMemoryProperties2Khr = 1000059006,
        SparseImageFormatProperties2Khr = 1000059007,
        PhysicalDeviceSparseImageFormatInfo2Khr = 1000059008,
        MemoryAllocateFlagsInfoKhx = 1000060000,
        BindBufferMemoryInfoKhx = 1000060001,
        BindImageMemoryInfoKhx = 1000060002,
        DeviceGroupRenderPassBeginInfoKhx = 1000060003,
        DeviceGroupCommandBufferBeginInfoKhx = 1000060004,
        DeviceGroupSubmitInfoKhx = 1000060005,
        DeviceGroupBindSparseInfoKhx = 1000060006,
        AcquireNextImageInfoKhx = 1000060010,
        ValidationFlagsExt = 1000061000,
        VISurfaceCreateInfoNN = 1000062000,
        PhysicalDeviceGroupPropertiesKhx = 1000070000,
        DeviceGroupDeviceCreateInfoKhx = 1000070001,
        PhysicalDeviceExternalImageFormatInfoKhr = 1000071000,
        ExternalImageFormatPropertiesKhr = 1000071001,
        PhysicalDeviceExternalBufferInfoKhr = 1000071002,
        ExternalBufferPropertiesKhr = 1000071003,
        PhysicalDeviceIdPropertiesKhr = 1000071004,
        ExternalMemoryBufferCreateInfoKhr = 1000072000,
        ExternalMemoryImageCreateInfoKhr = 1000072001,
        ExportMemoryAllocateInfoKhr = 1000072002,
        ImportMemoryWin32HandleInfoKhr = 1000073000,
        ExportMemoryWin32HandleInfoKhr = 1000073001,
        MemoryWin32HandlePropertiesKhr = 1000073002,
        MemoryGetWin32HandleInfoKhr = 1000073003,
        ImportMemoryFdInfoKhr = 1000074000,
        MemoryFdPropertiesKhr = 1000074001,
        MemoryGetFdInfoKhr = 1000074002,
        Win32KeyedMutexAcquireReleaseInfoKhr = 1000075000,
        PhysicalDeviceExternalSemaphoreInfoKhr = 1000076000,
        ExternalSemaphorePropertiesKhr = 1000076001,
        ExportSemaphoreCreateInfoKhr = 1000077000,
        ImportSemaphoreWin32HandleInfoKhr = 1000078000,
        ExportSemaphoreWin32HandleInfoKhr = 1000078001,
        D3D12FenceSubmitInfoKhr = 1000078002,
        SemaphoreGetWin32HandleInfoKhr = 1000078003,
        ImportSemaphoreFdInfoKhr = 1000079000,
        SemaphoreGetFdInfoKhr = 1000079001,
        PhysicalDevicePushDescriptorPropertiesKhr = 1000080000,
        PhysicalDevice16BitStorageFeaturesKhr = 1000083000,
        PresentRegionsKhr = 1000084000,
        DescriptorUpdateTemplateCreateInfoKhr = 1000085000,
        ObjectTableCreateInfoNvx = 1000086000,
        IndirectCommandsLayoutCreateInfoNvx = 1000086001,
        CmdProcessCommandsInfoNvx = 1000086002,
        CmdReserveSpaceForCommandsInfoNvx = 1000086003,
        DeviceGeneratedCommandsLimitsNvx = 1000086004,
        DeviceGeneratedCommandsFeaturesNvx = 1000086005,
        PipelineViewportWScalingStateCreateInfoNV = 1000087000,
        SurfaceCapabilities2Ext = 1000090000,
        DisplayPowerInfoExt = 1000091000,
        DeviceEventInfoExt = 1000091001,
        DisplayEventInfoExt = 1000091002,
        SwapchainCounterCreateInfoExt = 1000091003,
        PresentTimesInfoGoogle = 1000092000,
        PhysicalDeviceMultiviewPerViewAttributesPropertiesNvx = 1000097000,
        PipelineViewportSwizzleStateCreateInfoNV = 1000098000,
        PhysicalDeviceDiscardRectanglePropertiesExt = 1000099000,
        PipelineDiscardRectangleStateCreateInfoExt = 1000099001,
        HdrMetadataExt = 1000105000,
        SharedPresentSurfaceCapabilitiesKhr = 1000111000,
        PhysicalDeviceExternalFenceInfoKhr = 1000112000,
        ExternalFencePropertiesKhr = 1000112001,
        ExportFenceCreateInfoKhr = 1000113000,
        ImportFenceWin32HandleInfoKhr = 1000114000,
        ExportFenceWin32HandleInfoKhr = 1000114001,
        FenceGetWin32HandleInfoKhr = 1000114002,
        ImportFenceFdInfoKhr = 1000115000,
        FenceGetFdInfoKhr = 1000115001,
        PhysicalDeviceSurfaceInfo2Khr = 1000119000,
        SurfaceCapabilities2Khr = 1000119001,
        SurfaceFormat2Khr = 1000119002,
        PhysicalDeviceVariablePointerFeaturesKhr = 1000120000,
        IOSSurfaceCreateInfoMvk = 1000122000,
        MacOSSurfaceCreateInfoMvk = 1000123000,
        MemoryDedicatedRequirementsKhr = 1000127000,
        MemoryDedicatedAllocateInfoKhr = 1000127001,
        PhysicalDeviceSamplerFilterMinmaxPropertiesExt = 1000130000,
        SamplerReductionModeCreateInfoExt = 1000130001,
        SampleLocationsInfoExt = 1000143000,
        RenderPassSampleLocationsBeginInfoExt = 1000143001,
        PipelineSampleLocationsStateCreateInfoExt = 1000143002,
        PhysicalDeviceSampleLocationsPropertiesExt = 1000143003,
        MultisamplePropertiesExt = 1000143004,
        BufferMemoryRequirementsInfo2Khr = 1000146000,
        ImageMemoryRequirementsInfo2Khr = 1000146001,
        ImageSparseMemoryRequirementsInfo2Khr = 1000146002,
        MemoryRequirements2Khr = 1000146003,
        SparseImageMemoryRequirements2Khr = 1000146004,
        PhysicalDeviceBlendOperationAdvancedFeaturesExt = 1000148000,
        PhysicalDeviceBlendOperationAdvancedPropertiesExt = 1000148001,
        PipelineColorBlendAdvancedStateCreateInfoExt = 1000148002,
        PipelineCoverageToColorStateCreateInfoNV = 1000149000,
        PipelineCoverageModulationStateCreateInfoNV = 1000152000,
        ValidationCacheCreateInfoExt = 1000160000,
        ShaderModuleValidationCacheCreateInfoExt = 1000160001
    }
}
