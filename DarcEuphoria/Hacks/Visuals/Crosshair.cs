using System;
using DarcEuphoria.Euphoric;
using DarcEuphoria.Euphoric.CSGO;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;

namespace DarcEuphoria.Hacks.Visuals
{
    public static class Crosshair
    {
        public static void Start(RenderTarget Device)
        {
            if (!GlobalVariables.ActiveSettings.VisualSettings.DrawSniper &&
                CSGOEngine.LocalPlayer.ActiveWeapon.IsSniper()) return;

            var radAngle = CSGOEngine.LocalPlayer.FOV.Value * (3.14f / 180f);
            var radHFov = 2 * Math.Atan(Math.Tan(radAngle / 2f) * GlobalVariables.AspectRatio);
            var hFov = radHFov * (180f / 3.14f);

            var rcsPunchVec = CSGOEngine.LocalPlayer.AimPunchAngle.Value;

            var x = GlobalVariables.ScreenSize.Width / 2f;
            var y = GlobalVariables.ScreenSize.Height / 2f;
            var dx = GlobalVariables.ScreenSize.Width / hFov;
            var dy = GlobalVariables.ScreenSize.Height / CSGOEngine.LocalPlayer.FOV.Value;

            x -= (int) (dx * rcsPunchVec.X);
            y += (int) (dy * rcsPunchVec.Y);
            var point = new RawVector2(x + 1, y + 1);
            var p1 = point;
            var p2 = point;
            var p3 = point;
            var p4 = point;
            p1.X -= 6;
            p2.X += 5;
            p3.Y -= 6;
            p4.Y += 5;

            using (var Brush = new SolidColorBrush(Device, new RawColor4(1, 1, 1, 1)))
            {
                Device.DrawLine(p1, p2, Brush, 1);
                Device.DrawLine(p3, p4, Brush, 1);
            }
        }
    }
}