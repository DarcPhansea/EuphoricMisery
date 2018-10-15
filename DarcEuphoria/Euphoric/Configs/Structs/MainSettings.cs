using SharpDX.Mathematics.Interop;

namespace DarcEuphoria.Euphoric.Configs.Structs
{
    public struct MainSettings
    {
        public double ConfigVersion;
        public RawColor4 PrimaryForeColor;
        public RawColor4 PrimaryBackColor;
        public RawColor4 SecondaryBackColor;
        public RawColor4 PrimaryTextColor;
        public AimbotSettings AimbotSettings;
        public TriggerbotSettings TriggerbotSettings;
        public VisualSettings VisualSettings;
        public MiscSettings MiscSettings;
        public SkinChangerSettings SkinSettings;
    }
}