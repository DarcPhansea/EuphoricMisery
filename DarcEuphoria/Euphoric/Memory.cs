using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using DarcEuphoria.Euphoric.CSGO;
using DarcEuphoria.Euphoric.ProcessScanner;
using DarcEuphoria.Euphoric.Structs;
using DarcEuphoria.Hacks.Injection;

namespace DarcEuphoria.Euphoric
{
    public static class Memory
    {
        public static IntPtr PHandle = IntPtr.Zero;

        public static Module Engine = new Module(IntPtr.Zero, 0);
        public static Module Client = new Module(IntPtr.Zero, 0);

        public static uint nBytesRead = uint.MinValue;

        public static string SteamPath =>
            Process.GetProcessesByName("csgo")[0].MainModule.FileName.Substring(0,
                Process.GetProcessesByName("csgo")[0].MainModule.FileName.Length - "csgo.exe".Length);

        public static bool IsValid
        {
            get
            {
                try
                {
                    var Proc = Process.GetProcessesByName("csgo");

                    if (Proc.Length > 0)
                    {
                        GlobalVariables.CSGO = Process.GetProcessesByName("csgo")[0];
                        PHandle = GlobalVariables.CSGO.Handle;

                        InitModules();

                        return true;
                    }

                    return false;
                }
                catch
                {
                    return false;
                }
            }
        }

        public static void InitModules()
        {
            foreach (ProcessModule module in GlobalVariables.CSGO.Modules)
                if (module.ModuleName == "client_panorama.dll")
                    Client = new Module(module.BaseAddress, module.ModuleMemorySize);
                else if (module.ModuleName == "engine.dll")
                    Engine = new Module(module.BaseAddress, module.ModuleMemorySize);
            NetvarManager.Init();
            Offsets.Init();
            Netvars.Init();
        }

        public static void CloseCheat()
        {
            GlobalVariables.IsActive = false;

            while (!GlobalVariables.SHUTDOWN)
                Thread.Sleep(1);

            CSGOEngine.csClient.SendPackets = true;
            ClanChanger.Set(string.Empty);
            CSGOEngine.LocalPlayer.ThirdPerson = false;
            ClientCmd.Exec("bind mouse1 +attack; bind mouse2 +attack2");
            CSGOEngine.LocalPlayer.DefaultFOV.Value = 90;
            CSGOEngine.LocalPlayer.FOV.Value = 90;
            CSGOEngine.csClient.ForceFullUpdate();
            CloseHandle();
            Environment.Exit(Environment.ExitCode);
        }

        public static void CloseHandle()
        {
            try
            {
                //if (WinApi.IsWindow(PHandle))
                //   WinApi.CloseHandle(PHandle);
            }
            catch
            {
            }
        }

        public static T GetStructure<T>(byte[] bytes)
        {
            var handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            var structure = (T) Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();
            return structure;
        }

        public static byte[] GetStructBytes<T>(T str)
        {
            var size = Marshal<T>.Size;
            var arr = new byte[size];
            var ptr = Marshal.AllocHGlobal((int) size);
            Marshal.StructureToPtr(str, ptr, true);
            Marshal.Copy(ptr, arr, 0, (int) size);
            Marshal.FreeHGlobal(ptr);
            return arr;
        }

        public static T Read<T>(int address)
        {
            var length = Marshal<T>.Size;

            if (typeof(T) == typeof(bool))
                length = 1;

            var buffer = new byte[length];

            WinApi.ReadProcessMemory(PHandle, (IntPtr) address, buffer, length, out nBytesRead);

            return GetStructure<T>(buffer);
        }

        public static T Read<T>(IntPtr address)
        {
            var length = Marshal<T>.Size;

            if (typeof(T) == typeof(bool))
                length = 1;

            var buffer = new byte[length];

            WinApi.ReadProcessMemory(PHandle, address, buffer, length, out nBytesRead);

            return GetStructure<T>(buffer);
        }

        public static void Write<T>(int address, T value)
        {
            var length = Marshal<T>.Size;
            var buffer = new byte[length];
            var ptr = Marshal.AllocHGlobal((int) length);
            Marshal.StructureToPtr(value, ptr, true);
            Marshal.Copy(ptr, buffer, 0, (int) length);
            Marshal.FreeHGlobal(ptr);

            WinApi.WriteProcessMemory(PHandle, (IntPtr) address, buffer, (IntPtr) length, out nBytesRead);
        }

        public static void Write<T>(IntPtr address, T value)
        {
            var length = Marshal<T>.Size;
            var buffer = new byte[length];
            var ptr = Marshal.AllocHGlobal((int) length);
            Marshal.StructureToPtr(value, ptr, true);
            Marshal.Copy(ptr, buffer, 0, (int) length);
            Marshal.FreeHGlobal(ptr);

            WinApi.WriteProcessMemory(PHandle, address, buffer, (IntPtr) length, out nBytesRead);
        }

        public static byte[] ReadBytes(int address, int length)
        {
            var buffer = new byte[length];
            WinApi.ReadProcessMemory(PHandle, (IntPtr) address, buffer, (uint) length, out nBytesRead);
            return buffer;
        }

        public static byte[] ReadBytes(IntPtr address, int length)
        {
            var buffer = new byte[length];
            WinApi.ReadProcessMemory(PHandle, address, buffer, (uint) length, out nBytesRead);
            return buffer;
        }

        public static void WriteBytes(int address, byte[] value)
        {
            var nBytesRead = uint.MinValue;
            WinApi.WriteProcessMemory(PHandle, (IntPtr) address, value, (IntPtr) value.Length, out nBytesRead);
        }

        public static void WriteBytes(IntPtr address, byte[] value)
        {
            var nBytesRead = uint.MinValue;
            WinApi.WriteProcessMemory(PHandle, address, value, (IntPtr) value.Length, out nBytesRead);
        }

        public static string ReadString(int address, int bufferSize, Encoding enc)
        {
            var buffer = new byte[bufferSize];
            WinApi.ReadProcessMemory(PHandle, (IntPtr) address, buffer, (uint) bufferSize, out nBytesRead);
            var text = enc.GetString(buffer);
            if (text.Contains('\0'))
                text = text.Substring(0, text.IndexOf('\0'));

            return text;
        }
    }
}