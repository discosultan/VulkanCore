namespace VulkanCore.Khr
{
    /// <summary>
    /// Provides Khronos specific extension methods for the <see cref="Instance"/> class.
    /// </summary>
    public static unsafe class InstanceExtensions
    {
        /// <summary>
        /// Create a <see cref="SurfaceKhr"/> object for an Android native window.
        /// </summary>
        /// <param name="instance">The <see cref="Instance"/> to associate with the surface.</param>
        /// <param name="createInfo">
        /// The structure containing the parameters affecting the creation of the surface object.
        /// </param>
        /// <param name="allocator">
        /// The allocator used for host memory allocated for the surface object.
        /// </param>
        /// <returns>The resulting surface object handle.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static SurfaceKhr CreateAndroidSurfaceKhr(this Instance instance,
            AndroidSurfaceCreateInfoKhr createInfo, AllocationCallbacks? allocator = null)
        {
            return new SurfaceKhr(instance, &createInfo, ref allocator);
        }

        /// <summary>
        /// Create a <see cref="SurfaceKhr"/> object for a Mir window.
        /// </summary>
        /// <param name="instance">The <see cref="Instance"/> to associate with the surface.</param>
        /// <param name="createInfo">
        /// The structure containing the parameters affecting the creation of the surface object.
        /// </param>
        /// <param name="allocator">
        /// The allocator used for host memory allocated for the surface object.
        /// </param>
        /// <returns>The resulting surface object handle.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static SurfaceKhr CreateMirSurfaceKhr(this Instance instance,
            MirSurfaceCreateInfoKhr createInfo, AllocationCallbacks? allocator = null)
        {
            return new SurfaceKhr(instance, &createInfo, ref allocator);
        }

        /// <summary>
        /// Create a <see cref="SurfaceKhr"/> object for a Wayland window.
        /// </summary>
        /// <param name="instance">The <see cref="Instance"/> to associate with the surface.</param>
        /// <param name="createInfo">
        /// The structure containing the parameters affecting the creation of the surface object.
        /// </param>
        /// <param name="allocator">
        /// The allocator used for host memory allocated for the surface object.
        /// </param>
        /// <returns>The resulting surface object handle.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static SurfaceKhr CreateWaylandSurfaceKhr(this Instance instance,
            WaylandSurfaceCreateInfoKhr createInfo, AllocationCallbacks? allocator = null)
        {
            return new SurfaceKhr(instance, &createInfo, ref allocator);
        }

        /// <summary>
        /// Create a <see cref="SurfaceKhr"/> object for a Win32 window.
        /// </summary>
        /// <param name="instance">The <see cref="Instance"/> to associate with the surface.</param>
        /// <param name="createInfo">
        /// The structure containing the parameters affecting the creation of the surface object.
        /// </param>
        /// <param name="allocator">
        /// The allocator used for host memory allocated for the surface object.
        /// </param>
        /// <returns>The resulting surface object handle.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static SurfaceKhr CreateWin32SurfaceKhr(this Instance instance,
            Win32SurfaceCreateInfoKhr createInfo, AllocationCallbacks? allocator = null)
        {
            return new SurfaceKhr(instance, &createInfo, ref allocator);
        }

        /// <summary>
        /// Create a <see cref="SurfaceKhr"/> object for an X11 window, using the Xlib client-side library.
        /// </summary>
        /// <param name="instance">The <see cref="Instance"/> to associate with the surface.</param>
        /// <param name="createInfo">
        /// The structure containing the parameters affecting the creation of the surface object.
        /// </param>
        /// <param name="allocator">
        /// The allocator used for host memory allocated for the surface object.
        /// </param>
        /// <returns>The resulting surface object handle.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static SurfaceKhr CreateXlibSurfaceKhr(this Instance instance,
            XlibSurfaceCreateInfoKhr createInfo, AllocationCallbacks? allocator = null)
        {
            return new SurfaceKhr(instance, &createInfo, ref allocator);
        }

        /// <summary>
        /// Create a <see cref="SurfaceKhr"/> structure representing a display plane and mode.
        /// </summary>
        /// <param name="instance">The <see cref="Instance"/> to associate with the surface.</param>
        /// <param name="createInfo">
        /// The structure specifying which mode, plane, and other parameters to use.
        /// </param>
        /// <param name="allocator">
        /// The allocator used for host memory allocated for the surface object.
        /// </param>
        /// <returns>The resulting surface object handle.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static SurfaceKhr CreateDisplayPlaneSurfaceKhr(this Instance instance,
            DisplaySurfaceCreateInfoKhr createInfo, AllocationCallbacks? allocator = null)
        {
            return new SurfaceKhr(instance, &createInfo, ref allocator);
        }

        /// <summary>
        /// Create a <see cref="SurfaceKhr"/> object for a X11 window, using the XCB client-side library.
        /// </summary>
        /// <param name="instance">The <see cref="Instance"/> to associate with the surface.</param>
        /// <param name="createInfo">
        /// The structure containing the parameters affecting the creation of the surface object.
        /// </param>
        /// <param name="allocator">
        /// The allocator used for host memory allocated for the surface object.
        /// </param>
        /// <returns>The resulting surface object handle.</returns>
        /// <exception cref="VulkanException">Vulkan returns an error code.</exception>
        public static SurfaceKhr CreateXcbSurfaceKhr(this Instance instance,
            XcbSurfaceCreateInfoKhr createInfo, AllocationCallbacks? allocator = null)
        {
            return new SurfaceKhr(instance, &createInfo, ref allocator);
        }
    }
}
