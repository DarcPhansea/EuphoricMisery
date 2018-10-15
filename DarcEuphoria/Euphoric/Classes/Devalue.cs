using System;

namespace DarcEuphoria.Euphoric.Classes
{
    public class Devalue<T>
    {
        public readonly int Address;
        private readonly bool Force;
        private T _value;
        private byte[] bytes = new byte[1] {0};
        private int Refresh;

        public Devalue(int address, bool force = false)
        {
            Address = address;
            Force = force;
            Refresh = -1;
        }

        public Devalue(IntPtr address, bool force = false)
        {
            Address = (int) address;
            Force = force;
            Refresh = -1;
        }

        public T Value
        {
            get
            {
                if (Refresh != GlobalVariables.GlobalRefresh)
                {
                    Refresh = GlobalVariables.GlobalRefresh;
                    _value = Memory.Read<T>(Address);

                    bytes = Memory.GetStructBytes(_value);
                }

                return _value;
            }
            set
            {
                var _byte = Memory.GetStructBytes(value);

                if (_byte == bytes) return;

                if (Force)
                    for (var i = 0; i < 1000; i++)
                        Memory.Write(Address, value);
                else
                    Memory.Write(Address, value);
            }
        }
    }
}