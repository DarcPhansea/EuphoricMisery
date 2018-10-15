using System.Runtime.InteropServices;
using DarcEuphoria.Euphoric.Structs;

namespace DarcEuphoria.Euphoric.BspParsing
{
    [StructLayout(LayoutKind.Sequential)]
    public struct VPlane
    {
        public Vector3 m_Origin;
        public float m_Distance;

        public VPlane(Vector3 origin, float dist)
        {
            m_Origin = origin;
            m_Distance = dist;
        }

        public float DistTo(Vector3 location)
        {
            return m_Origin.Dot(location) - m_Distance;
        }
    }
}