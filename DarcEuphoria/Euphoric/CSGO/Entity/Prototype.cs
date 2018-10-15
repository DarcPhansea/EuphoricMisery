using System;
using DarcEuphoria.Euphoric.Classes;

namespace DarcEuphoria.Euphoric.CSGO.Entity
{
    public abstract class Prototype : IDisposable
    {
        public int Index;
        public Devalue<int> Pointer;

        protected Prototype(int index)
        {
            Index = index;
        }

        protected Prototype()
        {
        }
        //public void SetFields()
        //{
        //Pointer = new Devalue<int>(Memory.Client.Base + Offsets.dwEntityList + Index * 10);
        //}

        public int ClassID
        {
            get
            {
                var vt = Memory.Read<int>(Pointer.Value + 0x8);
                var fn = Memory.Read<int>(vt + 0x8);
                var cls = Memory.Read<int>(fn + 0x1);
                return Memory.Read<int>(cls + 0x1);
            }
        }

        public void Dispose()
        {
        }

        protected abstract void SetFields();
    }
}