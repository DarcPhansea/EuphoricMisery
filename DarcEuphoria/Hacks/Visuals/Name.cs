using DarcEuphoria.Euphoric.Structs;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;

namespace DarcEuphoria.Hacks.Visuals
{
    public static class Name
    {
        public static void Start(RenderTarget Device, DrawArea drawArea, string Name, RawColor4 color,
            bool rankshown = false)
        {
            var rect = new RawRectangleF
            {
                Left = drawArea.Left,
                Top = drawArea.Top - (rankshown ? 25 : 15),
                Right = drawArea.Left + drawArea.Width,
                Bottom = drawArea.Top
            };

            VMaths.DrawOutline(Device, Name, rect);

            using (var b = new SolidColorBrush(Device, color))
            {
                Device.DrawText(Name, VMaths.txtForm, rect, b, DrawTextOptions.NoSnap);
            }
        }
    }
}