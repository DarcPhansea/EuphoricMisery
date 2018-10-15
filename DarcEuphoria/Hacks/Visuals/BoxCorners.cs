using DarcEuphoria.Euphoric.Structs;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;

namespace DarcEuphoria.Hacks.Visuals
{
    public static class BoxCorners
    {
        public static void Start(RenderTarget Device, DrawArea drawArea, RawColor4 color, int strokeWidth = 1)
        {
            var len = (int) (drawArea.Width / 3.5);
            var p1 = new RawVector2(drawArea.Left, drawArea.Top);
            var p2 = new RawVector2(drawArea.Left + drawArea.Width, drawArea.Top);
            var p3 = new RawVector2(drawArea.Left, drawArea.Top + drawArea.Height);
            var p4 = new RawVector2(p2.X, p3.Y);
            var p11 = new RawVector2(p1.X + len, p1.Y);
            var p12 = new RawVector2(p1.X, p1.Y + len);
            var p21 = new RawVector2(p2.X - len, p2.Y);
            var p22 = new RawVector2(p2.X, p2.Y + len);
            var p31 = new RawVector2(p3.X + len, p3.Y);
            var p32 = new RawVector2(p3.X, p3.Y - len);
            var p41 = new RawVector2(p4.X - len, p4.Y);
            var p42 = new RawVector2(p4.X, p4.Y - len);

            using (var brush = new SolidColorBrush(Device, color))
            {
                Device.DrawLine(p11, p1, brush, strokeWidth);
                Device.DrawLine(p1, p12, brush, strokeWidth);
                Device.DrawLine(p21, p2, brush, strokeWidth);
                Device.DrawLine(p2, p22, brush, strokeWidth);
                Device.DrawLine(p31, p3, brush, strokeWidth);
                Device.DrawLine(p3, p32, brush, strokeWidth);
                Device.DrawLine(p41, p4, brush, strokeWidth);
                Device.DrawLine(p4, p42, brush, strokeWidth);
            }
        }
    }
}