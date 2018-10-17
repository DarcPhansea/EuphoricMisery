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
        private int Sleep;

        public Devalue(int address, bool force = false, int sleep = 10)
        {
            Address = address;
            Force = force;
            Refresh = -1;
            Sleep = sleep;
        }

        public Devalue(IntPtr address, bool force = false, int sleep = 10)
        {
            Address = (int) address;
            Force = force;
            Refresh = -1;
            Sleep = sleep;
        }

        public T Value
        {
            get
            {
                if (Refresh < GlobalVariables.GlobalRefresh)
                {
                    Refresh = GlobalVariables.GlobalRefresh + Sleep;
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