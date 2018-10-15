using System.Runtime.InteropServices;

namespace DarcEuphoria.Euphoric
{
    public static class Marshal<T>
    {
        static Marshal()
        {
            Size = (uint) Marshal.SizeOf(typeof(T));
        }

        public static uint Size { get; }
    }
}