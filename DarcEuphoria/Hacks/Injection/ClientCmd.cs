using System;
using System.Text;
using DarcEuphoria.Euphoric;
using DarcEuphoria.Euphoric.CSGO;

namespace DarcEuphoria.Hacks.Injection
{
    public static class ClientCmd
    {
        public static int Size = 256;
        public static IntPtr Address = IntPtr.Zero;

        public static void Exec(string szCmd)
        {
            if (Address == IntPtr.Zero)
            {
                Address = GlobalVariables.Allocator.Alloc(Size);
                if (Address == IntPtr.Zero)
                    return;
            }


            if (szCmd.Length > 255)
                szCmd = szCmd.Substring(0, 255);

            var szCmd_bytes = Encoding.UTF8.GetBytes(szCmd + "\0");

            WinApi.WriteProcessMemory(Memory.PHandle, Address, szCmd_bytes, szCmd_bytes.Length, 0);
            var Thread = WinApi.CreateRemoteThread(Memory.PHandle, (IntPtr) null, IntPtr.Zero,
                Memory.Engine.Base + Offsets.dw_clientCmd, Address, 0, (IntPtr) null);
            WinApi.CloseHandle(Thread);
            WinApi.WaitForSingleObject(Thread, 0xFFFFFFFF);
        }
    }
}