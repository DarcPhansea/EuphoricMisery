using DarcEuphoria.Euphoric.Structs;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;

namespace DarcEuphoria.Hacks.Visuals
{
    public static class Health
    {
        public static void Start(RenderTarget Device, DrawArea drawArea, int Health)
        {
            var x = drawArea.Left - 5;
            var y1 = drawArea.Top;
            var y2 = drawArea.Top + drawArea.Height;
            var y3 = drawArea.Top + drawArea.Height / 100f * (Health < 100f ? Health : 100f);

            using (var Brush = new SolidColorBrush(Device, new RawColor4(0.1f, 0.1f, 0.1f, 1f)))
            {
                Device.DrawLine(new RawVector2(x, y1), new RawVector2(x, y2), Brush, 4);
            }

            using (var Brush = new SolidColorBrush(Device, (Health < 100 ? Health : 100).ToHp()))
            {
                Device.DrawLine(new RawVector2(x, y1 + 1), new RawVector2(x, y3 - 1), Brush, 2);
            }
        }

        public static void NumberStart(RenderTarget Device, DrawArea drawArea, int Health)
        {
            var rect = new RawRectangleF
            {
                Left = drawArea.Left - 31,
                Top = drawArea.Top,
                Right = drawArea.Left - 5,
                Bottom = drawArea.Top + 10
            };

            VMaths.DrawOutline(Device, Health + "%", rect);

            using (var b = new SolidColorBrush(Device, new RawColor4(1, 1, 1, 1)))
            {
                Device.DrawText(Health + "%", VMaths.txtForm, rect, b, DrawTextOptions.NoSnap);
            }
        }
    }
}