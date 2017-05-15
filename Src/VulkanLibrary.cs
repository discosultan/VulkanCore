﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace VulkanCore
{
    internal static class VulkanLibrary
    {
        private static readonly IntPtr _handle;

        // Note that the loaded Vulkan module is never freed by this assembly because we have no way
        // to guarantee that it's freed after the client application has finished using this
        // assembly. This becomes a problem, for example, when the client application wants to
        // dispose any types of this assmebly in a finalizer. In that case, we don't know in which
        // order the finalizers are run.

        // If for some reason, the client application wishes to unload Vulkan module when the process
        // is still running, it may do so resorting to its platforms load and free library methods
        // similar to what is done in this class. In order to successfully unload the module, it will
        // need to get a handle to the module using LoadLibrary/dlopen. Note that this will increase
        // the CLR ref count to 2 which means that the handle must be freed TWICE using FreeLibrary/dlclose.

        static VulkanLibrary()
        {
            _handle = GetVulkanLibraryNameCandidates()
                .Select(LoadLibrary)
                .FirstOrDefault(handle => handle != IntPtr.Zero);
            if (_handle == IntPtr.Zero)
                throw new NotImplementedException("Vulkan native library was not found.");
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
		
	    public static IEnumerable<string> SearchExecutablePath(string searchPattern)
	    {
		    var values = Environment.GetEnvironmentVariable("PATH");
		    foreach (var path in values.Split(Path.PathSeparator))
		    {
			    if (string.IsNullOrWhiteSpace(path))
				{
				    continue;
			    }

				string expandedPath = Environment.ExpandEnvironmentVariables(path);
			    IEnumerable<string> searchResults;
			    try
				{
					searchResults = Directory.EnumerateFiles(expandedPath, searchPattern).ToArray();
				}
				catch ( FileNotFoundException )
				{
					continue;
				}
			    catch ( DirectoryNotFoundException )
			    {
				    continue;
			    }
				foreach (var file in searchResults)
			    {
				    yield return file;
			    }
		    }
	    }

        private static IEnumerable<string> GetVulkanLibraryNameCandidates()
        {
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				var searchResults = SearchExecutablePath("vulkan-*.dll")
					.Distinct(StringComparer.OrdinalIgnoreCase)
					.OrderByDescending(Path.GetFileName);
				foreach ( var result in searchResults )
				{
					yield return result;
				}
				yield return "vulkan-1-1-0-46-0.dll";
				yield return "vulkan-1-1-0-42-1.dll";
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

        [DllImport("libdl.so", EntryPoint = "dlopen")]
        private static extern IntPtr LibDLLoadLibrary(string fileName, int flags);

        [DllImport("libdl.so", EntryPoint = "dlsym")]
        private static extern IntPtr LibDLGetProcAddress(IntPtr handle, string name);

        private const int LibDLRtldNow = 2;
    }
}
