using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace VulkanCore
{
    internal class VulkanLibrary
    {
        private static readonly VulkanLibrary _finalizer;
        private static readonly IntPtr _handle;

        private VulkanLibrary() { }

        static VulkanLibrary()
        {
            _finalizer = new VulkanLibrary();
            _handle = GetVulkanLibraryNameCandidates()
                .Select(name => LoadLibrary(name))
                .FirstOrDefault(handle => handle != IntPtr.Zero);
            if (_handle == IntPtr.Zero)
                throw new NotImplementedException("Vulkan native library was not found.");
        }

        ~VulkanLibrary()
        {
            FreeLibrary(_handle);
        }

        public static TDelegate GetProc<TDelegate>(string procName)
            where TDelegate : class
        {
            IntPtr handle;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                handle = Kernel32GetProcAddress(_handle, procName);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                handle = LibDLGetProcAddress(_handle, procName);
            }
            else
            {
                throw new NotImplementedException();
            }
            return handle == IntPtr.Zero
                ? null
                : Interop.GetDelegateForFunctionPointer<TDelegate>(handle);
        }

        private static IntPtr LoadLibrary(string fileName)
        {
            IntPtr handle;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                handle = Kernel32LoadLibrary(fileName);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                handle = LibDLLoadLibrary(fileName, LibDLRtldNow);
            }
            else
            {
                throw new NotImplementedException();
            }
            return handle;
        }

        private static int FreeLibrary(IntPtr module)
        {
            int returnCode;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                returnCode = Kernel32FreeLibrary(module);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                returnCode = LibDLFreeLibrary(module);
            }
            else
            {
                throw new NotImplementedException();
            }
            return returnCode;
        }

        private static IEnumerable<string> GetVulkanLibraryNameCandidates()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                yield return "vulkan-1.dll";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                yield return "libvulkan.so.1"; // Known to be present on Ubuntu 16.
                yield return "libvulkan.so";   // Known to be present on Android 7.
            }
            throw new NotImplementedException();
        }

        [DllImport("kernel32", EntryPoint = "LoadLibrary")]
        private static extern IntPtr Kernel32LoadLibrary(string fileName);

        [DllImport("kernel32", EntryPoint = "GetProcAddress")]
        private static extern IntPtr Kernel32GetProcAddress(IntPtr module, string procName);

        [DllImport("kernel32", EntryPoint = "FreeLibrary")]
        private static extern int Kernel32FreeLibrary(IntPtr module);

        [DllImport("libdl.so", EntryPoint = "dlopen")]
        private static extern IntPtr LibDLLoadLibrary(string fileName, int flags);

        [DllImport("libdl.so", EntryPoint = "dlsym")]
        private static extern IntPtr LibDLGetProcAddress(IntPtr handle, string name);

        [DllImport("libdl.so", EntryPoint = "dlclose")]
        private static extern int LibDLFreeLibrary(IntPtr handle);

        private const int LibDLRtldNow = 2;
    }
}
