using System;
using System.Windows.Forms;
using DarcEuphoria.Euphoric.Classes;
using DarcEuphoria.Euphoric.Structs;

namespace DarcEuphoria.Euphoric.ProcessScanner
{
    internal static class OffsetScanner
    {
        private static byte[] _dump;

        private static void Dump(Module module)
        {
            _dump = Memory.ReadBytes(module.Base, module.Size);
        }

        private static bool CheckSignature(int index, Signature sig)
        {
            for (var i = 0; i < sig.ByteArray.Length; i++)
            {
                if (sig.Mask[i] == '?')
                    continue;

                if (sig.ByteArray[i] != _dump[index + i])
                    return false;
            }

            return true;
        }

        public static int Find(Signature sig)
        {
            var mod = new Module();

            if (sig.Module == "client.dll")
                mod = Memory.Client;
            else if (sig.Module == "engine.dll")
                mod = Memory.Engine;

            Dump(mod);

            for (var i = 0; i < mod.Size; i++)
                if (sig.BaseAddress == IntPtr.Zero && CheckSignature(i, sig))
                {
                    sig.BaseAddress = mod.Base + i + sig.Offset;

                    if (sig.BaseAddress != IntPtr.Zero)
                        return BitConverter.ToInt32(Memory.ReadBytes(sig.BaseAddress, 4), 0) +
                               sig.Extra - mod.Base.ToInt32();
                }

            MessageBox.Show(string.Format("The Signature {0}\nCouldn't Be Found", sig.Sig));
            return int.MinValue;
        }

        public static int GetSig(Signature sig)
        {
            var mod = new Module();

            if (sig.Module == "client.dll")
                mod = Memory.Client;
            else if (sig.Module == "engine.dll")
                mod = Memory.Engine;

            for (var i = 0; i < mod.Size; i++)
                if (sig.BaseAddress == IntPtr.Zero && CheckSignature(i, sig))
                {
                    var offset = sig.Offset;
                    sig.BaseAddress = mod.Base + i + offset;

                    if (sig.BaseAddress != IntPtr.Zero) return (int) sig.BaseAddress;
                }

            return (int) sig.BaseAddress;
        }

        public static int FindPattern(Signature sig)
        {
            var mod = new Module();

            if (sig.Module == "client.dll")
                mod = Memory.Client;
            else if (sig.Module == "engine.dll")
                mod = Memory.Engine;

            var modBytes = new byte[mod.Size];
            uint numBytes = 0;

            if (WinApi.ReadProcessMemory(Memory.PHandle, mod.Base, modBytes, (uint) mod.Size, out numBytes))
                for (var i = 0; i < mod.Size; i++)
                {
                    var found = true;

                    for (var j = 0; j < sig.Mask.Length; j++)
                    {
                        found = sig.Mask[j] == '?' || modBytes[j + i] == sig.ByteArray[j];

                        if (!found)
                            break;
                    }

                    if (found)
                        return i;
                }

            return 0;
        }
    }
}