using System.Runtime.InteropServices;

namespace DarcEuphoria.Euphoric.BspParsing
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct dnode_t
    {
        public int m_Planenum;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public int[] m_Children;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public short[] m_Mins;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public short[] m_Maxs;

        public ushort m_Firstface;
        public ushort m_Numfaces;
        public short m_Area;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        private readonly byte[] m_Pad;
    }
}