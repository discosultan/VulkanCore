namespace VulkanCore
{
    /// <summary>
    /// Specify an enumeration to track object handle types.
    /// <para>
    /// The enumeration defines values, each of which corresponds to a specific Vulkan handle type.
    /// These values can be used to associate debug information with a particular type of object
    /// through one or more extensions.
    /// </para>
    /// </summary>
    public enum ObjectType
    {
        Unknown = 0,
        Instance = 1,
        PhysicalDevice = 2,
        Device = 3,
        Queue = 4,
        Semaphore = 5,
        CommandBuffer = 6,
        Fence = 7,
        DeviceMemory = 8,
        Buffer = 9,
        Image = 10,
        Event = 11,
        QueryPool = 12,
        BufferView = 13,
        ImageView = 14,
        ShaderModule = 15,
        PipelineCache = 16,
        PipelineLayout = 17,
        RenderPass = 18,
        Pipeline = 19,
        DescriptorSetLayout = 20,
        Sampler = 21,
        DescriptorPool = 22,
        DescriptorSet = 23,
        Framebuffer = 24,
        CommandPool = 25,
        SurfaceKhr = 1000000000,
        SwapchainKhr = 1000001000,
        DisplayKhr = 1000002000,
        DisplayModeKhr = 1000002001,
        DebugReportCallbackExt = 1000011000,
        DescriptorUpdateTemplateKhr = 1000085000,
        ObjectTableNvx = 1000086000,
        IndirectCommandsLayoutNvx = 1000086001,
        SamplerYcbcrConversionKhr = 1000156000,
        ValidationCacheExt = 1000160000
    }
}
