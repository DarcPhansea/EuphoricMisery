using DarcEuphoria.Euphoric.Structs;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;

namespace DarcEuphoria.Hacks.Visuals
{
    public static class Box2D
    {
        public static void Start(RenderTarget Device, DrawArea drawArea, RawColor4 color, int strokeWidth = 1)
        {
            using (var b = new SolidColorBrush(Device, color))
            {
                Device.DrawRectangle(drawArea.ToRawRectangleF(), b, strokeWidth);
            }
        }
    }
}