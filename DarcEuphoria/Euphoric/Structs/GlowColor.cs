using System.Runtime.InteropServices;
using SharpDX.Mathematics.Interop;

namespace DarcEuphoria.Euphoric.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct GlowColor
    {
        public float Red;
        public float Green;
        public float Blue;
        public float Alpha;


        public GlowColor(float r, float g, float b, float a)
        {
            Red = r;
            Green = g;
            Blue = b;
            Alpha = a;
        }

        public GlowColor(RawColor4 color)
        {
            Red = color.R;
            Green = color.G;
            Blue = color.B;
            Alpha = color.A;
        }
    }
}