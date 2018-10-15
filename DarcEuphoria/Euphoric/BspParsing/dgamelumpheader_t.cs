using System.Runtime.InteropServices;

namespace DarcEuphoria.Euphoric.BspParsing
{
    [StructLayout(LayoutKind.Sequential)]
    public struct dgamelumpheader_t
    {
        public int m_LumpCount;
    }
}