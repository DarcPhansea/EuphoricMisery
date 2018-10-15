using DarcEuphoria.Euphoric.Structs;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;

namespace DarcEuphoria.Hacks.Visuals
{
    public static class ActiveWeapon
    {
        public static void Start(RenderTarget Device, DrawArea drawArea, string activeWeapon, RawColor4 color)
        {
            var rect = new RawRectangleF(
                drawArea.Left,
                drawArea.Top + drawArea.Height,
                drawArea.Left + drawArea.Width,
                drawArea.Top + drawArea.Height + 30);

            VMaths.DrawOutline(Device, activeWeapon, rect);

            using (var b = new SolidColorBrush(Device, color))
            {
                Device.DrawText(activeWeapon, VMaths.txtForm, rect, b, DrawTextOptions.NoSnap);
            }
        }
    }
}