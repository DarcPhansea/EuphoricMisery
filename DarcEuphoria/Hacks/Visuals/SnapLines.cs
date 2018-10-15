using DarcEuphoria.Euphoric;
using DarcEuphoria.Euphoric.Structs;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;

namespace DarcEuphoria.Hacks.Visuals
{
    public static class SnapLines
    {
        public static void Start(RenderTarget Device, Vector2 position, RawColor4 color)
        {
            using (var brush = new SolidColorBrush(Device, color))
            {
                var vec1 = new RawVector2(GlobalVariables.ScreenSize.Width / 2f,
                    GlobalVariables.ScreenSize.Height);

                Device.DrawLine(vec1, position.ToRawVector2(), brush);
            }
        }
    }
}