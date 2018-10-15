using System;
using System.Drawing;
using DarcEuphoria.Euphoric.Structs;

namespace DarcEuphoria.Euphoric
{
    public static class Maths
    {
        public static double CalculateFOV(Vector2 src, Vector2 dest)
        {
            var i = Math.Sqrt(
                (dest.X - src.X) * (dest.X - src.X) +
                (dest.Y - src.Y) * (dest.Y - src.Y)
            );
            return i;
        }

        public static Vector2 CalcAngle(Vector3 src, Vector3 dist)
        {
            var delta = new Vector3
            {
                X = dist.X - src.X,
                Y = dist.Y - src.Y,
                Z = dist.Z - src.Z
            };

            var magn = (float) Math.Sqrt(
                delta.X * delta.X +
                delta.Y * delta.Y +
                delta.Z * delta.Z
            );

            var returnAngle = new Vector2
            {
                X = (float) (Math.Atan2(delta.Y, delta.X) * (180f / 3.14f)),
                Y = (float) (-Math.Atan2(delta.Z, magn) * (180f / 3.14f))
            };

            return returnAngle;
        }

        public static Vector2 Normalize(this Vector2 angle)
        {
            while (0f > angle.X || angle.X > 360f)
            {
                if (angle.X < 0f) angle.X += 360.0f;
                if (angle.X > 360f) angle.X -= 360.0f;
            }

            return angle;
        }

        public static Vector2 ClampAngle(this Vector2 angle)
        {
            while (angle.X > 180f)
                angle.X -= 360f;

            while (angle.X < -180f)
                angle.X += 360f;

            // while (angle.Y > 89f)
            //     angle.Y -= (angle.Y - 89) * 2;

            //while (angle.Y < -89f)
            //    angle.Y += (-89 - angle.Y) * 2;

            while (angle.Y > 89.0f)
                angle.Y -= 89.0f;

            while (angle.Y < -89.0f)
                angle.Y += 89.0f;


            return angle;
        }

        public static float Vector3Distance(Vector3 src, Vector3 dest, bool noZ = false)
        {
            if (!noZ)
            {
                var distance = Math.Sqrt(
                    (dest.X - src.X) * (dest.X - src.X) +
                    (dest.Y - src.Y) * (dest.Y - src.Y) +
                    (dest.Z - src.Z) * (dest.Z - src.Z)
                );
                distance = Math.Round(distance, 4);
                return (float) distance;
            }
            else
            {
                var distance = Math.Sqrt(
                    (dest.X - src.X) * (dest.X - src.X) +
                    (dest.Y - src.Y) * (dest.Y - src.Y)
                );

                distance = Math.Round(distance, 4);
                return (float) distance;
            }
        }

        public static Vector2 LocationToPlayer(Vector3 src, Vector3 dist)
        {
            var vec = new Vector2(src.X - dist.X, src.Y - dist.Y);
            return vec;
        }

        public static Point RotatePoint(Point pointToRotate, Point center, float angle, bool angleInRadians = false)
        {
            if (!angleInRadians)
                angle = (float) (angle * (Math.PI / 180f));
            var cosTheta = (float) Math.Cos(angle);
            var sinTheta = (float) Math.Sin(angle);
            var returnVec = new Point(
                (int) (cosTheta * (pointToRotate.X - center.X) - sinTheta * (pointToRotate.Y - center.Y)),
                (int) (sinTheta * (pointToRotate.X - center.X) + cosTheta * (pointToRotate.Y - center.Y))
            );
            returnVec.X += center.X;
            returnVec.Y += center.Y;
            return returnVec;
        }

        public static bool IsKeyDown(this int T)
        {
            if (T == 0) return true;

            if (WinApi.GetAsyncKeyState(T) > 0) return true;
            if ((WinApi.GetAsyncKeyState(T) & 0x1) > 0) return true;
            if ((WinApi.GetAsyncKeyState(T) & 0x8000) > 0) return true;

            return false;
        }

        public static PointF sCoord(float center)
        {
            var returnCoord = new PointF();
            var val = DateTime.Now.Second * 6 + (int) (DateTime.Now.Millisecond * .006f);

            if (val >= 0 && val <= 180)
            {
                returnCoord.X = center + (float) (70 * Math.Sin(Math.PI * val / 180));
                returnCoord.Y = center - (float) (70 * Math.Cos(Math.PI * val / 180));
            }
            else
            {
                returnCoord.X = center - (float) (70 * -Math.Sin(Math.PI * val / 180));
                returnCoord.Y = center - (float) (70 * Math.Cos(Math.PI * val / 180));
            }

            return returnCoord;
        }

        public static PointF mCoord(float center)
        {
            var returnCoord = new PointF();
            var val = DateTime.Now.Minute * 6 + (int) (DateTime.Now.Second * .1f);

            if (val >= 0 && val <= 180)
            {
                returnCoord.X = center + (float) (70 * Math.Sin(Math.PI * val / 180));
                returnCoord.Y = center - (float) (70 * Math.Cos(Math.PI * val / 180));
            }
            else
            {
                returnCoord.X = center - (float) (70 * -Math.Sin(Math.PI * val / 180));
                returnCoord.Y = center - (float) (70 * Math.Cos(Math.PI * val / 180));
            }

            return returnCoord;
        }

        public static PointF hrCoord(float center)
        {
            var returnCoord = new PointF();

            var val = (int) (DateTime.Now.Hour * 30 + DateTime.Now.Minute * 0.5);

            if (val >= 0 && val <= 180)
            {
                returnCoord.X = center + (float) (50 * Math.Sin(Math.PI * val / 180));
                returnCoord.Y = center - (float) (50 * Math.Cos(Math.PI * val / 180));
            }
            else
            {
                returnCoord.X = center - (float) (50 * -Math.Sin(Math.PI * val / 180));
                returnCoord.Y = center - (float) (50 * Math.Cos(Math.PI * val / 180));
            }

            return returnCoord;
        }
    }
}