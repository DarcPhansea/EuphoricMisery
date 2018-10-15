using DarcEuphoria.Euphoric;
using DarcEuphoria.Euphoric.Configs.Enums;
using DarcEuphoria.Euphoric.CSGO;
using DarcEuphoria.Euphoric.CSGO.Entity;
using DarcEuphoria.Euphoric.Structs;
using SharpDX.Mathematics.Interop;

namespace DarcEuphoria.Hacks.Visuals
{
    public static class Glow
    {
        public static void Start(BaseEntity player, GlowMode glowMode, RawColor4 color)
        {
            GlowSettings glowSettings;

            glowSettings =
                new GlowSettings(glowMode == GlowMode.FullBloom ? true : false);

            Memory.Write(
                CSGOEngine.csClient.GlowObjectManager.Value + player.GlowIndex.Value * 0x38 + 0x4,
                color);

            Memory.Write(CSGOEngine.csClient.GlowObjectManager.Value + player.GlowIndex.Value * 0x38 + 0x24,
                glowSettings);

            if (glowMode == GlowMode.PseudoChams)
                Memory.Write(CSGOEngine.csClient.GlowObjectManager.Value + player.GlowIndex.Value * 0x38 + 0x2C, 1);
        }
    }
}