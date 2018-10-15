using System.Drawing;
using DarcEuphoria.Euphoric.Configs.Enums;

namespace DarcEuphoria.Euphoric.Configs.Structs
{
    public struct MiscSettings
    {
        public bool InGameRadar;
        public bool C4Countdown;
        public bool NoPostProcessing;
        public bool RankRevealer;
        public bool ChatSpam;
        public bool ThirdPerson;
        public bool AutoHop;
        public bool ExternalRadar;
        public bool FakeLag;
        public bool ClanChanger;
        public bool NameChanger;
        public double AutoHopChance;
        public double FakeLagAmount;
        public double ExternalRadarSize;
        public double ExternalRadarScale;
        public Point ExternalRadarPosition;
        public double FlashAlpha;
        public double FieldOfView;
        public ClanMode ClanChangerMode;
        public string ClanTag;
        public string NameTag;
        public int ThirdPersonKey;
    }
}