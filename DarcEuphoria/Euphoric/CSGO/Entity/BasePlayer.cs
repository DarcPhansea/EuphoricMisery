using System.Collections.Generic;
using DarcEuphoria.Euphoric.Classes;
using DarcEuphoria.Euphoric.Enums;
using DarcEuphoria.Euphoric.Structs;

namespace DarcEuphoria.Euphoric.CSGO.Entity
{
    public class BasePlayer : BaseEntity
    {
        private BaseWeapon _BaseWeapon;

        private Devalue<int> _lifeState;
        public Devalue<int> BoneMatrix;
        public Devalue<int> GameResource;
        public Devalue<int> Health;
        public Devalue<int> ObserverMode;
        public Devalue<int> ObserverTarget;

        private int rActiveWeapon = -1;
        public Devalue<RENDERCOLOR> RenderColor;
        public Devalue<bool> Spotted;
        public Devalue<bool> SpottedByMask;
        public Devalue<int> Team;
        public Devalue<Vector3> VectorVelocity;

        public BasePlayer(int index) : base(index)
        {
            Pointer = new Devalue<int>(Memory.Client.Base + Offsets.dwEntityList + (Index - 1) * 0x10);
            SetFields();
        }

        public BasePlayer()
        {
            Pointer = new Devalue<int>(Memory.Client.Base + Offsets.dwLocalPlayer);
            SetFields();
        }

        public static BasePlayer[] PlayerList
        {
            get
            {
                var returnArray = new List<BasePlayer>();
                for (var i = 0; i < 64; i++)
                {
                    var player = new BasePlayer(i);

                    if (player.Pointer.Value == 0) continue;

                    if (GlobalVariables.ActiveSettings.MiscSettings.InGameRadar)
                        player.Spotted.Value = true;

                    returnArray.Add(player);
                }

                return returnArray.ToArray();
            }
        }

        public bool IsVisible => CSGOEngine.bspMap.IsVisible(CSGOEngine.LocalPlayer.EyeLevel, BonePosition(6));

        public BaseWeapon ActiveWeapon
        {
            get
            {
                if (rActiveWeapon != GlobalVariables.GlobalRefresh)
                {
                    rActiveWeapon = GlobalVariables.GlobalRefresh;
                    _BaseWeapon = new BaseWeapon().ActiveWeapon(Pointer.Value);
                }

                return _BaseWeapon;
            }
        }


        public string Name
        {
            get
            {
                var nameAddr = Offsets.RadarPointer + (Index + 1) * 0x168 + 0x18;

                return Memory.ReadString(nameAddr, 64, Offsets.RadarEncoding);
            }
        }

        public Ranks Rank =>
            (Ranks) Memory.Read<int>(GameResource.Value + Netvars.m_iCompetitiveRanking + Index + 0x4);

        public LifeState LifeState => (LifeState) _lifeState.Value;

        public bool IsLocalPlayer()
        {
            return Pointer.Value == CSGOEngine.LocalPlayer.Pointer.Value;
        }

        public bool IsSameTeam()
        {
            return Team.Value == CSGOEngine.LocalPlayer.Team.Value;
        }

        protected override void SetFields()
        {
            base.SetFields();
            GameResource = new Devalue<int>(Memory.Client.Base + Offsets.dwPlayerResource);
            Health = new Devalue<int>(Pointer.Value + Netvars.m_iHealth);
            _lifeState = new Devalue<int>(Pointer.Value + Netvars.m_lifeState);
            ObserverTarget = new Devalue<int>(Pointer.Value + Netvars.m_hObserverTarget);
            ObserverMode = new Devalue<int>(Pointer.Value + Netvars.m_iObserverMode);
            Team = new Devalue<int>(Pointer.Value + Netvars.m_iTeamNum);
            BoneMatrix = new Devalue<int>(Pointer.Value + Netvars.m_dwBoneMatrix);
            Spotted = new Devalue<bool>(Pointer.Value + Netvars.m_bSpotted);
            SpottedByMask = new Devalue<bool>(Pointer.Value + Netvars.m_bSpottedByMask);
            VectorVelocity = new Devalue<Vector3>(Pointer.Value + Netvars.m_vecVelocity);
            RenderColor = new Devalue<RENDERCOLOR>(Pointer.Value + Netvars.m_clrRender);
        }

        public Vector3 BonePosition(int Bone)
        {
            var bonePosition = Memory.Read<BonePos>(BoneMatrix.Value + 0x30 * Bone);
            return bonePosition.ToVector3();
        }
    }
}