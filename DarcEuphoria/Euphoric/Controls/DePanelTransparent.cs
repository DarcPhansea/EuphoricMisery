using System.Drawing;
using System.Windows.Forms;

namespace DarcEuphoria.Euphoric.Controls
{
    internal class DePanelTransparent : Panel
    {
        public DePanelTransparent()
        {
            DoubleBuffered = true;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x20;
                return cp;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            using (var b = new SolidBrush(Color.FromArgb(50, 255, 255, 255)))
            {
                e.Graphics.FillRectangle(b, ClientRectangle);
            }
        }
    }
}