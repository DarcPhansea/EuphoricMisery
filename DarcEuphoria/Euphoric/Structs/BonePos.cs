using System.Runtime.InteropServices;

namespace DarcEuphoria.Euphoric.Structs
{
    [StructLayout(LayoutKind.Explicit)]
    public struct BonePos
    {
        [FieldOffset(0xC)] public float X;
        [FieldOffset(0x1C)] public float Y;
        [FieldOffset(0x2C)] public float Z;

        public Vector3 ToVector3()
        {
            return new Vector3(X, Y, Z);
        }
    }
}