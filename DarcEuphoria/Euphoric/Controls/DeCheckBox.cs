using System.Drawing;
using System.Windows.Forms;

namespace DarcEuphoria.Euphoric.Controls
{
    internal class DeCheckBox : CheckBox
    {
        public DeCheckBox()
        {
            Size = new Size(80, 30);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            using (Brush b = new SolidBrush(GlobalVariables.PrimaryBackColor))
            {
                e.Graphics.FillRectangle(b, 0, 0, Width, Height);
            }

            using (Brush b = new SolidBrush(GlobalVariables.PrimaryTextColor))
            {
                e.Graphics.DrawString(Text, Font, b, 20, 8);
            }

            using (Brush b = new SolidBrush(GlobalVariables.SecondaryBackColor))
            {
                e.Graphics.FillRectangle(b, 0, 6, 18, 18);
            }

            if (!Checked) return;

            using (Brush b = new SolidBrush(GlobalVariables.PrimaryForeColor))
            {
                e.Graphics.FillRectangle(b, 2, 8, 14, 14);
            }
        }
    }
}