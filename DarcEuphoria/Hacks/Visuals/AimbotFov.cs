using System;
using DarcEuphoria.Euphoric;
using DarcEuphoria.Euphoric.CSGO;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;

namespace DarcEuphoria.Hacks.Visuals
{
    public static class AimbotFov
    {
        public static void Start(RenderTarget Device)
        {
            var radiusAngle = CSGOEngine.LocalPlayer.FOV.Value * (3.14f / 180f);
            var radiusHorizontalFov = 2 * Math.Atan(Math.Tan(radiusAngle / 2f) * GlobalVariables.AspectRatio);
            var hFov = radiusHorizontalFov * (180f / 3.14f);

            var percentage = GlobalVariables.ScreenSize.Width / hFov;

            var radius = (float) (Aimbot.WeaponCfg.Fov * percentage);

            var calculation = Math.Sqrt(
                GlobalVariables.ScreenSize.Width / 2f * (GlobalVariables.ScreenSize.Width / 2f) +
                GlobalVariables.ScreenSize.Height / 2f * (GlobalVariables.ScreenSize.Height / 2f));

            if (radius < calculation)
            {
                var rcsPunchVec = CSGOEngine.LocalPlayer.AimPunchAngle.Value;
                var x = GlobalVariables.ScreenSize.Width / 2f;
                var y = GlobalVariables.ScreenSize.Height / 2f;
                var dx = GlobalVariables.ScreenSize.Width / hFov;
                var dy = GlobalVariables.ScreenSize.Height / CSGOEngine.LocalPlayer.FOV.Value;
                x -= (int) (dx * rcsPunchVec.X);
                y += (int) (dy * rcsPunchVec.Y);

                using (var Brush = new SolidColorBrush(Device, new RawColor4(1, 1, 1, 1)))
                {
                    Device.DrawEllipse(
                        new Ellipse(
                            new RawVector2(x, y),
                            radius,
                            radius),
                        Brush);
                }
            }
        }
    }
}