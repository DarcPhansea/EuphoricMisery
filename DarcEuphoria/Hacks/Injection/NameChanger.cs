using System;
using System.Text;
using DarcEuphoria.Euphoric;
using DarcEuphoria.Euphoric.CSGO;

namespace DarcEuphoria.Hacks.Injection
{
    internal class NameChanger
    {
        public static byte[] Shellcode =
        {
            0x55,
            0x8B, 0xEC,
            0x83, 0xE4, 0xF8,
            0x83, 0xEC, 0x44,
            0x53,
            0x56,
            0x57,
            0xBF, 0x00, 0x00, 0x00, 0x00,
            0xBE, 0x00, 0x00, 0x00, 0x00,
            0xB8, 0x00, 0x00, 0x00, 0x00,
            0xFF, 0xE0,
            0x6E, 0x61, 0x6D, 0x65, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        };

        public static int Size = Shellcode.Length;
        public static IntPtr Address;
        public static string PREVNAME = string.Empty;

        public static void Set(string name)
        {
            if (Address == IntPtr.Zero)
            {
                Address = GlobalVariables.Allocator.Alloc(Size);

                if (Address == IntPtr.Zero)
                    return;

                Buffer.BlockCopy(BitConverter.GetBytes((int) Address + 0x1D), 0, Shellcode, 0xD, 4);
                Buffer.BlockCopy(BitConverter.GetBytes((int) Address + 0x22), 0, Shellcode, 0x12, 4);
                Buffer.BlockCopy(BitConverter.GetBytes((int) Memory.Engine.Base + Offsets.dw_SetConVar), 0, Shellcode,
                    0x17, 4);
            }

            if (!CSGOEngine.csClient.InGame) return;

            if (name == PREVNAME) return;
            PREVNAME = name;

            byte[] reset =
            {
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            };

            byte[] name_bytes;

            if (name == "\n" || name == " ")
                name_bytes = Encoding.UTF8.GetBytes(' ' + "\0");
            else
                name_bytes = Encoding.UTF8.GetBytes(name + "\0");

            Buffer.BlockCopy(reset, 0, Shellcode, 0x22, reset.Length);
            Buffer.BlockCopy(name_bytes, 0, Shellcode, 0x22, name_bytes.Length);
            WinApi.WriteProcessMemory(Memory.PHandle, Address, Shellcode, Shellcode.Length, 0);

            for (var i = 0; i < 500; i++)
                CreateThread.Execute(Address);
        }
    }
}