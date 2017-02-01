using System;
using Xunit;

namespace VulkanCore.Tests
{
    public unsafe class InteropTest
    {
        [Fact]
        public void AllocStringToPtr_ReturnsNullHandleForNull()
        {
            IntPtr handle = Interop.AllocStringToPtr(null);
            Assert.Equal(IntPtr.Zero, handle);
        }

        [Fact]
        public void AllocStringToPtr_ReturnsValidHandleForEmptyString()
        {
            const string value = "";
            IntPtr handle = IntPtr.Zero;
            try
            {
                handle = Interop.AllocStringToPtr(value);
                var ptr = (byte*)handle;
                    
                Assert.Equal(0x00, ptr[0]); // '\0' - null-terminator
            }
            finally
            {
                Interop.Free(handle);
            }
        }

        [Fact]
        public void AllocStringToPtr_ReturnsValidHandleForString()
        {
            const string value = "hi"; // 0x68 + 0x69
            IntPtr handle = IntPtr.Zero;
            try
            {
                handle = Interop.AllocStringToPtr(value);
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
        public void PtrToString_ReturnsNullForNullHandle()
        {
            string value = Interop.PtrToString(null);
            Assert.Equal(null, value);
        }

        [Fact]
        public void PtrToString_ReturnsEmptyStringForValidHandle()
        {
            const string value = "";
            IntPtr handle = IntPtr.Zero;
            try
            {
                handle = Interop.AllocStringToPtr(value);
                string unmarshalledValue = Interop.PtrToString(handle);

                Assert.Equal(value, unmarshalledValue);
            }
            finally
            {
                Interop.Free(handle);
            }
        }

        [Fact]
        public void PtrToString_ReturnsStringForValidHandle()
        {
            const string value = "hi";
            IntPtr handle = IntPtr.Zero;
            try
            {
                handle = Interop.AllocStringToPtr(value);
                string unmarshalledValue = Interop.PtrToString(handle);

                Assert.Equal(value, unmarshalledValue);
            }
            finally
            {
                Interop.Free(handle);
            }
        }

        [Fact]
        public void AllocStringsToPtrs_ReturnsNullForNullStrings()
        {
            IntPtr* ptr = Interop.AllocStringsToPtrs(null);
            Assert.Equal(IntPtr.Zero, (IntPtr)ptr);
        }

        [Fact]
        public void AllocStringsToPtrs_ReturnsNullForEmptyStringsArray()
        {
            IntPtr* ptr = Interop.AllocStringsToPtrs(new string[0]);
            Assert.Equal(IntPtr.Zero, (IntPtr)ptr);
        }

        [Fact]
        public void AllocStringsToPtrs_ReturnsValidPtrForStrings()
        {
            string[] values = { "hi", "hello" };
            IntPtr* ptr = null;
            try
            {
                ptr = Interop.AllocStringsToPtrs(values);

                IntPtr firstHandle = ptr[0];
                IntPtr secondHandle = ptr[1];
                Assert.Equal(values[0], Interop.PtrToString(firstHandle));
                Assert.Equal(values[1], Interop.PtrToString(secondHandle));
            }
            finally
            {
                Interop.Free(ptr, values.Length);
            }
        }

        [Fact]
        public void AllocNullStructToPtr_ReturnsNull()
        {
            int? value = null;
            IntPtr ptr = Interop.AllocStructToPtr(ref value);
            Assert.Equal(IntPtr.Zero, ptr);
        }

        [Fact]
        public void AllocStructToPtr_Succeeds()
        {
            IntPtr ptr = IntPtr.Zero;
            try
            {
                int value = 1;
                ptr = Interop.AllocStructToPtr(ref value);
                Assert.Equal(1, *(int*)ptr);
            }
            finally
            {
                Interop.Free(ptr);
            }            
        }

        [Fact]
        public void AllocNullableStructToPtr_Succeeds()
        {
            IntPtr ptr = IntPtr.Zero;
            try
            {
                int? value = 1;
                ptr = Interop.AllocStructToPtr(ref value);
                Assert.Equal(1, *(int*)ptr);
            }
            finally
            {
                Interop.Free(ptr);
            }
        }

        [Fact]
        public void AllocStructsToPtr_ReturnsNullForNullStructs()
        {
            IntPtr ptr = Interop.AllocStructsToPtr<int>(null);
            Assert.Equal(IntPtr.Zero, ptr);
        }

        [Fact]
        public void AllocStructsToPtr_ReturnsNullForEmptyStructs()
        {
            IntPtr ptr = Interop.AllocStructsToPtr(new int[0]);
            Assert.Equal(IntPtr.Zero, ptr);
        }

        [Fact]
        public void Alloc32BitStructsToPtr_Succeeds()
        {
            int[] structs = { 1, 2, 3, 4 };
            IntPtr ptr = IntPtr.Zero;
            try
            {
                ptr = Interop.AllocStructsToPtr(structs);
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
        public void Alloc64BitStructsToPtr_Succeeds()
        {
            long[] structs = { 1, 2, 3, 4 };
            IntPtr ptr = IntPtr.Zero;
            try
            {
                ptr = Interop.AllocStructsToPtr(structs);
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
        public void ReAlloc_Succeeds()
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
    }
}
