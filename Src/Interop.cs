using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace VulkanCore
{
    /// <summary>
    /// Utilities for interoping with the Vulkan C API.
    /// </summary>
    public static unsafe class Interop
    {
        /// <summary>
        /// Allocates memory from the unmanaged memory of the process.
        /// </summary>
        /// <param name="byteCount">The required number of bytes in memory.</param>
        /// <returns>
        /// A pointer to the newly allocated memory. This memory must be released using the <see
        /// cref="Free(IntPtr)"/> method.
        /// </returns>
        public static IntPtr Alloc(Size byteCount)
        {
            if (byteCount == 0) return IntPtr.Zero;

            IntPtr ptr = Marshal.AllocHGlobal(byteCount);
            RaiseAlloc(ptr);
            return ptr;
        }

        /// <summary>
        /// Allocates memory from the unmanaged memory of the process.
        /// </summary>
        /// <typeparam name="T">Type to allocate for.</typeparam>
        /// <param name="count">
        /// The number of instances of <typeparamref name="T"/> to allocate for.
        /// </param>
        /// <returns>
        /// A pointer to the newly allocated memory. This memory must be released using the <see
        /// cref="Free(IntPtr)"/> method.
        /// </returns>
        public static IntPtr Alloc<T>(int count = 1) => Alloc(SizeOf<T>() * count);

        /// <summary>
        /// Resizes a block of memory previously allocated with <see cref="Alloc"/>.
        /// </summary>
        /// <param name="original">A pointer to memory allocated with <see cref="Alloc"/>.</param>
        /// <param name="size">The new size of the allocated block.</param>
        /// <returns>
        /// A pointer to the reallocated memory. This memory must be released using <see cref="Free(IntPtr)"/>.
        /// </returns>
        public static IntPtr ReAlloc(IntPtr original, Size size) =>
            Marshal.ReAllocHGlobal(original, size);

        /// <summary>
        /// Frees memory previously allocated from the unmanaged memory of the process.
        /// </summary>
        /// <param name="pointer">The handle to free memory from.</param>
        public static void Free(void* pointer) => Free(new IntPtr(pointer));

        /// <summary>
        /// Frees memory previously allocated from the unmanaged memory of the process.
        /// </summary>
        /// <param name="pointer">The handle to free memory from.</param>
        public static void Free(IntPtr pointer)
        {
            if (pointer == IntPtr.Zero) return;

            RaiseFree(pointer);
            Marshal.FreeHGlobal(pointer);
        }

        /// <summary>
        /// Frees memory previously allocated from the unmanaged memory of the process.
        /// <para>Both the handle as well as the handles pointed by the handle are freed.</para>
        /// </summary>
        /// <param name="pointers">A handle to handles to free memory from.</param>
        /// <param name="count">Number of handles to free.</param>
        public static void Free(IntPtr* pointers, int count)
        {
            if (pointers == null) return;

            for (int i = 0; i < count; i++)
                Free(pointers[i]);
            Free(pointers);
        }

        /// <summary>
        /// Converts an unmanaged function pointer to a delegate of a specified type.
        /// </summary>
        /// <typeparam name="TDelegate">The type of the delegate to return.</typeparam>
        /// <param name="pointer">The unmanaged function pointer to convert.</param>
        /// <returns>An instance of the specified delegate type.</returns>
        public static TDelegate GetDelegateForFunctionPointer<TDelegate>(IntPtr pointer)
            => Marshal.GetDelegateForFunctionPointer<TDelegate>(pointer);

        /// <summary>
        /// Converts a delegate of a specified type to a function pointer that is callable from
        /// unmanaged code.
        /// </summary>
        /// <typeparam name="TDelegate">The delegate to be passed to unmanaged code.</typeparam>
        /// <param name="delegate">The type of delegate to convert.</param>
        /// <returns>
        /// A value that can be passed to unmanaged code, which, in turn, can use it to call the
        /// underlying managed delegate.
        /// </returns>
        public static IntPtr GetFunctionPointerForDelegate<TDelegate>(TDelegate @delegate)
            => Marshal.GetFunctionPointerForDelegate(@delegate);

        /// <summary>
        /// Copies a value of type <typeparamref name="T"/> to the given location.
        /// </summary>
        /// <typeparam name="T">The type of value to copy.</typeparam>
        /// <param name="srcPointer">A pointer to the value to copy.</param>
        /// <param name="value">The location to copy to.</param>
        public static void Read<T>(IntPtr srcPointer, ref T value)
            => Unsafe.Copy(ref value, srcPointer.ToPointer());

        /// <summary>
        /// Copies values of type <typeparamref name="T"/> to the given location.
        /// </summary>
        /// <typeparam name="T">The type of values to copy.</typeparam>
        /// <param name="srcPointer">A pointer to the value to copy.</param>
        /// <param name="values">The location to copy to.</param>
        public static void Read<T>(IntPtr srcPointer, T[] values)
        {
            if (values == null || values.Length == 0) return;

            int stride = SizeOf<T>();
            long size = stride * values.Length;
            void* dstPtr = Unsafe.AsPointer(ref values[0]);
            System.Buffer.MemoryCopy(srcPointer.ToPointer(), dstPtr, size, size);
        }

        /// <summary>
        /// Copies a value of type <typeparamref name="T"/> to the given location.
        /// </summary>
        /// <typeparam name="T">The type of value to copy.</typeparam>
        /// <param name="dstPointer">The location to copy to.</param>
        /// <param name="value">A reference to the value to copy.</param>
        public static void Write<T>(IntPtr dstPointer, ref T value)
            => Unsafe.Copy(dstPointer.ToPointer(), ref value);

        /// <summary>
        /// Copies values of type <typeparamref name="T"/> to the given location.
        /// </summary>
        /// <typeparam name="T">The type of values to copy.</typeparam>
        /// <param name="dstPointer">The location to copy to.</param>
        /// <param name="values">A reference to the values to copy.</param>
        public static void Write<T>(IntPtr dstPointer, T[] values)
            where T : struct
        {
            if (values == null || values.Length == 0) return;

            int stride = SizeOf<T>();
            long size = stride * values.Length;
            void* srcPtr = Unsafe.AsPointer(ref values[0]);
            System.Buffer.MemoryCopy(srcPtr, dstPointer.ToPointer(), size, size);
        }

        /// <summary>
        /// Returns the size of an object of the given type parameter.
        /// </summary>
        /// <typeparam name="T">The type of object whose size is retrieved.</typeparam>
        /// <returns>The size of an object of type <typeparamref name="T"/>.</returns>
        public static int SizeOf<T>() => Unsafe.SizeOf<T>();

        /// <summary>
        /// Casts the gived object to the specified type.
        /// </summary>
        /// <typeparam name="T">The type which the object will be cast to.</typeparam>
        /// <param name="obj">The object to cast.</param>
        /// <returns>The original object, casted to the given type.</returns>
        public static T As<T>(object obj) where T : class => Unsafe.As<T>(obj);

        /// <summary>
        /// Utilities for interoping with strings.
        /// </summary>
        public static class String
        {
            /// <summary>
            /// Decodes specified null-terminated UTF-8 bytes into string.
            /// </summary>
            /// <param name="pointer">Pointer to decode from.</param>
            /// <returns>
            /// A string that contains the results of decoding the specified sequence of bytes.
            /// </returns>
            public static string FromPointer(byte* pointer)
            {
                if (pointer == null) return null;

                // Read until null-terminator.
                byte* walkPtr = pointer;
                while (*walkPtr != 0) walkPtr++;

                // Decode UTF-8 bytes to string.
                return Encoding.UTF8.GetString(pointer, (int)(walkPtr - pointer));
            }

            /// <summary>
            /// Decodes specified null-terminated UTF-8 bytes into string.
            /// </summary>
            /// <param name="pointer">Pointer to decode from.</param>
            /// <returns>
            /// A string that contains the results of decoding the specified sequence of bytes.
            /// </returns>
            public static string FromPointer(IntPtr pointer) => FromPointer((byte*)pointer);

            /// <summary>
            /// Encodes a string as null-terminated UTF-8 bytes and stores into specified pointer.
            /// </summary>
            /// <param name="value">The string to encode.</param>
            /// <param name="dstPointer">Pointer to store encoded bytes.</param>
            /// <param name="maxByteCount">
            /// The maximum number of bytes the string can occupy found by <see cref="GetMaxByteCount"/>.
            /// </param>
            public static void ToPointer(string value, byte* dstPointer, int maxByteCount)
            {
                if (value == null) return;

                int destBytesWritten;
                fixed (char* srcPointer = value)
                    destBytesWritten = Encoding.UTF8.GetBytes(srcPointer, value.Length, dstPointer, maxByteCount);
                dstPointer[destBytesWritten] = 0; // Null-terminator.
            }

            /// <summary>
            /// Encodes a string as null-terminated UTF-8 bytes and allocates to unmanaged memory.
            /// </summary>
            /// <param name="value">The string to encode.</param>
            /// <returns>
            /// A pointer to the newly allocated memory. This memory must be released using the <see
            /// cref="Free(IntPtr)"/> method.
            /// </returns>
            public static IntPtr AllocToPointer(string value)
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

            /// <summary>
            /// Encodes strings as null-terminated UTF-8 byte sequences and allocates sequences as
            /// well as pointers to unmanaged memory.
            /// </summary>
            /// <param name="values">Strings to encode.</param>
            /// <returns>
            /// A pointer to the newly allocated memory. This memory must be released using the <see
            /// cref="Free(IntPtr*, int)"/> method.
            /// </returns>
            public static IntPtr* AllocToPointers(string[] values)
            {
                if (values == null || values.Length == 0)
                    return null;

                // Allocate unmanaged memory for string pointers.
                var stringHandlesPtr = (IntPtr*)Alloc<IntPtr>(values.Length);

                for (var i = 0; i < values.Length; i++)
                    // Store the pointer to the string.
                    stringHandlesPtr[i] = AllocToPointer(values[i]);

                return stringHandlesPtr;
            }

            /// <summary>
            /// Calculates the maximum number of bytes produced by encoding the specified number of
            /// UTF-8 characters.
            /// </summary>
            /// <param name="value">The string to get byte count for.</param>
            /// <returns>
            /// The maximum number of bytes produced by encoding the specified number of characters.
            /// </returns>
            public static int GetMaxByteCount(string value) => value == null 
                ? 0
                : Encoding.UTF8.GetMaxByteCount(value.Length + 1); // +1 for null-terminator.
        }

        /// <summary>
        /// Utilities for interoping with structs.
        /// </summary>
        public static class Struct
        {
            /// <summary>
            /// Allocates unmanaged memory and copies the specified structure over.
            /// </summary>
            /// <typeparam name="T">Type of structure to copy.</typeparam>
            /// <param name="value">The value to copy.</param>
            /// <returns>
            /// A pointer to the newly allocated memory. This memory must be released using the <see
            /// cref="Free(IntPtr)"/> method.
            /// </returns>
            public static IntPtr AllocToPointer<T>(ref T value) where T : struct
            {
                IntPtr ptr = Alloc<T>();
                Unsafe.Copy(ptr.ToPointer(), ref value);
                return ptr;
            }

            /// <summary>
            /// Allocates unmanaged memory and copies the specified structure over.
            /// <para>If the value is <c>null</c>, returns <see cref="IntPtr.Zero"/>.</para>
            /// </summary>
            /// <typeparam name="T">Type of structure to copy.</typeparam>
            /// <param name="value">The value to copy.</param>
            /// <returns>
            /// A pointer to the newly allocated memory. This memory must be released using the <see
            /// cref="Free(IntPtr)"/> method.
            /// </returns>
            public static IntPtr AllocToPointer<T>(ref T? value) where T : struct
            {
                if (!value.HasValue) return IntPtr.Zero;

                IntPtr ptr = Alloc<T>();
                Unsafe.Write(ptr.ToPointer(), value.Value);
                return ptr;
            }

            /// <summary>
            /// Allocates unmanaged memory and copies the specified structures over.
            /// <para>If the array is <c>null</c> or empty, returns <see cref="IntPtr.Zero"/>.</para>
            /// </summary>
            /// <typeparam name="T">Type of elements to copy.</typeparam>
            /// <param name="values">The values to copy.</param>
            /// <returns>
            /// A pointer to the newly allocated memory. This memory must be released using the <see
            /// cref="Free(IntPtr)"/> method.
            /// </returns>
            public static IntPtr AllocToPointer<T>(T[] values) where T : struct
            {
                if (values == null || values.Length == 0) return IntPtr.Zero;

                int structSize = SizeOf<T>();
                int totalSize = values.Length * structSize;
                IntPtr ptr = Alloc(totalSize);

                var walk = (byte*)ptr;
                for (int i = 0; i < values.Length; i++)
                {
                    Unsafe.Copy(walk, ref values[i]);
                    walk += structSize;
                }

                return ptr;
            }
        }

        // Using actual managed thread id is not possible in .NET Standard 1.3
        // because the API is missing. If the library is upgraded to .NET Standard 2.0,
        // it should be reworked to use that instead.

        [ThreadStatic] private static Guid? _threadId;

        /// <summary>
        /// Gets a per-thread unique id to distuingish the static instance of <see cref="Interop"/>.
        /// Useful when tracking unmanaged memory.
        /// </summary>
        public static Guid ThreadId => (_threadId ?? (_threadId = Guid.NewGuid())).Value;

        /// <summary>
        /// Occurs when unmanaged memory is allocated in Debug mode.
        /// </summary>
        public static event EventHandler<(Guid, IntPtr)> OnDebugAlloc;

        /// <summary>
        /// Occurs when unmanaged memory is freed in Debug mode.
        /// </summary>
        public static event EventHandler<(Guid, IntPtr)> OnDebugFree;

        [Conditional("DEBUG")]
        private static void RaiseAlloc(IntPtr ptr) => OnDebugAlloc?.Invoke(null, (ThreadId, ptr));

        [Conditional("DEBUG")]
        private static void RaiseFree(IntPtr ptr) => OnDebugFree?.Invoke(null, (ThreadId, ptr));
    }
}
