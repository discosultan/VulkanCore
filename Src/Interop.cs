using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace VulkanCore
{
    // TODO: doc

    public static unsafe class Interop
    {
        public static void Read<T>(IntPtr dstPtr, ref T data)
        {
            Unsafe.Copy(ref data, dstPtr.ToPointer());
        }

        public static void Read<T>(IntPtr dstPtr, T[] data)
        {
            if (data == null) return;

            int size = SizeOf<T>();
            var dstBytesPtr = (byte*)dstPtr;
            for (int i = 0; i < data.Length; i++)
            {
                Unsafe.Copy(ref data[i], dstBytesPtr);
                dstBytesPtr += size;
            }
        }

        public static void Write<T>(IntPtr dstPtr, ref T data)
        {
            Unsafe.Copy(dstPtr.ToPointer(), ref data);
        }

        public static void Write<T>(IntPtr dstPtr, T[] data)
            where T : struct
        {
            if (data == null) return;

            int size = SizeOf<T>();
            var dstBytesPtr = (byte*)dstPtr;
            for (int i = 0; i < data.Length; i++)
            {
                Unsafe.Copy(dstBytesPtr, ref data[i]);
                dstBytesPtr += size;
            }
        }

        public static int GetMaxByteCount(string value)
        {
            return value == null 
                ? 0 
                : Encoding.UTF8.GetMaxByteCount(value.Length + 1); // +1 for null-terminator.
        }

        public static void StringToPtr(string value, byte* dstPtr, int maxByteCount)
        {
            if (value == null) return;

            int destBytesWritten;
            fixed (char* srcPtr = value)
                destBytesWritten = Encoding.UTF8.GetBytes(srcPtr, value.Length, dstPtr, maxByteCount);
            dstPtr[destBytesWritten] = 0; // Null-terminator.
        }

        // Unmarshals a UTF-8 null-terminated string.
        public static string PtrToString(byte* ptr)
        {
            if (ptr == null) return null;

            // Read until null-terminator.
            byte* walkPtr = ptr;
            while (*walkPtr != 0) walkPtr++;

            // Decode UTF-8 bytes to string.
            return Encoding.UTF8.GetString(ptr, (int)(walkPtr - ptr));
        }

        public static string PtrToString(IntPtr handle) => PtrToString((byte*)handle);

        // Marshal to an UTF-8 null-terminated string.
        public static IntPtr AllocStringToPtr(string value)
        {
            if (value == null) return IntPtr.Zero;
            
            // Get max number of bytes the string may need.
            int maxSize = GetMaxByteCount(value);
            // Allocate unmanaged memory.
            IntPtr managedPtr = Alloc(maxSize);
            var ptr = (byte*)managedPtr;
            // Encode to utf-8, null-terminate and write to unmanaged memory.
            int actualNumberOfBytesWritten;
            fixed (char* ch = value)
                actualNumberOfBytesWritten = Encoding.UTF8.GetBytes(ch, value.Length, ptr, maxSize);
            ptr[actualNumberOfBytesWritten] = 0;
            // Return pointer to the beginning of unmanaged memory.
            return managedPtr;
        }

        // Marshals to an array of UTF-8 null-terminated strings.
        public static IntPtr* AllocStringsToPtrs(string[] values)
        {
            if (values == null || values.Length == 0)
                return null;

            // Allocate unmanaged memory for string pointers.
            var stringHandlesPtr = (IntPtr*)Alloc<IntPtr>(values.Length);

            for (var i = 0; i < values.Length; i++)
                // Store the pointer to the string.
                stringHandlesPtr[i] = AllocStringToPtr(values[i]);

            return stringHandlesPtr;
        }

        public static IntPtr AllocStructToPtr<T>(ref T value) where T : struct
        {
            IntPtr ptr = Alloc<T>();
            Unsafe.Copy(ptr.ToPointer(), ref value);
            return ptr;
        }

        public static IntPtr AllocStructToPtr<T>(ref T? value) where T : struct
        {
            if (!value.HasValue) return IntPtr.Zero;
            
            IntPtr ptr = Alloc<T>();
            Unsafe.Write(ptr.ToPointer(), value.Value);
            return ptr;
        }

        public static IntPtr AllocStructsToPtr<T>(T[] value) where T : struct
        {
            if (value == null || value.Length == 0) return IntPtr.Zero;

            int structSize = SizeOf<T>();
            int totalSize = value.Length * structSize;
            IntPtr ptr = Alloc(totalSize);
            
            var walk = (byte*)ptr;
            for (int i = 0; i < value.Length; i++)
            {
                Unsafe.Copy(walk, ref value[i]);
                walk += structSize;
            }

            return ptr;
        }

        public static int SizeOf<T>() => Unsafe.SizeOf<T>();

        public static TDelegate GetDelegateForFunctionPointer<TDelegate>(IntPtr ptr)
            => Marshal.GetDelegateForFunctionPointer<TDelegate>(ptr);

        public static IntPtr GetFunctionPointerForDelegate<TDelegate>(TDelegate d) => Marshal.GetFunctionPointerForDelegate(d);

        public static IntPtr Alloc(int byteCount)
        {
            if (byteCount == 0) return IntPtr.Zero;

            IntPtr ptr = Marshal.AllocHGlobal(byteCount);
            RaiseAlloc(ptr);
            return ptr;
        }

        public static IntPtr Alloc(Size byteCount)
        {
            if (byteCount == 0) return IntPtr.Zero;

            IntPtr ptr = Marshal.AllocHGlobal(byteCount);
            RaiseAlloc(ptr);
            return ptr;
        }

        public static IntPtr Alloc<T>(int count = 1) => Alloc(SizeOf<T>() * count);

        public static IntPtr ReAlloc(IntPtr original, Size size) =>
            Marshal.ReAllocHGlobal(original, size);

        public static void Free(void* ptr) => Free(new IntPtr(ptr));

        public static void Free(IntPtr ptr)
        {
            if (ptr != IntPtr.Zero)
            {
                RaiseFree(ptr);
                Marshal.FreeHGlobal(ptr);
            }
        }

        public static void Free(IntPtr* ptr, int count)
        {
            if (ptr == null) return;

            for (int i = 0; i < count; i++)
                Free(ptr[i]);
            Free(ptr);
        }

        // Using actual managed thread id is not possible in .NET Standard 1.4
        // because the API is missing. If the library is upgraded to .NET Standard 2.0,
        // it should be reworked to use that instead.

        [ThreadStatic] private static Guid? _threadId;
        public static Guid ThreadId => (_threadId ?? (_threadId = Guid.NewGuid())).Value;

        [Conditional("DEBUG")]
        private static void RaiseAlloc(IntPtr ptr) => OnAlloc?.Invoke(null, (ThreadId, ptr));
        [Conditional("DEBUG")]
        private static void RaiseFree(IntPtr ptr) => OnFree?.Invoke(null, (ThreadId, ptr));

        public static event EventHandler<(Guid, IntPtr)> OnAlloc;
        public static event EventHandler<(Guid, IntPtr)> OnFree;
    }
}
