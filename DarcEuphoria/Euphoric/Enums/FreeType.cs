using System;

namespace DarcEuphoria.Euphoric.Enums
{
    [Flags]
    public enum FreeType
    {
        Decommit = 0x4000,
        MEM_COMMIT = 0x1000,
        MEM_RELEASE = 0x8000,
        MEM_RESERVE = 0x2000
    }
}