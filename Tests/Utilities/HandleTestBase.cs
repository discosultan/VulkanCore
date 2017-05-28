using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace VulkanCore.Tests.Utilities
{
    public abstract class HandleTestBase : IClassFixture<DefaultHandles>, IDisposable
    {
        // Since we are tracking memory allocations and allocations are notified
        // at a global level, we match the notification against the thread id this
        // test class was created on to only record allocations from this thread.
        private readonly Guid _threadId = Interop.ThreadId;
        private readonly HashSet<IntPtr> _allocs = new HashSet<IntPtr>();
        private readonly DefaultHandles _defaults;

        protected HandleTestBase(DefaultHandles defaults, ITestOutputHelper output)
        {
            _defaults = defaults;

            Output = output;

            // Subscribe to track memory alloc/free ops.
            Interop.OnDebugAlloc += OnAlloc;
            Interop.OnDebugFree += OnFree;
        }

        protected ITestOutputHelper Output { get; }
        protected Instance Instance => _defaults.Instance;
        protected PhysicalDevice PhysicalDevice => _defaults.PhysicalDevice;
        protected Device Device => _defaults.Device;
        protected Queue GraphicsQueue => _defaults.GraphicsQueue;
        protected ExtensionProperties[] AvailableInstanceExtensions => _defaults.AvailableInstanceExtensions;
        protected ExtensionProperties[] AvailableDeviceExtensions => _defaults.AvailableDeviceExtensions;
        protected PhysicalDeviceFeatures PhysicalDeviceFeatures => _defaults.PhysicalDeviceFeatures;
        protected PhysicalDeviceMemoryProperties PhysicalDeviceMemoryProperties => _defaults.PhysicalDeviceMemoryProperties;

        protected byte[] ReadAllBytes(string assetName)
        {
            string baseDirectory = AppContext.BaseDirectory;
            return File.ReadAllBytes(Path.Combine(baseDirectory, Path.Combine("Content", assetName)));
        }

        public virtual void Dispose()
        {
            // Unsubscribe from tracking memory.
            Interop.OnDebugAlloc -= OnAlloc;
            Interop.OnDebugFree -= OnFree;

            // Fail the test if any allocation is not cleared up.
            if (_allocs.Count > 0)
            {
                string allocPtrs = string.Join(", ",
                    _allocs.Select(ptr => ptr.ToInt64().ToString("X")));
                Assert.True(false, $"There are {_allocs.Count} unfreed unmanaged allocations: {allocPtrs}.");
            }
        }

        // Ignores alignment!
        protected AllocationCallbacks? CustomAllocator => new AllocationCallbacks(
            alloc: (userData, size, alignment, scope) => Interop.Alloc(size),
            realloc: (userData, original, size, alignment, scope) => Interop.ReAlloc(original, size),
            free: (userData, memory) => Interop.Free(memory),
            internalAlloc: (userData, size, type, scope) => { },
            internalFree: (userData, size, type, scope) => { });

        private void OnAlloc(object sender, (Guid threadId, IntPtr ptr) val)
        {
            if (val.threadId == _threadId)
            {
                // Fail the test if duplicate alloc to same address.
                bool success = _allocs.Add(val.ptr);
                if (!success)
                    Assert.True(false, $"Duplicate alloc at {val.ptr}.");
            }
        }

        private void OnFree(object sender, (Guid threadId, IntPtr ptr) val)
        {
            if (val.threadId == _threadId)
            {
                // Fail the test if duplicate freeing of same address.
                bool success = _allocs.Remove(val.ptr);
                if (!success)
                    Assert.True(false, $"Duplicate free at {val.ptr}.");
            }
        }
    }
}
