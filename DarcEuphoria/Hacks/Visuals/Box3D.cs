using DarcEuphoria.Euphoric.CSGO.Entity;
using DarcEuphoria.Euphoric.Structs;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;

namespace DarcEuphoria.Hacks.Visuals
{
    public static class Box3D
    {
        public static void Start(RenderTarget Device, BasePlayer player, Matrix4x4 matrix4X4, RawColor4 color,
            int strokeWidth = 1)
        {
            var size = 15;

            var p1 = player.Position.Value;
            var p2 = p1;
            var p3 = p1;
            var p4 = p1;

            p1.X -= size;
            p1.Y -= size;
            p2.X -= size;
            p2.Y += size;
            p3.X += size;
            p3.Y += size;
            p4.X += size;
            p4.Y -= size;

            if (p1.ToScreen(matrix4X4).X == -1f && p1.ToScreen(matrix4X4).Y == -1f) return;
            if (p2.ToScreen(matrix4X4).X == -1f && p2.ToScreen(matrix4X4).Y == -1f) return;
            if (p3.ToScreen(matrix4X4).X == -1f && p3.ToScreen(matrix4X4).Y == -1f) return;
            if (p4.ToScreen(matrix4X4).X == -1f && p4.ToScreen(matrix4X4).Y == -1f) return;

            var headOrigin = player.BonePosition(8);
            headOrigin.Z += 7;

            var p5 = p1;
            var p6 = p2;
            var p7 = p3;
            var p8 = p4;
            p5.Z = headOrigin.Z;
            p6.Z = headOrigin.Z;
            p7.Z = headOrigin.Z;
            p8.Z = headOrigin.Z;


            if (p5.ToScreen(matrix4X4).X == -1f && p5.ToScreen(matrix4X4).Y == -1f) return;
            if (p6.ToScreen(matrix4X4).X == -1f && p6.ToScreen(matrix4X4).Y == -1f) return;
            if (p7.ToScreen(matrix4X4).X == -1f && p7.ToScreen(matrix4X4).Y == -1f) return;
            if (p8.ToScreen(matrix4X4).X == -1f && p8.ToScreen(matrix4X4).Y == -1f) return;

            using (var brush = new SolidColorBrush(Device, color))
            {
                Device.DrawLine(p7.ToScreen(matrix4X4).ToRawVector2(), p8.ToScreen(matrix4X4).ToRawVector2(), brush,
                    strokeWidth);
                Device.DrawLine(p4.ToScreen(matrix4X4).ToRawVector2(), p8.ToScreen(matrix4X4).ToRawVector2(), brush,
                    strokeWidth);
                Device.DrawLine(p3.ToScreen(matrix4X4).ToRawVector2(), p4.ToScreen(matrix4X4).ToRawVector2(), brush,
                    strokeWidth);
                Device.DrawLine(p3.ToScreen(matrix4X4).ToRawVector2(), p7.ToScreen(matrix4X4).ToRawVector2(), brush,
                    strokeWidth);
                Device.DrawLine(p1.ToScreen(matrix4X4).ToRawVector2(), p5.ToScreen(matrix4X4).ToRawVector2(), brush,
                    strokeWidth);
                Device.DrawLine(p6.ToScreen(matrix4X4).ToRawVector2(), p7.ToScreen(matrix4X4).ToRawVector2(), brush,
                    strokeWidth);
                Device.DrawLine(p8.ToScreen(matrix4X4).ToRawVector2(), p5.ToScreen(matrix4X4).ToRawVector2(), brush,
                    strokeWidth);
                Device.DrawLine(p1.ToScreen(matrix4X4).ToRawVector2(), p2.ToScreen(matrix4X4).ToRawVector2(), brush,
                    strokeWidth);
                Device.DrawLine(p4.ToScreen(matrix4X4).ToRawVector2(), p1.ToScreen(matrix4X4).ToRawVector2(), brush,
                    strokeWidth);
                Device.DrawLine(p2.ToScreen(matrix4X4).ToRawVector2(), p3.ToScreen(matrix4X4).ToRawVector2(), brush,
                    strokeWidth);
                Device.DrawLine(p2.ToScreen(matrix4X4).ToRawVector2(), p6.ToScreen(matrix4X4).ToRawVector2(), brush,
                    strokeWidth);
                Device.DrawLine(p5.ToScreen(matrix4X4).ToRawVector2(), p6.ToScreen(matrix4X4).ToRawVector2(), brush,
                    strokeWidth);
            }
        }
    }
}