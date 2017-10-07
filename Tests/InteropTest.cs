using System;
using Xunit;

namespace VulkanCore.Tests
{
    public unsafe class InteropTest
    {
        [Fact]
        public void AllocZeroBytes()
        {
            IntPtr handle = Interop.Alloc(0);
            Assert.Equal(IntPtr.Zero, handle);
        }

        [Fact]
        public void AllocStringToPtr()
        {
            const string value = "hi"; // 0x68 + 0x69
            IntPtr handle = IntPtr.Zero;
            try
            {
                handle = Interop.String.AllocToPointer(value);
                var ptr = (byte*)handle;

                Assert.Equal(0x68, ptr[0]); // 'h'
                Assert.Equal(0x69, ptr[1]); // 'i'
                Assert.Equal(0x00, ptr[2]); // '\0' - null-terminator
            }
            finally
            {
                Interop.Free(handle);
            }
        }

        [Fact]
        public void AllocEmptyStringToPtr()
        {
            const string value = "";
            IntPtr handle = IntPtr.Zero;
            try
            {
                handle = Interop.String.AllocToPointer(value);
                var ptr = (byte*)handle;

                Assert.Equal(0x00, ptr[0]); // '\0' - null-terminator
            }
            finally
            {
                Interop.Free(handle);
            }
        }

        [Fact]
        public void AllocNullStringToPtr()
        {
            IntPtr handle = Interop.String.AllocToPointer(null);
            Assert.Equal(IntPtr.Zero, handle);
        }

        [Fact]
        public void StringToPtr()
        {
            const string str = "hi"; // 0x68 + 0x69
            int count = Interop.String.GetMaxByteCount(str);
            var bytes = stackalloc byte[count];
            Interop.String.ToPointer(str, bytes, count);
            Assert.Equal(0x68, bytes[0]);
            Assert.Equal(0x69, bytes[1]);
            Assert.Equal(0, bytes[2]);
        }

        [Fact]
        public void EmptyStringToPtr()
        {
            const string str = "";
            int count = Interop.String.GetMaxByteCount(str);
            var bytes = stackalloc byte[count];
            Interop.String.ToPointer(str, bytes, count);
            Assert.Equal(0, bytes[0]);
        }

        [Fact]
        public void NullStringToPtr()
        {
            const string str = null;
            int count = Interop.String.GetMaxByteCount(str);
            var bytes = stackalloc byte[count];
            Interop.String.ToPointer(str, bytes, count);
            Assert.Equal(0, count);
        }

        [Fact]
        public void NullPtrToString()
        {
            string value = Interop.String.FromPointer(null);
            Assert.Null(value);
        }

        [Fact]
        public void PtrToEmptyString()
        {
            const string value = "";
            IntPtr handle = IntPtr.Zero;
            try
            {
                handle = Interop.String.AllocToPointer(value);
                string unmarshalledValue = Interop.String.FromPointer(handle);

                Assert.Equal(value, unmarshalledValue);
            }
            finally
            {
                Interop.Free(handle);
            }
        }

        [Fact]
        public void PtrToString()
        {
            const string value = "hi";
            IntPtr handle = IntPtr.Zero;
            try
            {
                handle = Interop.String.AllocToPointer(value);
                string unmarshalledValue = Interop.String.FromPointer(handle);

                Assert.Equal(value, unmarshalledValue);
            }
            finally
            {
                Interop.Free(handle);
            }
        }

        [Fact]
        public void AllocNullStringsToPtrs()
        {
            IntPtr* ptr = Interop.String.AllocToPointers(null);
            Assert.Equal(IntPtr.Zero, (IntPtr)ptr);
        }

        [Fact]
        public void AllocEmptyStringsToPtrs()
        {
            IntPtr* ptr = Interop.String.AllocToPointers(new string[0]);
            Assert.Equal(IntPtr.Zero, (IntPtr)ptr);
        }

        [Fact]
        public void AllocStringsToPtrs()
        {
            string[] values = { "hi", "hello" };
            IntPtr* ptr = null;
            try
            {
                ptr = Interop.String.AllocToPointers(values);

                IntPtr firstHandle = ptr[0];
                IntPtr secondHandle = ptr[1];
                Assert.Equal(values[0], Interop.String.FromPointer(firstHandle));
                Assert.Equal(values[1], Interop.String.FromPointer(secondHandle));
            }
            finally
            {
                Interop.Free(ptr, values.Length);
            }
        }

        [Fact]
        public void AllocNullStructToPtr()
        {
            int? value = null;
            IntPtr ptr = Interop.Struct.AllocToPointer(ref value);
            Assert.Equal(IntPtr.Zero, ptr);
        }

        [Fact]
        public void AllocStructToPtr()
        {
            IntPtr ptr = IntPtr.Zero;
            try
            {
                int value = 1;
                ptr = Interop.Struct.AllocToPointer(ref value);
                Assert.Equal(1, *(int*)ptr);
            }
            finally
            {
                Interop.Free(ptr);
            }
        }

        [Fact]
        public void AllocNullableStructToPtr()
        {
            IntPtr ptr = IntPtr.Zero;
            try
            {
                int? value = 1;
                ptr = Interop.Struct.AllocToPointer(ref value);
                Assert.Equal(1, *(int*)ptr);
            }
            finally
            {
                Interop.Free(ptr);
            }
        }

        [Fact]
        public void AllocNullStructsToPtr()
        {
            IntPtr ptr = Interop.Struct.AllocToPointer<int>(null);
            Assert.Equal(IntPtr.Zero, ptr);
        }

        [Fact]
        public void AllocEmptyStructsToPtr()
        {
            IntPtr ptr = Interop.Struct.AllocToPointer(new int[0]);
            Assert.Equal(IntPtr.Zero, ptr);
        }

        [Fact]
        public void Alloc32BitStructsToPtr()
        {
            int[] structs = { 1, 2, 3, 4 };
            IntPtr ptr = IntPtr.Zero;
            try
            {
                ptr = Interop.Struct.AllocToPointer(structs);
                var intPtr = (int*)ptr;
                for (int i = 0; i < structs.Length; i++)
                    Assert.Equal(structs[i], intPtr[i]);
            }
            finally
            {
                Interop.Free(ptr);
            }
        }

        [Fact]
        public void Alloc64BitStructsToPtr()
        {
            long[] structs = { 1, 2, 3, 4 };
            IntPtr ptr = IntPtr.Zero;
            try
            {
                ptr = Interop.Struct.AllocToPointer(structs);
                var intPtr = (long*)ptr;
                for (int i = 0; i < structs.Length; i++)
                    Assert.Equal(structs[i], intPtr[i]);
            }
            finally
            {
                Interop.Free(ptr);
            }
        }

        [Fact]
        public void ReAlloc()
        {
            IntPtr ptr = IntPtr.Zero;
            try
            {
                ptr = Interop.Alloc(32);
                ptr = Interop.ReAlloc(ptr, 64);
                Assert.NotEqual(IntPtr.Zero, ptr);
            }
            finally
            {
                Interop.Free(ptr);
            }
        }

        [Fact]
        public void ReadValue()
        {
            long src = 0L;
            long dst = 1L;
            Interop.Read(new IntPtr(&dst), ref src);
            Assert.Equal(1L, src);
        }

        [Fact]
        public void WriteValue()
        {
            long src = 1L;
            long dst = 0L;
            Interop.Write(new IntPtr(&dst), ref src);
            Assert.Equal(1L, dst);
        }

        [Fact]
        public void ReadArray()
        {
            var dst = new long[2];
            long[] src = { 1L, 2L };
            fixed (long* srcPtr = src)
                Interop.Read(new IntPtr(srcPtr), dst);
            Assert.Equal(1L, dst[0]);
            Assert.Equal(2L, dst[1]);
        }

        [Fact]
        public void WriteArray()
        {
            long[] src = { 1L, 2L };
            var dst = new long[2];
            fixed (long* dstPtr = dst)
                Interop.Write(new IntPtr(dstPtr), src);
            Assert.Equal(1L, dst[0]);
            Assert.Equal(2L, dst[1]);
        }

        [Fact]
        public void ReadEmptyArray()
        {
            var dst = new long[0];
            var src = new long[0];
            fixed (long* srcPointer = src)
                Interop.Read(new IntPtr(srcPointer), dst);
        }

        [Fact]
        public void WriteEmptyArray()
        {
            var src = new long[0];
            var dst = new long[0];
            fixed (long* dstPtr = dst)
                Interop.Write(new IntPtr(dstPtr), src);
        }
    }
}
