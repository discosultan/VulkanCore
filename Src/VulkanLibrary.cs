using System;
using System.Runtime.InteropServices;

namespace VulkanCore
{
    internal class VulkanLibrary
    {
        private static readonly VulkanLibrary _finalizer = new VulkanLibrary();
        private static IntPtr _handle;

        private VulkanLibrary()
        {
            string name = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? "vulkan-1.dll"
                : "libvulkan.so.1";
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
                handle = Kernel32.GetProcAddress(_handle, procName);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                handle = LibDL.dlsym(_handle, procName);
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
                handle = Kernel32.LoadLibrary(fileName);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                handle = LibDL.dlopen(fileName, LibDL.RtldNow);
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
                returnCode = Kernel32.FreeLibrary(module);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                returnCode = LibDL.dlclose(module);
            }
            else
            {
                throw new NotImplementedException();
            }
            return returnCode;
        }

        private static class Kernel32
        {
            [DllImport("kernel32")]
            public static extern IntPtr LoadLibrary(string fileName);

            [DllImport("kernel32")]
            public static extern IntPtr GetProcAddress(IntPtr module, string procName);

            [DllImport("kernel32")]
            public static extern int FreeLibrary(IntPtr module);
        }

        private static class LibDL
        {
            [DllImport("libdl.so")]
            public static extern IntPtr dlopen(string fileName, int flags);

            [DllImport("libdl.so")]
            public static extern IntPtr dlsym(IntPtr handle, string name);

            [DllImport("libdl.so")]
            public static extern int dlclose(IntPtr handle);

            [DllImport("libdl.so")]
            public static extern string dlerror();

            public const int RtldNow = 0x002;
        }
    }
}
