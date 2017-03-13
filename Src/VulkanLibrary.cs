using System;
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
            string name = GetVulkanLibraryName();
            _handle = LoadLibrary(name);
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

        private static string GetVulkanLibraryName()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return "vulkan-1.dll";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                // Ubuntu OS description starts with "linux".
                if (RuntimeInformation.OSDescription.StartsWith("linux", StringComparison.OrdinalIgnoreCase))
                {
                    return "libvulkan.so.1";
                }
                // Android OS description starts with "unix".
                else if (RuntimeInformation.OSDescription.StartsWith("unix", StringComparison.OrdinalIgnoreCase))
                {
                    return "vulkan.so";
                }
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

        private const int LibDLRtldNow = 0x002;
    }
}
