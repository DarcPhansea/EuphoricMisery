using System.Diagnostics;
using System.Drawing;
using DarcEuphoria.Euphoric.Configs.Structs;
using DarcEuphoria.Euphoric.CSGO.Entity;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using Factory = SharpDX.DirectWrite.Factory;
using FontStyle = SharpDX.DirectWrite.FontStyle;

namespace DarcEuphoria.Euphoric
{
    public static class GlobalVariables
    {
        public const double CHEATVERSION = 1.0;
        public const string CHEATNAME = "Euphoric 悲惨";

        public const bool UseBetaThirdPerson = false;
        public static int GlobalRefresh = 0;

        public static MainSettings ActiveSettings;

        public static Size ScreenSize;

        public static WindowRenderTarget Device = null;

        public static TextFormat textFormat = new TextFormat(new Factory(),
            "Bahnschrift SemiBold", FontWeight.Medium, FontStyle.Normal, 17f);

        public static Process CSGO = null;
        public static bool InMenu = false;
        public static bool IsActive = true;
        public static bool SHUTDOWN = false;

        public static Allocator Allocator = new Allocator();

        public static BasePlayer[] PlayerList;
        public static BaseWeapon[] EntityList;


        public static double AspectRatio => ScreenSize.Width / (double) ScreenSize.Height;

        #region Colors

        public static Color PrimaryForeColor = Color.FromArgb(100, 0, 200);
        public static Color PrimaryBackColor = Color.FromArgb(45, 45, 45);
        public static Color SecondaryBackColor = Color.FromArgb(26, 26, 26);
        public static Color PrimaryTextColor = Color.White;

        #endregion
    }
}