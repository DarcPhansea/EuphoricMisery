using System;
using System.Text;
using DarcEuphoria.Euphoric.Classes;
using DarcEuphoria.Euphoric.ProcessScanner;

namespace DarcEuphoria.Euphoric.CSGO
{
    internal static class Offsets
    {
        public const int m_bDormant = 0xE9;
        public const int m_szArmsModel = 0x38D7;
        public const int m_iHideHUD = 0x2FF4;
        public const int m_viewFOV = 0x330C;
        public static int m_dwClientState_LastOutgoingCommand = 0x4CA8;
        public static int m_nModelIndex = 0x254;

        public static int RadarStructSize = 0x168;
        public static int RadarStructPosition = 0x18;
        public static Encoding RadarEncoding = Encoding.UTF8;

        public static int dwClientState;
        public static int dwClientState_GetLocalPlayer;
        public static int dwClientState_IsHLTV;
        public static int dwClientState_Map;
        public static int dwClientState_MapDirectory;
        public static int dwClientState_MaxPlayer;
        public static int dwClientState_PlayerInfo;
        public static int dwClientState_State;
        public static int dwClientState_ViewAngles;
        public static int dwEntityList;
        public static int dwForceAttack;
        public static int dwForceAttack2;
        public static int dwForceUse;
        public static int dwGameDir;
        public static int dwGameRulesProxy;
        public static int dwGlobalVars;
        public static int dwGlowObjectManager;
        public static int dwInput;
        public static int dwLocalPlayer;
        public static int dwPlayerResource;
        public static int dwRadarBase;
        public static int dwSetClanTag;
        public static int dw_SetConVar;
        public static int dwViewMatrix;
        public static int dwWeaponTable;
        public static int dwWeaponTableIndex;
        public static int dw_RevealRankFn;
        public static int dwSendPackets;
        public static int dwForceJump;
        public static int LastOutGoingCommand;
        public static int dwEntityListLength;
        public static int dw_clientCmd;
        public static int s_bOverridePostProcessingDisable;
        public static int noarms;
        public static Devalue<int> RadarBase;
        public static int RadarPointer;
        public static int ForceUpdate = 0x16C;

        public static void Init()
        {
            dwClientState = new Signature("engine.dll", "A1 ? ? ? ? 33 D2 6A 00 6A 00 33 C9 89 B0", 0x1).Value;
            dwClientState_Map =
                Convert.ToInt32(
                    new Signature("engine.dll", "05 ? ? ? ? C3 CC CC CC CC CC CC CC A1", 0x1).Value.ToString("X")
                        .Substring(4), 16);
            dwClientState_State =
                Convert.ToInt32(
                    new Signature("engine.dll", "83 B8 ? ? ? ? ? 0F 94 C0 C3", 0x2).Value.ToString("X").Substring(4),
                    16);
            dwClientState_ViewAngles =
                Convert.ToInt32(
                    new Signature("engine.dll", "F3 0F 11 80 ? ? ? ? D9 46 04 D9 05", 0x4).Value.ToString("X")
                        .Substring(4), 16);

            dwGlobalVars = new Signature("engine.dll", "68 ? ? ? ? 68 ? ? ? ? FF 50 08 85 C0", 0x1).Value;
            dwLocalPlayer =
                new Signature("client.dll", "A3 ? ? ? ? C7 05 ? ? ? ? ? ? ? ? E8 ? ? ? ? 59 C3 6A ?", 0x1, 16).Value;
            dwRadarBase = new Signature("client.dll", "A1 ? ? ? ? 8B 0C B0 8B 01 FF 50 ? 46 3B 35 ? ? ? ? 7C EA 8B 0D",
                0x1).Value;
            dwForceJump = new Signature("client.dll", "8B 0D ? ? ? ? 8B D6 8B C1 83 CA 02", 0x2).Value;
            dwForceAttack = new Signature("client.dll", "89 0D ? ? ? ? 8B 0D ? ? ? ? 8B F2 8B C1 83 CE 04", 0x2).Value;
            dwForceAttack2 = new Signature("client.dll", "23 C8 89 ? ? ? ? ? 8B ? ? ? ? ? 8B F2 8B C1 81 CE 00 20", 0x4)
                .Value;

            dwForceUse = new Signature("client.dll", "8B 0D ? ? ? ? 8B F2 8B C1 83 CE 20", 0x2).Value;

            dwEntityListLength = new Signature("engine.dll", "89 01 89 0D ? ? ? ? 66", -0x4).Value;
            s_bOverridePostProcessingDisable = new Signature("client.dll", "80 3D ? ? ? ? ? 53 56 57 0F 85", 0x2).Value;
            dwEntityList = new Signature("client.dll", "BB ? ? ? ? 83 FF 01 0F 8C ? ? ? ? 3B F8", 0x1).Value;
            dw_RevealRankFn = new Signature("client.dll", "C7 00 ? ? ? ? E8 ? ? ? ? 83 EC 08 8D 4E 74", 0x2).Value;
            dwInput = new Signature("client.dll", "B9 ? ? ? ? F3 0F 11 04 24 FF 50 10", 0x1).Value;
            dwViewMatrix = new Signature("client.dll", "0F 10 05 ? ? ? ? 8D 85 ? ? ? ? B9", 0x3, 176).Value;
            dwPlayerResource = new Signature("client.dll", "8B 3D ? ? ? ? 85 FF 0F 84 ? ? ? ? 81 C7", 0x2).Value;
            dwGlowObjectManager = new Signature("client.dll", "A1 ? ? ? ? A8 01 75 4B", 0x1, 4).Value;

            LastOutGoingCommand = (int) Memory.Engine.Base +
                                  new Signature("engine.dll",
                                      "C7 80 ? ? ? ? ? ? ? ? A1 ? ? ? ? F2 0F 10 05 ? ? ? ? F2 0F 11 80 ? ? ? ? FF 15",
                                      0x2).Value;

            dw_SetConVar = OffsetScanner.FindPattern(new Signature("engine.dll", "8D 4C 24 1C E8 ? ? ? ? 56"));
            dwSetClanTag = OffsetScanner.FindPattern(new Signature("engine.dll", "53 56 57 8B DA 8B F9 FF 15"));
            dw_clientCmd = OffsetScanner.FindPattern(new Signature("engine.dll",
                "55 8B EC 8B 0D ? ? ? ? 81 F9 ? ? ? ? 75 0C A1 ? ? ? ? 35 ? ? ? ? EB 05 8B 01 FF 50 34 50 A1"));

            dwSendPackets = OffsetScanner.FindPattern(new Signature("engine.dll",
                                "B3 01 8B 01 8B 40 10 FF D0 84 C0 74 0F 80 BF ? ? ? ? ? 0F 84")) + 1;

            noarms = Memory.Read<int>(dwLocalPlayer + 0x254);
            RadarBase = new Devalue<int>(Memory.Client.Base + dwRadarBase);
            RadarPointer = Memory.Read<int>(RadarBase.Value + 0x6C);
        }
    }
}