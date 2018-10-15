using System.Runtime.InteropServices;
using DarcEuphoria.Euphoric.Structs;

namespace DarcEuphoria.Euphoric.BspParsing
{
    [StructLayout(LayoutKind.Sequential)]
    public struct mvertex_t
    {
        public Vector3 m_Position;
    }
}