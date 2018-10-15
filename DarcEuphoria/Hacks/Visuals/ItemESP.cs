using DarcEuphoria.Euphoric;
using DarcEuphoria.Euphoric.Structs;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.Mathematics.Interop;

namespace DarcEuphoria.Hacks.Visuals
{
    public static class ItemESP
    {
        private static TextFormat txtForm;

        public static void Start(RenderTarget device, Vector2 screenPosition, string text, RawColor4 color)
        {
            txtForm = GlobalVariables.textFormat;
            txtForm.WordWrapping = WordWrapping.NoWrap;
            txtForm.TextAlignment = TextAlignment.Center;

            var rect = new RawRectangleF
            {
                Top = screenPosition.Y - 10,
                Bottom = screenPosition.Y,
                Left = screenPosition.X - 10,
                Right = screenPosition.X + 10
            };

            VMaths.DrawOutline(device, text, rect, txtForm);

            using (var brush = new SolidColorBrush(device, color))
            {
                device.DrawText(text, txtForm, rect, brush,
                    DrawTextOptions.EnableColorFont |
                    DrawTextOptions.DisableColorBitmapSnapping |
                    DrawTextOptions.NoSnap);
            }
        }
    }
}