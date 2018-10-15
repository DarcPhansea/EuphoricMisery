using DarcEuphoria.Euphoric.Configs.Enums;
using SharpDX.Mathematics.Interop;

namespace DarcEuphoria.Euphoric.Configs.Structs
{
    public struct VisualsPlayer
    {
        public bool Enabled;
        public bool VisableOnly;

        public BoxMode BoxMode;
        public bool BoxOutline;

        public GlowMode GlowMode;

        public HealthMode HealthMode;

        public bool Name;
        public bool Weapon;
        public bool Rank;

        public bool HeadSpot;

        public bool Snaplines;

        public RawColor4 vBoxColor;
        public RawColor4 vGlowColor;
        public RawColor4 vTextColor;
        public RawColor4 vHeadSpotColor;
        public RawColor4 vSnapLinesColor;

        public RawColor4 hBoxColor;
        public RawColor4 hGlowColor;
        public RawColor4 hTextColor;
        public RawColor4 hHeadSpotColor;
        public RawColor4 hSnapLinesColor;
    }
}