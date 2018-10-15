using System.Runtime.InteropServices;

namespace DarcEuphoria.Euphoric.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RENDERCOLOR
    {
        public byte R;
        public byte G;
        public byte B;
        public byte A;

        public RENDERCOLOR(byte r, byte g, byte b, byte a)
        {
            B = b;
            R = r;
            A = a;
            G = g;
        }
    }
}