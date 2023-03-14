using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ListSerializer
{
    /// <summary>
    /// Extensions for stream IO.
    /// </summary>
    public static class Extensions
    {
        public static void WriteMarshal<T>(this Stream stream, T value)
            where T : struct
        {
            int size = Marshal.SizeOf(value);
            IntPtr ptr = Marshal.AllocHGlobal(size);

            try
            {
                Marshal.StructureToPtr(value, ptr, false);
                byte[] managedArray = new byte[size];
                Marshal.Copy(ptr, managedArray, 0, size);
                stream.Write(managedArray, 0, size);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

        public static T ReadMarshal<T>(this Stream stream)
            where T : struct
        {
            T result = default;
            int size = Marshal.SizeOf(result);
            byte[] managedArray = new byte[size];
            IntPtr ptr = Marshal.AllocHGlobal(size);

            try
            {
                stream.Read(managedArray, 0, size);
                Marshal.Copy(managedArray, 0, ptr, size);
                result = Marshal.PtrToStructure<T>(ptr);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }

            return result;
        }
    }
}
