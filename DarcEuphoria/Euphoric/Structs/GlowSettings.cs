using System.Runtime.InteropServices;

namespace DarcEuphoria.Euphoric.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct GlowSettings
    {
        public byte m_renderWhenOccluded;
        public byte m_renderWhenUnoccluded;
        public byte m_fullBloomRender;

        public GlowSettings(bool fullBloom)
        {
            m_renderWhenOccluded = 1;
            m_renderWhenUnoccluded = 1;
            m_fullBloomRender = fullBloom ? (byte) 1 : (byte) 0;
        }
    }
}