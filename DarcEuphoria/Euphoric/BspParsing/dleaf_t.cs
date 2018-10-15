using System.Runtime.InteropServices;

namespace DarcEuphoria.Euphoric.BspParsing
{
    [StructLayout(LayoutKind.Sequential)]
    public struct dleaf_t
    {
        public ContentsFlag m_Contents;
        public short m_Cluster;
        public short m_Area;
        public area_flags m_AreaFlags;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public short[] m_Mins;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public short[] m_Maxs;

        public ushort m_Firstleafface;
        public ushort m_Numleaffaces;
        public ushort m_Firstleafbrush;
        public ushort m_Numleafbrushes;
        public short m_LeafWaterDataID;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct area_flags
    {
        private readonly short m_Data;

        public short m_Area => (short) (m_Data & 0x1ff);
        public short m_Flags => (short) (m_Data >> 9);
    }
}