using System.Collections.Generic;
using System.Text;
using DarcEuphoria.Euphoric.Classes;
using DarcEuphoria.Euphoric.Structs;

namespace DarcEuphoria.Euphoric.CSGO.Entity
{
    public class BaseWeapon
    {
        public Devalue<int> AccountID;
        public Devalue<int> Base;
        public Devalue<float> BombTime;
        public Devalue<int> GlowIndex;
        public Devalue<bool> IsDormant;
        public Devalue<int> ItemIDLow;
        public Devalue<int> MyAccountID;
        public Devalue<float> NextPrimaryAttack;

        public Devalue<int> PaintKit;
        public int Pointer;
        public Devalue<Vector3> Position;
        public Devalue<int> Seed;
        public Devalue<int> StatTrak;
        public Devalue<int> WeaponClip;
        public Devalue<short> WeaponID;
        public Devalue<float> Wear;
        public Devalue<int> ZoomLevel;

        public BaseWeapon()
        {
        }

        public BaseWeapon(int index, bool byindex)
        {
            Base = new Devalue<int>(Memory.Client.Base + Offsets.dwEntityList + index * 0x10);
            WeaponID = new Devalue<short>(Base.Value + Netvars.m_iItemDefinitionIndex);
            Position = new Devalue<Vector3>(Base.Value + Netvars.m_vecOrigin);
            IsDormant = new Devalue<bool>(Base.Value + Offsets.m_bDormant);
            BombTime = new Devalue<float>(Base.Value + Netvars.m_flC4Blow);
        }

        public BaseWeapon(int ptr)
        {
            Pointer = ptr;
            Base = new Devalue<int>(Memory.Client.Base + Offsets.dwEntityList + (Pointer - 1) * 0x10);
            WeaponID = new Devalue<short>(Base.Value + Netvars.m_iItemDefinitionIndex);
            WeaponClip = new Devalue<int>(Pointer + Netvars.m_iClip1);
            NextPrimaryAttack = new Devalue<float>(Pointer + Netvars.m_flNextPrimaryAttack);
            Position = new Devalue<Vector3>(Pointer + Netvars.m_vecOrigin);

            PaintKit = new Devalue<int>(Base.Value + Netvars.m_nFallbackPaintKit);
            Seed = new Devalue<int>(Base.Value + Netvars.m_nFallbackSeed);
            Wear = new Devalue<float>(Base.Value + Netvars.m_flFallbackWear);
            StatTrak = new Devalue<int>(Base.Value + Netvars.m_nFallbackStatTrak);

            MyAccountID = new Devalue<int>(Base.Value + Netvars.m_OriginalOwnerXuidLow);
            AccountID = new Devalue<int>(Base.Value + Netvars.m_iAccountID);
            ItemIDLow = new Devalue<int>(Base.Value + Netvars.m_iItemIDLow);
            ZoomLevel = new Devalue<int>(Base.Value + Netvars.m_zoomLevel);

            IsDormant = new Devalue<bool>(Pointer + Offsets.m_bDormant);
        }

        public static BaseWeapon[] EntityList
        {
            get
            {
                var returnArray = new List<BaseWeapon>();

                for (var i = 65; i < CSGOEngine.csClient.EntityListLength.Value; i++)
                {
                    var player = new BaseWeapon(i, true);

                    if (player.Base.Value == 0) continue;
                    if (player.ClassName == "NOCLASSNAME") continue;
                    if (player.IsDormant.Value) continue;
                    if (player.HasOwner()) continue;
                    returnArray.Add(player);
                }

                return returnArray.ToArray();
            }
        }

        public int ClassID
        {
            get
            {
                var vt = Memory.Read<int>(Base.Value + 0x8);
                var fn = Memory.Read<int>(vt + 0x8);
                var cls = Memory.Read<int>(fn + 0x1);
                return Memory.Read<int>(cls + 0x14);
            }
        }

        public string ClassName
        {
            get
            {
                switch (ClassID)
                {
                    case 83:
                        return "Hostage";
                    case 44:
                        return "Defuser";
                    case 202:
                        return "AUG";
                    case 222:
                        return "MAG-10";
                    case 226:
                        return "MP9";
                    case 227:
                        return "Negev";
                    case 39:
                        return "Deagle";
                    case 108:
                        return "Planted C4";
                    case 29:
                        return "C4";
                    case 237:
                        return "SG 553";
                    case 211:
                        return "Dual Berettas";
                    case 217:
                        return "Glock-18";
                    case 219:
                        return "M249";
                    case 31:
                        return "Chicken";
                    case 207:
                        return "PP-Bizon";
                    case 134:
                        return "Smoke";
                    case 231:
                        return "P90";
                    case 230:
                        return "P250";
                    case 223:
                        return "MAG-7";
                    case 214:
                        return "G3SG1";
                    case 205:
                        return "AWP";
                    case 225:
                        return "MP7";
                    case 212:
                        return "FAMAS";
                    case 1:
                        return "AK-47";
                    case 240:
                        return "Tec-9";
                    case 232:
                        return "Sawed-Off";
                    case 233:
                        return "SCAR-20";
                    case 228:
                        return "Nova";
                    case 218:
                        return "USP-S";
                    case 244:
                        return "XM1014";
                    case 239:
                        return "Zeus x27";
                    case 221:
                        return "M4A1";
                    case 242:
                        return "UMP-45";
                    case 213:
                        return "Five-SeveN";
                    case 238:
                        return "SSG 08";
                    case 216:
                        return "Galil-AR";


                    case 84:
                        return "HE Grenade";
                    case 9:
                        return "Grenade";
                    case 8:
                        return "Grenade";
                    case 88:
                        return "Incendiary";
                    case 97:
                        return "Incendiary";
                    case 66:
                        return "Flashbang";
                    case 87:
                        return "Incendiary";
                    case 98:
                        return "Incendiary";
                    case 133:
                        return "Smoke";
                    case 40:
                        return "Decoy";
                    case 41:
                        return "Decoy";
                }

                return "NOCLASSNAME";
            }
        }

        public string Name
        {
            set
            {
                if (string.IsNullOrEmpty(value))
                    return;

                var charArray = new char[32];

                for (var i = 0; i < charArray.Length; i++)
                    if (i < value.Length)
                        charArray[i] = value[i];

                var byteArray = Encoding.Default.GetBytes(charArray);


                Memory.WriteBytes(Base.Value + Netvars.m_szCustomName, byteArray);
            }
        }

        public bool AbleToFire => NextPrimaryAttack.Value < CSGOEngine.csClient.TickBase.Value;

        public string WeaponName
        {
            get
            {
                if (IsKnife()) return "Knife";

                switch (WeaponID.Value)
                {
                    case 1: return "Desert Eagle";
                    case 2: return "Duel Berettas";
                    case 3: return "Five-SeveN";
                    case 4: return "Glock-18";
                    case 7: return "AK-47";
                    case 8: return "AUG";
                    case 9: return "AWP";
                    case 10: return "FAMAS";
                    case 11: return "G3SG1";
                    case 13: return "Galil AR";
                    case 14: return "M249";
                    case 16: return "M4A4";
                    case 17: return "MAC-10";
                    case 19: return "P90";
                    case 23: return "MP5-SD";
                    case 24: return "UMP-45";
                    case 25: return "XM1014";
                    case 26: return "PP-Bizon";
                    case 27: return "MAG-7";
                    case 28: return "Negev";
                    case 29: return "Sawed-Off";
                    case 30: return "Tec-9";
                    case 31: return "Zeus x27";
                    case 32: return "P2000";
                    case 33: return "MP7";
                    case 34: return "MP9";
                    case 35: return "Nova";
                    case 36: return "P250";
                    case 38: return "SCAR-20";
                    case 39: return "SG 553";
                    case 40: return "SSG 08";
                    case 43: return "Flashbang";
                    case 44: return "HE Grenade";
                    case 45: return "Smoke Grenade";
                    case 46: return "Molotov";
                    case 47: return "Decoy";
                    case 48: return "Incendiary";
                    case 49: return "C4";
                    case 69: return "M4A1-S";
                    case 61: return "USP-S";
                    case 63: return "CZ75-Auto";
                    case 64: return "R8 Revolver";
                    default: return "NOWEAPONNAME";
                }
            }
        }

        public bool HasOwner()
        {
            return Position.Value == Vector3.Zero;
        }

        public BaseWeapon GetWeapon(int player, int index)
        {
            var ptr = Memory.Read<int>(player + Netvars.m_hMyWeapons + (index - 1) * 0x4) & 0xFFF;
            return new BaseWeapon(ptr);
        }

        public BaseWeapon ActiveWeapon(int player)
        {
            var ptr = Memory.Read<int>(player + Netvars.m_hActiveWeapon) & 0xFFF;
            return new BaseWeapon(ptr);
        }

        public bool IsWeapon()
        {
            if (IsKnife() || IsPistol() || IsSniper() ||
                IsRifle() || IsSmg() || IsShotgun() ||
                IsLmg()) return true;

            return false;
        }

        public bool IsBomb()
        {
            if (WeaponID.Value == 49) return true;
            return false;
        }

        public bool IsDefuser()
        {
            if (ClassID == 44) return true;
            return false;
        }

        public bool IsGrenade()
        {
            switch (WeaponID.Value)
            {
                case 43:
                case 44:
                case 45:
                case 46:
                case 47:
                case 48:
                    return true;
                default:
                    return false;
            }
        }

        public bool IsKnife()
        {
            switch (WeaponID.Value)
            {
                case 41:
                case 42:
                case 59:
                case 500:
                case 505:
                case 506:
                case 507:
                case 508:
                case 509:
                case 512:
                case 514:
                case 515:
                case 516:
                case 519:
                case 520:
                case 522:
                case 523:
                    return true;
                default:
                    return false;
            }
        }

        public bool IsPistol()
        {
            switch (WeaponID.Value)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                case 30:
                case 32:
                case 36:
                case 61:
                case 63:
                case 64:
                    return true;
                default:
                    return false;
            }
        }

        public bool IsSniper()
        {
            switch (WeaponID.Value)
            {
                case 9:
                case 11:
                case 38:
                case 40:
                    return true;
                default:
                    return false;
            }
        }

        public bool IsRifle()
        {
            switch (WeaponID.Value)
            {
                case 7:
                case 8:
                case 10:
                case 13:
                case 16:
                case 39:
                case 60:
                    return true;

                default:
                    return false;
            }
        }

        public bool IsSmg()
        {
            switch (WeaponID.Value)
            {
                case 17:
                case 19:
                case 24:
                case 26:
                case 33:
                case 34:
                case 23:
                    return true;
                default:
                    return false;
            }
        }

        public bool IsShotgun()
        {
            switch (WeaponID.Value)
            {
                case 25:
                case 27:
                case 29:
                case 35:
                    return true;
                default:
                    return false;
            }
        }

        public bool IsLmg()
        {
            switch (WeaponID.Value)
            {
                case 14:
                case 28:
                    return true;
                default:
                    return false;
            }
        }
    }
}