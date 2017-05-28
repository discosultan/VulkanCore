using System;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using static VulkanCore.Constant;

namespace VulkanCore
{
    /// <summary>
    /// Opaque handle to an instance object.
    /// <para>
    /// There is no global state in Vulkan and all per-application state is stored in an <see
    /// cref="Instance"/> object. Creating an <see cref="Instance"/> object initializes the Vulkan
    /// library and allows the application to pass information about itself to the implementation.
    /// </para>
    /// </summary>
    public unsafe class Instance : DisposableHandle<IntPtr>
    {
        private readonly ConcurrentDictionary<string, IntPtr> _procAddrCache
            = new ConcurrentDictionary<string, IntPtr>(StringComparer.Ordinal);

        private readonly ConcurrentDictionary<Type, object> _procCache
            = new ConcurrentDictionary<Type, object>();

        /// <summary>
        /// Create a new Vulkan instance.
        /// </summary>
        /// <param name="createInfo">
        /// An instance of <see cref="InstanceCreateInfo"/> controlling creation of the instance.
        /// </param>
        /// <param name="allocator">Controls host memory allocation.</param>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public Instance(InstanceCreateInfo createInfo = default(InstanceCreateInfo),
            AllocationCallbacks? allocator = null)
        {
            Allocator = allocator;

            createInfo.ToNative(out InstanceCreateInfo.Native nativeCreateInfo);

            IntPtr handle;
            Result result = vkCreateInstance(&nativeCreateInfo, NativeAllocator, &handle);
            nativeCreateInfo.Free();

            VulkanException.ThrowForInvalidResult(result);
            Handle = handle;
        }

        /// <summary>
        /// Enumerates the physical devices accessible to a Vulkan instance.
        /// </summary>
        /// <returns>A list of accessible physical devices. The result is never null.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public PhysicalDevice[] EnumeratePhysicalDevices()
        {
            int count;
            Result result = vkEnumeratePhysicalDevices(this, &count, null);
            VulkanException.ThrowForInvalidResult(result);

            var devicesHandle = stackalloc IntPtr[count];
            result = vkEnumeratePhysicalDevices(this, &count, devicesHandle);

            var devices = new PhysicalDevice[count];
            for (int i = 0; i < count; i++)
                devices[i] = new PhysicalDevice(devicesHandle[i], this);

            VulkanException.ThrowForInvalidResult(result);
            return devices;
        }

        /// <summary>
        /// Return a function handle for a command or <see cref="IntPtr.Zero"/> if not found.
        /// <para>
        /// Vulkan commands are not necessarily exposed statically on a platform. Function pointers
        /// for all Vulkan commands can be obtained with this method.
        /// </para>
        /// </summary>
        /// <param name="name">The name of the command to obtain.</param>
        /// <returns>Function handle for a command or <see cref="IntPtr.Zero"/> if not found.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <c>null</c>.</exception>
        public IntPtr GetProcAddr(string name)
        {
            if (!_procAddrCache.TryGetValue(name, out IntPtr addr))
            {
                int byteCount = Interop.String.GetMaxByteCount(name);
                var dstPtr = stackalloc byte[byteCount];
                Interop.String.ToPointer(name, dstPtr, byteCount);
                addr = vkGetInstanceProcAddr(Handle, dstPtr);
                _procAddrCache.TryAdd(name, addr);
            }
            return addr;
        }

        /// <summary>
        /// Return a function delegate for a command or <c>null</c> if not found.
        /// <para>
        /// Vulkan commands are not necessarily exposed statically on a platform. Function pointers
        /// for all Vulkan commands can be obtained with this method.
        /// </para>
        /// </summary>
        /// <param name="name">The name of the command to obtain.</param>
        /// <returns>Function delegate for a command or <c>null</c> if not found.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <c>null</c>.</exception>
        public TDelegate GetProc<TDelegate>(string name) where TDelegate : class
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            if (_procCache.TryGetValue(typeof(TDelegate), out object cachedProc))
                return (TDelegate)cachedProc;

            IntPtr ptr = GetProcAddr(name);
            TDelegate proc = ptr != IntPtr.Zero
                ? Interop.GetDelegateForFunctionPointer<TDelegate>(ptr)
                : null;
            _procCache.TryAdd(typeof(TDelegate), proc);
            return proc;
        }

        /// <summary>
        /// Returns global extension properties.
        /// </summary>
        /// <param name="layerName">
        /// Is either <c>null</c> or a unicode string naming the layer to retrieve extensions from.
        /// When parameter is <c>null</c>, only extensions provided by the Vulkan implementation or
        /// by implicitly enabled layers are returned.
        /// </param>
        /// <returns>Properties of available extensions for layer.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static ExtensionProperties[] EnumerateExtensionProperties(string layerName = null)
        {
            int dstLayerNameByteCount = Interop.String.GetMaxByteCount(layerName);
            var dstLayerNamePtr = stackalloc byte[dstLayerNameByteCount];
            Interop.String.ToPointer(layerName, dstLayerNamePtr, dstLayerNameByteCount);

            int count;
            Result result = vkEnumerateInstanceExtensionProperties(dstLayerNamePtr, &count, null);
            VulkanException.ThrowForInvalidResult(result);

            var propertiesPtr = stackalloc ExtensionProperties.Native[count];
            result = vkEnumerateInstanceExtensionProperties(dstLayerNamePtr, &count, propertiesPtr);
            VulkanException.ThrowForInvalidResult(result);

            var properties = new ExtensionProperties[count];
            for (int i = 0; i < count; i++)
                ExtensionProperties.FromNative(ref propertiesPtr[i], out properties[i]);
            return properties;
        }

        /// <summary>
        /// Returns global layer properties.
        /// </summary>
        /// <returns>Properties of available layers.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static LayerProperties[] EnumerateLayerProperties()
        {
            int count;
            Result result = vkEnumerateInstanceLayerProperties(&count, null);
            VulkanException.ThrowForInvalidResult(result);

            var nativePropertiesPtr = stackalloc LayerProperties.Native[count];
            result = vkEnumerateInstanceLayerProperties(&count, nativePropertiesPtr);
            VulkanException.ThrowForInvalidResult(result);

            var properties = new LayerProperties[count];
            for (int i = 0; i < count; i++)
                properties[i] = LayerProperties.FromNative(ref nativePropertiesPtr[i]);
            return properties;
        }

        /// <summary>
        /// Destroy an instance of Vulkan.
        /// </summary>
        public override void Dispose()
        {
            if (!Disposed) vkDestroyInstance(this, NativeAllocator);
            base.Dispose();
        }

        private delegate Result vkCreateInstanceDelegate(InstanceCreateInfo.Native* createInfo, AllocationCallbacks.Native* allocator, IntPtr* instance);
        private static readonly vkCreateInstanceDelegate vkCreateInstance = VulkanLibrary.GetStaticProc<vkCreateInstanceDelegate>(nameof(vkCreateInstance));

        private delegate void vkDestroyInstanceDelegate(IntPtr instance, AllocationCallbacks.Native* allocator);
        private static readonly vkDestroyInstanceDelegate vkDestroyInstance = VulkanLibrary.GetStaticProc<vkDestroyInstanceDelegate>(nameof(vkDestroyInstance));

        private delegate Result vkEnumeratePhysicalDevicesDelegate(IntPtr instance, int* physicalDeviceCount, IntPtr* physicalDevices);
        private static readonly vkEnumeratePhysicalDevicesDelegate vkEnumeratePhysicalDevices = VulkanLibrary.GetStaticProc<vkEnumeratePhysicalDevicesDelegate>(nameof(vkEnumeratePhysicalDevices));

        private delegate IntPtr vkGetInstanceProcAddrDelegate(IntPtr instance, byte* name);
        private static readonly vkGetInstanceProcAddrDelegate vkGetInstanceProcAddr = VulkanLibrary.GetStaticProc<vkGetInstanceProcAddrDelegate>(nameof(vkGetInstanceProcAddr));

        private delegate Result vkEnumerateInstanceLayerPropertiesDelegate(int* propertyCount, LayerProperties.Native* properties);
        private static readonly vkEnumerateInstanceLayerPropertiesDelegate vkEnumerateInstanceLayerProperties = VulkanLibrary.GetStaticProc<vkEnumerateInstanceLayerPropertiesDelegate>(nameof(vkEnumerateInstanceLayerProperties));

        private delegate Result vkEnumerateInstanceExtensionPropertiesDelegate(byte* layerName, int* propertyCount, ExtensionProperties.Native* properties);
        private static readonly vkEnumerateInstanceExtensionPropertiesDelegate vkEnumerateInstanceExtensionProperties = VulkanLibrary.GetStaticProc<vkEnumerateInstanceExtensionPropertiesDelegate>(nameof(vkEnumerateInstanceExtensionProperties));
    }

    /// <summary>
    /// Structure specifying parameters of a newly created instance.
    /// </summary>
    public unsafe struct InstanceCreateInfo
    {
        /// <summary>
        /// Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// The information that helps implementations recognize behavior inherent to classes of applications.
        /// </summary>
        public ApplicationInfo? ApplicationInfo;
        /// <summary>
        /// Unicode strings containing the names of layers to enable for the created instance.
        /// </summary>
        public string[] EnabledLayerNames;
        /// <summary>
        /// Unicode strings containing the names of extensions to enable.
        /// </summary>
        public string[] EnabledExtensionNames;

        /// <summary>
        /// Initializes a new instance of the <see cref="InstanceCreateInfo"/> structure.
        /// </summary>
        /// <param name="appInfo">
        /// The information that helps implementations recognize behavior inherent to classes of applications.
        /// </param>
        /// <param name="enabledLayerNames">
        /// Unicode strings containing the names of layers to enable for the created instance.
        /// </param>
        /// <param name="enabledExtensionNames">
        /// Unicode strings containing the names of extensions to enable.
        /// </param>
        /// <param name="next">Is <see cref="IntPtr.Zero"/> or a pointer to an extension-specific structure.</param>
        public InstanceCreateInfo(
            ApplicationInfo? appInfo = null,
            string[] enabledLayerNames = null,
            string[] enabledExtensionNames = null,
            IntPtr next = default(IntPtr))
        {
            Next = next;
            ApplicationInfo = appInfo;
            EnabledLayerNames = enabledLayerNames;
            EnabledExtensionNames = enabledExtensionNames;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct Native
        {
            public StructureType Type;
            public IntPtr Next;
            public InstanceCreateFlags Flags;
            public ApplicationInfo.Native* ApplicationInfo;
            public int EnabledLayerCount;
            public IntPtr* EnabledLayerNames;
            public int EnabledExtensionCount;
            public IntPtr* EnabledExtensionNames;

            public void Free()
            {
                if (ApplicationInfo != null)
                {
                    ApplicationInfo->Free();
                    Interop.Free(ApplicationInfo);
                }
                Interop.Free(EnabledLayerNames, EnabledLayerCount);
                Interop.Free(EnabledExtensionNames, EnabledExtensionCount);
            }
        }

        internal void ToNative(out Native val)
        {
            val.Type = StructureType.InstanceCreateInfo;
            val.Next = IntPtr.Zero;
            val.Flags = 0;
            if (ApplicationInfo.HasValue)
            {
                var appInfoNative = (ApplicationInfo.Native*)Interop.Alloc<ApplicationInfo.Native>();
                ApplicationInfo.Value.ToNative(appInfoNative);
                val.ApplicationInfo = appInfoNative;
            }
            else
            {
                val.ApplicationInfo = null;
            }
            val.EnabledLayerCount = EnabledLayerNames?.Length ?? 0;
            val.EnabledLayerNames = Interop.String.AllocToPointers(EnabledLayerNames);
            val.EnabledExtensionCount = EnabledExtensionNames?.Length ?? 0;
            val.EnabledExtensionNames = Interop.String.AllocToPointers(EnabledExtensionNames);
        }
    }

    // Is reserved for future use.
    [Flags]
    internal enum InstanceCreateFlags
    {
        None = 0
    }

    /// <summary>
    /// Structure specifying application info.
    /// </summary>
    public unsafe struct ApplicationInfo
    {
        /// <summary>
        /// The unicode string containing the name of the application.
        /// </summary>
        public string ApplicationName;
        /// <summary>
        /// The unsigned integer variable containing the developer-supplied version
        /// number of the application.
        /// </summary>
        public int ApplicationVersion;
        /// <summary>
        /// The unicode string containing the name of the engine (if any) used to create the application.
        /// </summary>
        public string EngineName;
        /// <summary>
        /// The unsigned integer variable containing the developer-supplied version
        /// number of the engine used to create the application.
        /// </summary>
        public int EngineVersion;
        /// <summary>
        /// The version of the Vulkan API against which the application expects to run. If <see
        /// cref="ApiVersion"/> is <see cref="Version.Zero"/> the implementation must ignore it,
        /// otherwise if the implementation does not support the requested <see cref="ApiVersion"/>
        /// it must return <see cref="Result.ErrorIncompatibleDriver"/>. The patch version number
        /// specified in <see cref="ApiVersion"/> is ignored when creating an instance object. Only
        /// the major and minor versions of the instance must match those requested in <see cref="ApiVersion"/>.
        /// </summary>
        public Version ApiVersion;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationInfo"/> structure.
        /// </summary>
        /// <param name="applicationName">The unicode string containing the name of the application.</param>
        /// <param name="applicationVersion">
        /// The unsigned integer variable containing the developer-supplied version number of the application.
        /// </param>
        /// <param name="engineName">
        /// The unicode string containing the name of the engine (if any) used to create the application.
        /// </param>
        /// <param name="engineVersion">
        /// The unsigned integer variable containing the developer-supplied version number of the
        /// engine used to create the application.
        /// </param>
        /// <param name="apiVersion">
        /// The version of the Vulkan API against which the application expects to run. If <see
        /// cref="ApiVersion"/> is <see cref="Version.Zero"/> the implementation must ignore it,
        /// otherwise if the implementation does not support the requested <see cref="ApiVersion"/>
        /// it must return <see cref="Result.ErrorIncompatibleDriver"/>. The patch version number
        /// specified in <see cref="ApiVersion"/> is ignored when creating an instance object. Only
        /// the major and minor versions of the instance must match those requested in <see cref="ApiVersion"/>.
        /// </param>
        public ApplicationInfo(
            string applicationName = null, int applicationVersion = 0,
            string engineName = null, int engineVersion = 0,
            Version apiVersion = default(Version))
        {
            ApplicationName = applicationName;
            ApplicationVersion = applicationVersion;
            EngineName = engineName;
            EngineVersion = engineVersion;
            ApiVersion = apiVersion;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct Native
        {
            public StructureType Type;
            public IntPtr Next;
            public IntPtr ApplicationName;
            public int ApplicationVersion;
            public IntPtr EngineName;
            public int EngineVersion;
            public Version ApiVersion;

            public void Free()
            {
                Interop.Free(ApplicationName);
                Interop.Free(EngineName);
            }
        }

        internal void ToNative(Native* val)
        {
            val->Type = StructureType.ApplicationInfo;
            val->Next = IntPtr.Zero;
            val->ApplicationName = Interop.String.AllocToPointer(ApplicationName);
            val->ApplicationVersion = ApplicationVersion;
            val->EngineName = Interop.String.AllocToPointer(EngineName);
            val->EngineVersion = EngineVersion;
            val->ApiVersion = ApiVersion;
        }
    }

    /// <summary>
    /// Structure specifying a extension properties.
    /// </summary>
    public unsafe struct ExtensionProperties
    {
        /// <summary>
        /// The name of the extension.
        /// </summary>
        public string ExtensionName;
        /// <summary>
        /// The version of this extension. It is an integer, incremented with backward compatible changes.
        /// </summary>
        public int SpecVersion;

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() => $"{ExtensionName} v{SpecVersion}";

        [StructLayout(LayoutKind.Sequential)]
        internal struct Native
        {
            public fixed byte ExtensionName[MaxExtensionNameSize];
            public int SpecVersion;
        }

        internal static void FromNative(ref Native native, out ExtensionProperties managed)
        {
            fixed (byte* extensionNamePtr = native.ExtensionName)
            {
                managed = new ExtensionProperties
                {
                    ExtensionName = Interop.String.FromPointer(extensionNamePtr),
                    SpecVersion = native.SpecVersion
                };
            }
        }
    }

    /// <summary>
    /// Structure specifying layer properties.
    /// </summary>
    public unsafe struct LayerProperties
    {
        /// <summary>
        /// A unicode string specifying the name of the layer. Use this name in the <see
        /// cref="InstanceCreateInfo.EnabledLayerNames"/> array to enable this layer for an instance.
        /// </summary>
        public string LayerName;
        /// <summary>
        /// The Vulkan version the layer was written to.
        /// </summary>
        public Version SpecVersion;
        /// <summary>
        /// The version of this layer. It is an integer, increasing with backward compatible changes.
        /// </summary>
        public int ImplementationVersion;
        /// <summary>
        /// A unicode string providing additional details that can be used by the application to
        /// identify the layer.
        /// </summary>
        public string Description;

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() => $"{LayerName} v{SpecVersion}";

        [StructLayout(LayoutKind.Sequential)]
        internal struct Native
        {
            public fixed byte LayerName[MaxExtensionNameSize];
            public Version SpecVersion;
            public int ImplementationVersion;
            public fixed byte Description[MaxDescriptionSize];
        }

        internal static LayerProperties FromNative(ref Native native)
        {
            fixed (byte* layerNamePtr = native.LayerName)
            fixed (byte* descriptionPtr = native.Description)
            {
                return new LayerProperties
                {
                    LayerName = Interop.String.FromPointer(layerNamePtr),
                    SpecVersion = native.SpecVersion,
                    ImplementationVersion = native.ImplementationVersion,
                    Description = Interop.String.FromPointer(descriptionPtr)
                };
            }
        }
    }
}
