using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace MovementChecklist
{
    public class MemoryHelper
    {
        private Process process;

        public MemoryHelper()
        {
            process = Process.GetProcessesByName("MirrorsEdge").FirstOrDefault();
        }

        public uint GetBaseAddress(uint pointer)
        {
            try
            {
                if (process.MainModule == null)
                {
                    return 0;
                }

                return (uint)process.MainModule.BaseAddress + pointer;
            }
            catch (System.ComponentModel.Win32Exception)
            {
                return 0;
            }
        }

        public byte[] ReadMemoryBytes(uint target, uint bytes)
        {
            byte[] data = new byte[bytes];
            ReadProcessMemory(process.Handle, target, data, data.Length, IntPtr.Zero);
            return data;
        }

        public T ReadMemory<T>(uint target)
        {
            byte[] data = ReadMemoryBytes(target, (uint)Marshal.SizeOf(typeof(T)));

            if (data.Length == 0)
            {
                throw new Exception();
            }

            T type;
            GCHandle PinnedStruct = GCHandle.Alloc(data, GCHandleType.Pinned);

            try
            {
                type = (T)Marshal.PtrToStructure(PinnedStruct.AddrOfPinnedObject(), typeof(T));
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                PinnedStruct.Free();
            }

            return type;
        }

        [DllImport("kernel32.dll")]
        private static extern bool ReadProcessMemory(IntPtr hProcess, uint lpBaseAddress, byte[] lpBuffer, int nSize, IntPtr lpNumberOfBytesRead);
    }
}

namespace MovementChecklist
{
    static class MemoryUtils
    {
        public static uint OffsetCalculator(MemoryHelper targetMemory, uint BaseAddress, int[] Offsets)
        {
            uint address = BaseAddress;

            foreach (uint offset in Offsets)
            {
                address = targetMemory.ReadMemory<uint>(address) + offset;
            }

            return address;
        }
    }

    public static class ObjectType
    {
        public static int GetSize<T>()
        {
            return Marshal.SizeOf(typeof(T));
        }

        public static byte[] GetBytes<T>(T Value)
        {
            switch (typeof(T).ToString())
            {
                case "System.Single":
                    return BitConverter.GetBytes((float)Convert.ChangeType(Value, typeof(float)));
                case "System.Int32":
                    return BitConverter.GetBytes((int)Convert.ChangeType(Value, typeof(int)));
                case "System.Int64":
                    return BitConverter.GetBytes((long)Convert.ChangeType(Value, typeof(long)));
                case "System.Double":
                    return BitConverter.GetBytes((double)Convert.ChangeType(Value, typeof(double)));
                case "System.Byte":
                    return BitConverter.GetBytes((byte)Convert.ChangeType(Value, typeof(byte)));
                case "System.String":
                    return Encoding.Unicode.GetBytes((string)Convert.ChangeType(Value, typeof(string)));
                default:
                    return Array.Empty<byte>();
            }
        }
    }
}