using System.Drawing;
using DarcEuphoria.Euphoric;
using DarcEuphoria.Euphoric.Structs;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.Mathematics.Interop;
using Factory = SharpDX.DirectWrite.Factory;
using FontStyle = SharpDX.DirectWrite.FontStyle;

namespace DarcEuphoria.Hacks.Visuals
{
    public static class VMaths
    {
        public static TextFormat txtForm = new TextFormat(new Factory(),
            "Bahnschrift SemiCondensed", FontWeight.Light, FontStyle.Normal, 11f);

        public static Vector2 ToScreen(this Vector3 position, Matrix4x4 matrix)
        {
            var fltTmp = matrix.M41 * position.X +
                         matrix.M42 * position.Y +
                         matrix.M43 * position.Z +
                         matrix.M44;

            if (fltTmp < 0.01f)
                return new Vector2(-1f, -1f);

            var inverseFltTmp = 1f / fltTmp;

            return new Vector2
            {
                X = GlobalVariables.ScreenSize.Width / 2f +
                    (0.5f * (
                              (matrix.M11 * position.X +
                               matrix.M12 * position.Y +
                               matrix.M13 * position.Z +
                               matrix.M14)
                              * inverseFltTmp)
                          * GlobalVariables.ScreenSize.Width + 0.5f),

                Y = GlobalVariables.ScreenSize.Height / 2f -
                    (0.5f * (
                              (matrix.M21 * position.X +
                               matrix.M22 * position.Y +
                               matrix.M23 * position.Z +
                               matrix.M24)
                              * inverseFltTmp)
                          * GlobalVariables.ScreenSize.Height + 0.5f)
            };
        }

        public static void DrawOutline(RenderTarget Device, string text, RawRectangleF rect)
        {
            using (var b = new SolidColorBrush(Device, new RawColor4(1 / 255f, 1 / 255f, 1 / 255f, 1)))
            {
                Device.DrawText(text, txtForm,
                    new RawRectangleF(
                        rect.Left - 1,
                        rect.Top - 1,
                        rect.Right - 1,
                        rect.Bottom - 1
                    ), b, DrawTextOptions.NoSnap);

                Device.DrawText(text, txtForm,
                    new RawRectangleF(
                        rect.Left + 1,
                        rect.Top - 1,
                        rect.Right + 1,
                        rect.Bottom - 1
                    ), b, DrawTextOptions.NoSnap);

                Device.DrawText(text, txtForm,
                    new RawRectangleF(
                        rect.Left - 1,
                        rect.Top + 1,
                        rect.Right - 1,
                        rect.Bottom + 1
                    ), b, DrawTextOptions.NoSnap);

                Device.DrawText(text, txtForm,
                    new RawRectangleF(
                        rect.Left + 1,
                        rect.Top + 1,
                        rect.Right + 1,
                        rect.Bottom + 1
                    ), b, DrawTextOptions.NoSnap);
            }
        }

        public static void DrawOutline(RenderTarget Device, string text, RawRectangleF rect, TextFormat txtfrm)
        {
            using (var b = new SolidColorBrush(Device, new RawColor4(1 / 255f, 1 / 255f, 1 / 255f, 1)))
            {
                Device.DrawText(text, txtfrm,
                    new RawRectangleF(
                        rect.Left - 1,
                        rect.Top - 1,
                        rect.Right - 1,
                        rect.Bottom - 1
                    ), b, DrawTextOptions.EnableColorFont |
                          DrawTextOptions.DisableColorBitmapSnapping |
                          DrawTextOptions.NoSnap);

                Device.DrawText(text, txtfrm,
                    new RawRectangleF(
                        rect.Left + 1,
                        rect.Top - 1,
                        rect.Right + 1,
                        rect.Bottom - 1
                    ), b, DrawTextOptions.EnableColorFont |
                          DrawTextOptions.DisableColorBitmapSnapping |
                          DrawTextOptions.NoSnap);

                Device.DrawText(text, txtfrm,
                    new RawRectangleF(
                        rect.Left - 1,
                        rect.Top + 1,
                        rect.Right - 1,
                        rect.Bottom + 1
                    ), b, DrawTextOptions.EnableColorFont |
                          DrawTextOptions.DisableColorBitmapSnapping |
                          DrawTextOptions.NoSnap);

                Device.DrawText(text, txtfrm,
                    new RawRectangleF(
                        rect.Left + 1,
                        rect.Top + 1,
                        rect.Right + 1,
                        rect.Bottom + 1
                    ), b, DrawTextOptions.EnableColorFont |
                          DrawTextOptions.DisableColorBitmapSnapping |
                          DrawTextOptions.NoSnap);
            }
        }

        public static RawColor4 ToRaw(this Color col)
        {
            return new RawColor4(col.R / 255f, col.G / 255f, col.B / 255f, col.A / 255f);
        }

        public static Color ToColor(this RawColor4 col)
        {
            return Color.FromArgb(
                (int) (col.A * 255f),
                (int) (col.R * 255f),
                (int) (col.G * 255f),
                (int) (col.B * 255f));
        }

        public static RawColor4 ToHp(this int health)
        {
            var i = health;
            if (255 - (int) (health * 2.55) < 0)
                i = 0;

            if (255 - (int) (health * 2.55) > 255)
                i = 255;

            if ((int) (health * 2.55) > 255)
                i = 255;

            if ((int) (health * 2.55) < 0)
                i = 0;

            return new RawColor4(
                (float) (255 - i * 2.55) / 255f,
                (float) (i * 2.55) / 255f,
                80f / 255f,
                1
            );
        }

        public static RawColor4 BombToColor(this float id)
        {
            var i = id;
            if (255 - (int) (id * 6.375) < 0)
                i = 0;

            if (255 - (int) (id * 6.375) > 255)
                i = 255;

            if ((int) (id * 6.375) > 255)
                i = 255;

            if ((int) (id * 6.375) < 0)
                i = 0;

            return new RawColor4(
                (float) (255 - i * 6.375) / 255f,
                (float) (i * 6.375) / 255f,
                80f / 255f,
                1
            );
        }

        public static RawRectangleF ToRawRectangleF(this DrawArea drawArea)
        {
            return new RawRectangleF(
                drawArea.Left,
                drawArea.Top,
                drawArea.Left + drawArea.Width,
                drawArea.Top + drawArea.Height);
        }

        public static bool IsVisible(this DrawArea drawArea)
        {
            if (drawArea.Left + drawArea.Width + 20 < 0)
                return false;

            if (drawArea.Top + drawArea.Height + 20 < 0)
                return false;

            if (drawArea.Left - 20 > GlobalVariables.ScreenSize.Width)
                return false;

            if (drawArea.Top - 20 > GlobalVariables.ScreenSize.Height)
                return false;

            return true;
        }

        public static RawVector2 ToRawVector2(this Vector2 vector2)
        {
            return new RawVector2(vector2.X, vector2.Y);
        }
    }
}