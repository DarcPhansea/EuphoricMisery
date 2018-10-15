using DarcEuphoria.Euphoric.Classes;
using DarcEuphoria.Euphoric.Structs;

namespace DarcEuphoria.Euphoric.CSGO.Entity
{
    public class BaseEntity : Prototype
    {
        public Devalue<int> GlowIndex;
        public Devalue<bool> IsDormant;
        public Devalue<Vector3> Position;

        public BaseEntity(int index) : base(index)
        {
        }

        public BaseEntity()
        {
        }

        protected override void SetFields()
        {
            IsDormant = new Devalue<bool>(Pointer.Value + Offsets.m_bDormant);
            GlowIndex = new Devalue<int>(Pointer.Value + Netvars.m_iGlowIndex);
            Position = new Devalue<Vector3>(Pointer.Value + Netvars.m_vecOrigin);
        }
    }
}