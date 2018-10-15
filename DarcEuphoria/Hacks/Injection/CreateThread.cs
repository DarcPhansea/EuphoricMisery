using System;
using DarcEuphoria.Euphoric;

namespace DarcEuphoria.Hacks.Injection
{
    internal class CreateThread
    {
        public static void Create(IntPtr address, byte[] shellcode)
        {
            WinApi.WriteProcessMemory(Memory.PHandle, address, shellcode, shellcode.Length, 0);
            var _Thread = WinApi.CreateRemoteThread(Memory.PHandle, (IntPtr) null, (IntPtr) null, address,
                (IntPtr) null, 0, (IntPtr) null);
            WinApi.WaitForSingleObject(_Thread, 0xFFFFFFFF);
            WinApi.CloseHandle(_Thread);
        }

        public static void Execute(IntPtr address)
        {
            try
            {
                var _Thread = WinApi.CreateRemoteThread(Memory.PHandle, (IntPtr) null, (IntPtr) null, address,
                    (IntPtr) null, 0, (IntPtr) null);
                WinApi.WaitForSingleObject(_Thread, 0xFFFFFFFF);
                WinApi.CloseHandle(_Thread);
            }
            catch
            {
            }
        }
    }
}