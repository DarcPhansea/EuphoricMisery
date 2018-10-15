using System;
using DarcEuphoria.Euphoric.Enums;
using DarcEuphoria.Euphoric.Structs;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;

namespace DarcEuphoria.Hacks.Visuals
{
    public static class Rank
    {
        public static void Start(RenderTarget Device, DrawArea drawArea, Ranks Rank, RawColor4 color)
        {
            var rect = new RawRectangleF
            {
                Left = drawArea.Left,
                Top = drawArea.Top - 15,
                Right = drawArea.Left + drawArea.Width,
                Bottom = drawArea.Top
            };

            VMaths.DrawOutline(Device, Enum.GetName(typeof(Ranks), Rank), rect);

            using (var b = new SolidColorBrush(Device, color))
            {
                Device.DrawText(Enum.GetName(typeof(Ranks), Rank), VMaths.txtForm, rect, b, DrawTextOptions.NoSnap);
            }
        }
    }
}