using System.Drawing;
using System.Windows.Forms;

namespace DarcEuphoria.Euphoric.Controls
{
    internal class DeMenu : Panel
    {
        private Point MouseDownLocation;

        public DeMenu()
        {
            DoubleBuffered = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            BackColor = GlobalVariables.SecondaryBackColor;
            using (Brush b = new SolidBrush(GlobalVariables.PrimaryForeColor))
            {
                e.Graphics.FillRectangle(b, 4, 4, Width - 8, 21);
            }

            using (Brush b = new SolidBrush(GlobalVariables.PrimaryTextColor))
            {
                var textSize = e.Graphics.MeasureString(
                    GlobalVariables.CHEATNAME + "  v" + GlobalVariables.CHEATVERSION.ToString($"F{1}"),
                    Font);
                var pos = (int) (Width / 2 - textSize.Width / 2);
                e.Graphics.DrawString(
                    GlobalVariables.CHEATNAME + "  v" + GlobalVariables.CHEATVERSION.ToString($"F{1}"), Font, b, pos,
                    8);
            }
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

            if (e.Location.Y > 30) return;

            if (e.Button == MouseButtons.Left)
            {
                Left = e.X + Left - MouseDownLocation.X;
                Top = e.Y + Top - MouseDownLocation.Y;
            }
        }
    }
}