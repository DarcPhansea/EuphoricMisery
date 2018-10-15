using System.Windows.Forms;
using DarcEuphoria.Euphoric;
using DarcEuphoria.Euphoric.Configs.Structs;
using DarcEuphoria.Euphoric.CSGO;
using DarcEuphoria.Euphoric.CSGO.Entity;
using DarcEuphoria.Euphoric.Structs;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.Mathematics.Interop;

namespace DarcEuphoria.Hacks.Visuals
{
    public static class ItemsLoop
    {
        private static TextFormat txtForm;

        public static void Start(RenderTarget Device, VisualSettings visSettings, Matrix4x4 matrix4X4)
        {
            foreach (var item in GlobalVariables.EntityList)
            {
                //Glow.Start(item, new RawColor4(1, 1, 1, 1));

                if (item.ClassID == 108)
                {
                    if (GlobalVariables.ActiveSettings.MiscSettings.C4Countdown)
                        DrawBombTime(Device, item);

                    if (!visSettings.wPlantedC4) continue;

                    if (item.Position.Value.ToScreen(matrix4X4).X == -1f &&
                        item.Position.Value.ToScreen(matrix4X4).Y == -1f) continue;

                    goto BOMB;
                }

                if (item.Position.Value.ToScreen(matrix4X4).X == -1f &&
                    item.Position.Value.ToScreen(matrix4X4).Y == -1f) continue;
                

                if (item.IsWeapon() && !visSettings.wWeapon) continue;
                if (item.ClassID == 29 && !visSettings.wDroppedC4) continue;
                if (item.IsDefuser() && !visSettings.wDefuser) continue;
                if (item.ClassID == 31 && !visSettings.wChicken) continue;
                if (item.ClassID == 83 && !visSettings.wHostage) continue;
                if (item.IsGrenade() && !visSettings.wGrenades) continue;

                if (item.IsWeapon())
                {
                    ItemESP.Start(Device,
                        item.Position.Value.ToScreen(matrix4X4),
                        item.WeaponName, new RawColor4(1, 1, 1, 1));

                    continue;
                }

                BOMB:
                ItemESP.Start(Device,
                    item.Position.Value.ToScreen(matrix4X4),
                    item.ClassName, new RawColor4(1, 1, 1, 1));
            }
        }

        public static void Start(RenderTarget Device)
        {
            foreach (var item in GlobalVariables.EntityList)
                if (item.ClassID == 108)
                    DrawBombTime(Device, item);
        }

        private static void DrawBombTime(RenderTarget Device, BaseWeapon item)
        {
            txtForm = GlobalVariables.textFormat;
            txtForm.WordWrapping = WordWrapping.NoWrap;
            txtForm.TextAlignment = TextAlignment.Center;

            var timeLeft = item.BombTime.Value - CSGOEngine.csClient.GlobalVarsBase.Value.curtime;
            if (timeLeft < 0) timeLeft = 0;

            var center = GlobalVariables.ScreenSize.Width / 2f;
            var rect = new RawRectangleF(center, 0, center + 1, 1);

            VMaths.DrawOutline(Device, timeLeft.ToString("0.00"), rect, txtForm);

            using (var brush = new SolidColorBrush(Device, timeLeft.BombToColor()))
            {
                Device.DrawText(timeLeft.ToString("0.00"), txtForm, rect, brush,
                    DrawTextOptions.EnableColorFont |
                    DrawTextOptions.DisableColorBitmapSnapping |
                    DrawTextOptions.NoSnap);
            }
        }
    }
}