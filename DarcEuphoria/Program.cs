using System;
using System.Windows.Forms;
using DarcEuphoria.Euphoric;

namespace DarcEuphoria
{
    public class EntryPoint
    {
        [DllExport]
        public static void DllMain()
        {
            Program.Main();
        }

        public EntryPoint()
        {
            
        }

    }

    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Application.ApplicationExit += Application_ApplicationExit;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new WndOverlay());
        }

        private static void Application_ApplicationExit(object sender, EventArgs e)
        {
            Memory.CloseCheat();
        }
    }
}