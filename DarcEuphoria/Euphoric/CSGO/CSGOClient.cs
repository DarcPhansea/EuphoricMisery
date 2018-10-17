using System.Text;
using System.Threading;
using DarcEuphoria.Euphoric.Classes;
using DarcEuphoria.Euphoric.Structs;
using DarcEuphoria.Hacks.Injection;

namespace DarcEuphoria.Euphoric.CSGO
{
    public class CSGOClient
    {
        public Devalue<int> ClientState;
        public Devalue<int> EntityListLength;
        public Devalue<CGlobalVarsBase> GlobalVarsBase;
        public Devalue<int> GlowObjectManager;
        public Devalue<Matrix4x4> Matrix4;
        public Devalue<bool> PostProcessDisabled;
        public Devalue<int> TickBase;

        public CSGOClient()
        {
            ClientState = new Devalue<int>(Memory.Engine.Base + Offsets.dwClientState);
            PostProcessDisabled = new Devalue<bool>(Memory.Client.Base + Offsets.s_bOverridePostProcessingDisable);
            EntityListLength = new Devalue<int>(Memory.Engine.Base + Offsets.dwEntityListLength);
            TickBase = new Devalue<int>(Memory.Client.Base + Netvars.m_nTickBase);
            Matrix4 = new Devalue<Matrix4x4>(Memory.Client.Base + Offsets.dwViewMatrix, false, 2);
            GlowObjectManager = new Devalue<int>(Memory.Client.Base + Offsets.dwGlowObjectManager);
            GlobalVarsBase = new Devalue<CGlobalVarsBase>(Memory.Engine.Base + Offsets.dwGlobalVars);
        }

        public bool SendPackets
        {
            get => Memory.ReadBytes(Memory.Engine.Base + Offsets.dwSendPackets, 1)[0] == 1;
            set => Memory.Write(Memory.Engine.Base + Offsets.dwSendPackets, value ? (byte) 1 : (byte) 0);
        }

        public bool InGame
        {
            get
            {
                try
                {
                    if (CSGOEngine.LocalPlayer.ActiveWeapon.WeaponName == "null")
                        return false;
                }
                catch
                {
                }

                return Memory.Read<int>(ClientState.Value + Offsets.dwClientState_State) == 6;
            }
        }

        public string MapName =>
            Memory.ReadString(ClientState.Value + Offsets.dwClientState_Map, 32, Encoding.ASCII);

        public void ForceFullUpdate()
        {
            SendPackets = true;
            Thread.Sleep(10);
            //Memory.Write(ClientState.Value + Offsets.ForceUpdate, -1);
            ClientCmd.Exec("record x; stop");
        }
    }
}