using System.Threading.Tasks;
using DarcEuphoria.Euphoric;
using DarcEuphoria.Euphoric.Configs.Structs;
using DarcEuphoria.Euphoric.CSGO;
using DarcEuphoria.Euphoric.Structs;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.Mathematics.Interop;
using static DarcEuphoria.Euphoric.CSGO.CSGOEngine;
using TextAntialiasMode = SharpDX.Direct2D1.TextAntialiasMode;

namespace DarcEuphoria.Hacks.Visuals
{
    public class VisualMain
    {
        public static Matrix4x4 matrix4x4;
        public static VisualSettings visSettings;

        private static Task ThreadTask;

        public static void Start(RenderTarget Device)
        {
            if (ThreadTask != null)
                if (!ThreadTask.IsCompleted)
                    return;

            ThreadTask = Task.Factory.StartNew(() =>
            {
                GlobalVariables.Device.BeginDraw();
                GlobalVariables.Device.Clear(new RawColor4(0, 0, 0, 0));
                GlobalVariables.Device.TextAntialiasMode = TextAntialiasMode.Aliased;
                GlobalVariables.Device.AntialiasMode = AntialiasMode.Aliased;

                GlobalVariables.textFormat.TextAlignment = TextAlignment.Leading;

                Device.DrawText(GlobalVariables.CHEATNAME + " v" + GlobalVariables.CHEATVERSION.ToString($"F{1}"),
                    GlobalVariables.textFormat,
                    new RawRectangleF(5, 1, 6, 2),
                    new SolidColorBrush(Device, GlobalVariables.PrimaryForeColor.ToRaw())
                );

                if (!csClient.InGame)
                {
                    GlobalVariables.Device.EndDraw();
                    return;
                }
                
                matrix4x4 = csClient.Matrix4.Value;

                visSettings = GlobalVariables.ActiveSettings.VisualSettings;

                if (!visSettings.Active)
                {
                    if (GlobalVariables.ActiveSettings.MiscSettings.C4Countdown)
                        goto C4COUNTDOWN;

                    GlobalVariables.Device.EndDraw();
                    return;
                }

                if (LocalPlayer.ActiveWeapon.ZoomLevel.Value != 0 &&
                    GlobalVariables.ActiveSettings.VisualSettings.NoScope)
                {
                    LocalPlayer.Scoped.Value = false;

                    var vec1 = new RawVector2(GlobalVariables.ScreenSize.Width / 2f, 0);
                    var vec2 = new RawVector2(GlobalVariables.ScreenSize.Width / 2f, GlobalVariables.ScreenSize.Height);
                    var vec3 = new RawVector2(0, GlobalVariables.ScreenSize.Height / 2f);
                    var vec4 = new RawVector2(GlobalVariables.ScreenSize.Width, GlobalVariables.ScreenSize.Height / 2f);

                    using (var brush = new SolidColorBrush(Device, new RawColor4(0, 0, 1 / 255f, 1)))
                    {
                        Device.DrawLine(vec1, vec2, brush);
                        Device.DrawLine(vec3, vec4, brush);
                    }
                }

                if (GlobalVariables.ActiveSettings.VisualSettings.NoHands)
                    if (LocalPlayer.ModelIndex.Value != 0)
                        LocalPlayer.ModelIndex.Value = 0;

                if (visSettings.Enemy.Enabled ||
                    visSettings.Team.Enabled ||
                    visSettings.Yourself.Enabled)
                    PlayersLoop.Start(Device, visSettings, matrix4x4);

                ItemsLoop.Start(Device, visSettings, matrix4x4);

                if (GlobalVariables.ActiveSettings.VisualSettings.DrawAimbotFov)
                    AimbotFov.Start(Device);

                if (GlobalVariables.ActiveSettings.VisualSettings.DrawRecoil ||
                    GlobalVariables.ActiveSettings.VisualSettings.DrawSniper &&
                    LocalPlayer.ActiveWeapon.IsSniper())
                    Crosshair.Start(Device);

                GlobalVariables.Device.EndDraw();
                return;

                C4COUNTDOWN:

                ItemsLoop.Start(Device);
                GlobalVariables.Device.EndDraw();
            });
        }
    }
}