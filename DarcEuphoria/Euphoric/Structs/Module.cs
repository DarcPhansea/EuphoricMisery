using System;

namespace DarcEuphoria.Euphoric.Structs
{
    public struct Module
    {
        public IntPtr Base;
        public int Size;

        public Module(IntPtr basee, int size)
        {
            Base = basee;
            Size = size;
        }
    }
}