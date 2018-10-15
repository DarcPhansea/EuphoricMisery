using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

namespace DarcEuphoria.Euphoric.ProcessScanner
{
    public static class NetvarManager
    {
        public static Dictionary<string, Dictionary<string, int>> Table =
            new Dictionary<string, Dictionary<string, int>>();

        public static int FirstTable(string pattern_str, int offset)
        {
            var temp = new List<byte>();
            var mask = "";

            foreach (var l in pattern_str.Split(' '))
                if (l == "?" || l == "00")
                {
                    temp.Add(0x00);
                    mask += "?";
                }
                else
                {
                    temp.Add((byte) int.Parse(l, NumberStyles.HexNumber));
                    mask += "x";
                }

            var pattern = temp.ToArray();

            var moduleBytes = new byte[Memory.Client.Size];
            uint numBytes = 0;

            if (WinApi.ReadProcessMemory(Memory.PHandle, Memory.Client.Base, moduleBytes, (uint) Memory.Client.Size,
                out numBytes))
                for (var i = 0; i < Memory.Client.Size; i++)
                {
                    var found = true;

                    for (var l = 0; l < mask.Length; l++)
                    {
                        found = mask[l] == '?' || moduleBytes[l + i] == pattern[l];

                        if (!found) break;
                    }

                    if (found)
                    {
                        i += (int) Memory.Client.Base;
                        i = Memory.Read<int>(i + offset);
                        return i;
                    }
                }

            return 0;
        }

        public static void ScanTable(IntPtr table, int level, int offset, string name)
        {
            var count = Memory.Read<int>((int) table + 0x4);

            for (var i = 0; i < count; i++)
            {
                var propID = Memory.Read<int>((int) table) + i * 0x3C;
                var propName = Memory.ReadString(Memory.Read<int>(propID), 64, Encoding.Default);
                var isBaseClass = propName.IndexOf("baseclass") == 0;
                var propOffset = offset + Memory.Read<int>(propID + 0x2C);
                if (!isBaseClass)
                {
                    if (!Table.ContainsKey(name))
                        Table.Add(name, new Dictionary<string, int>());

                    if (!Table[name].ContainsKey(propName)) Table[name].Add(propName, propOffset);
                }

                var child = Memory.Read<IntPtr>(propID + 0x28);

                if (child == IntPtr.Zero)
                    continue;

                if (!isBaseClass)
                    --level;

                ScanTable(child, ++level, propOffset, name);
            }
        }

        public static void Init()
        {
            var _firstclass = new IntPtr(FirstTable(
                "A1 ? ? ? ? C3 CC CC CC CC CC CC CC CC CC CC A1 ? ? ? ? B9", 0x1));


            _firstclass = Memory.Read<IntPtr>((int) _firstclass);

            if (_firstclass == IntPtr.Zero)
            {
                MessageBox.Show("Error Has Occured While Getting The NetVars...\nExiting Now.");
                Memory.CloseHandle();
                Environment.Exit(-1);
            }

            do
            {
                var table = Memory.Read<IntPtr>((int) _firstclass + 0xC);
                if (table != IntPtr.Zero)
                {
                    var table_name = Memory.ReadString(Memory.Read<int>((int) table + 0xC), 32, Encoding.Default);
                    ScanTable(table, 0, 0, table_name);
                }

                _firstclass = Memory.Read<IntPtr>((int) _firstclass + 0x10);
            } while (_firstclass != IntPtr.Zero);
        }
    }
}