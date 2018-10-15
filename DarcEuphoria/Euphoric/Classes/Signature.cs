using System;
using System.Globalization;
using DarcEuphoria.Euphoric.ProcessScanner;

namespace DarcEuphoria.Euphoric.Classes
{
    public class Signature
    {
        public IntPtr BaseAddress = IntPtr.Zero;
        public byte[] ByteArray;
        public int Extra;
        public string Mask;
        public string Module;
        public int Offset;
        public string Sig;

        public Signature(string module, string signature, int offset = 0, int extra = 0)
        {
            Module = module;
            Sig = signature;
            Offset = offset;
            Extra = extra;

            var mask = string.Empty;
            var patternBlocks = signature.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            var pattern = new byte[patternBlocks.Length];

            for (var i = 0; i < patternBlocks.Length; i++)
            {
                var block = patternBlocks[i];
                if (block == "?")
                {
                    mask += block;
                    pattern[i] = 0;
                }
                else
                {
                    mask += "x";
                    if (!byte.TryParse(patternBlocks[i], NumberStyles.HexNumber,
                        CultureInfo.DefaultThreadCurrentCulture, out pattern[i]))
                        throw new Exception("Signature Parsing Error");
                }
            }

            ByteArray = pattern;

            Mask = mask;
        }

        public int Value
        {
            get
            {
                if (BaseAddress == IntPtr.Zero)
                    BaseAddress = (IntPtr) OffsetScanner.Find(this);

                return (int) BaseAddress;
            }
        }
    }
}