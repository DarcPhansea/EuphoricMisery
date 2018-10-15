using System.Runtime.InteropServices;

namespace DarcEuphoria.Euphoric.BspParsing
{
    [StructLayout(LayoutKind.Sequential)]
    public struct StaticPropLeafLump_t
    {
        public int m_LeafEntries;
    }
}