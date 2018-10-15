using System;
using System.Drawing;
using System.Windows.Forms;
using DarcEuphoria.Euphoric.CSGO;

namespace DarcEuphoria.Euphoric.Controls
{
    internal class DeRadar : Panel
    {
        private readonly Timer time = new Timer();
        private Point MouseDownLocation;

        public DeRadar()
        {
            DoubleBuffered = true;
            time.Tick += Time_Tick;
            time.Enabled = true;
            time.Interval = 1;
            time.Start();
        }

        private void Time_Tick(object sender, EventArgs e)
        {
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button == MouseButtons.Left)
                MouseDownLocation = e.Location;

            BringToFront();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            //if (!GlobalVariables.InMenu) return;

            if (e.Button == MouseButtons.Left)
            {
                Left = e.X + Left - MouseDownLocation.X;
                Top = e.Y + Top - MouseDownLocation.Y;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var center = new Point(Width / 2, Height / 2);

            using (Brush b = new SolidBrush(GlobalVariables.SecondaryBackColor))
            {
                e.Graphics.FillRectangle(b,
                    ClientRectangle);
            }

            using (var b = new Pen(GlobalVariables.PrimaryBackColor))
            {
                e.Graphics.DrawLine(b,
                    2,
                    ClientRectangle.Height / 2,
                    ClientRectangle.Width - 4,
                    ClientRectangle.Height / 2);

                e.Graphics.DrawLine(b,
                    ClientRectangle.Width / 2,
                    2,
                    ClientRectangle.Width / 2,
                    ClientRectangle.Height - 4);

                e.Graphics.DrawLine(b,
                    2,
                    2,
                    ClientRectangle.Width / 2,
                    ClientRectangle.Height / 2);

                e.Graphics.DrawLine(b,
                    ClientRectangle.Width - 3,
                    2,
                    ClientRectangle.Width / 2,
                    ClientRectangle.Height / 2);
            }

            try
            {
                if (CSGOEngine.csClient.InGame &&
                    GlobalVariables.ActiveSettings.MiscSettings.ExternalRadar)
                    foreach (var player in GlobalVariables.PlayerList)
                    {
                        if (player.IsDormant.Value) continue;
                        if (player.IsLocalPlayer()) continue;
                        if (player.Health.Value <= 0) continue;

                        var dist = Maths.LocationToPlayer(
                                       CSGOEngine.LocalPlayer.Position.Value,
                                       player.Position.Value) *
                                   (float) (GlobalVariables.ActiveSettings.MiscSettings.ExternalRadarScale / 200f);

                        dist.X += center.X;
                        dist.Y += center.Y;

                        Brush b = player.IsSameTeam() ? new SolidBrush(Color.LimeGreen) : new SolidBrush(Color.Red);

                        var coord = Maths.RotatePoint(
                            new Point((int) dist.Y, (int) dist.X), center,
                            CSGOEngine.LocalPlayer.ViewAngles.X);

                        e.Graphics.FillRectangle(b, coord.X - 2, coord.Y - 2, 5, 5);

                        b.Dispose();
                    }
            }
            catch
            {
            }

            using (var b = new Pen(GlobalVariables.PrimaryForeColor, 3))
            {
                e.Graphics.DrawRectangle(b,
                    ClientRectangle.X,
                    ClientRectangle.Y,
                    ClientRectangle.Width - 1,
                    ClientRectangle.Height - 1);
            }
        }
    }
}