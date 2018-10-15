using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
using DarcEuphoria.Euphoric.Enums;
using DarcEuphoria.Euphoric.Structs;

namespace DarcEuphoria.Euphoric
{
    public static class WinApi
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetActiveWindow(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetFocus(IntPtr hWnd);

        [DllImport("User32.dll", EntryPoint = @"SendMessage", CharSet = CharSet.Auto)]
        public static extern int SendMessageRefRect(IntPtr hWnd, uint msg, int wParam, ref RECT rect);

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool AllocConsole();

        [DllImport("user32.dll", SetLastError = false)]
        public static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll")]
        public static extern bool GetClientRect(IntPtr hWnd, out RECT RECT);

        [DllImport("user32.dll")]
        public static extern bool ClientToScreen(IntPtr hWnd, out Point point);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT RECT);

        [DllImport("user32.dll")]
        public static extern bool BlockInput(bool fBlockIt);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        public static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);

        [DllImport("dwmapi.dll")]
        public static extern void DwmExtendFrameIntoClientArea(IntPtr hWnd, ref Margins pMargins);

        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int GetAsyncKeyState(int vKey);

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(ProcessAccessFlags dwDesiredAccess,
            [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, int dwProcessId);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindow(IntPtr hWnd);

        [DllImport("kernel32.dll")]
        internal static extern bool CloseHandle(IntPtr hProcess);

        [DllImport("Kernel32.dll")]
        internal static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer,
            uint nSize, out uint lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        internal static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer,
            IntPtr nSize, out uint lpNumberOfBytesWritten);

        [DllImport("kernel32.dll")]
        public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] buffer, int size,
            int lpNumberOfBytesWritten);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        internal static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress, IntPtr dwSize,
            FreeType dwFreeType);

        [DllImport("kernel32.dll")]
        internal static extern uint WaitForSingleObject(IntPtr hProcess, uint dwMilliseconds);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, IntPtr dwStackSize,
            IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        internal static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, IntPtr dwSize,
            uint flAllocationType, uint flProtect);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        public static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress, int dwSize, uint dwFreeType);

        [DllImport("gdi32.dll")]
        public static extern unsafe bool SetDeviceGammaRamp(int hdc, void* ramp);

        internal delegate int ThreadProc(IntPtr param);
    }
}