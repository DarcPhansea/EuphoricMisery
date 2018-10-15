using System;
using DarcEuphoria.Euphoric;
using DarcEuphoria.Euphoric.CSGO;

namespace DarcEuphoria.Hacks.Injection
{
    public static class RankRevealer
    {
        public static IntPtr address = IntPtr.Zero;

        public static void Start()
        {
            if (address == IntPtr.Zero)
            {
                address = GlobalVariables.Allocator.Alloc(12);

                if (address == IntPtr.Zero)
                    return;
            }

            var Thread = WinApi.CreateRemoteThread(
                Memory.PHandle,
                (IntPtr) null,
                IntPtr.Zero,
                Memory.Client.Base + Offsets.dw_RevealRankFn,
                address,
                0,
                (IntPtr) null);

            WinApi.WaitForSingleObject(Thread, 0xFFFFFFFF);

            WinApi.CloseHandle(Thread);
        }
    }
}