using System.Drawing;
using System.Windows.Forms;

namespace DarcEuphoria.Euphoric.Controls
{
    internal class DeGroupBox : GroupBox
    {
        public enum deHeaderPosition
        {
            Left,
            Middle,
            Right
        }

        private SizeF FontSize;

        public deHeaderPosition HeaderPosition = deHeaderPosition.Left;

        protected override void OnPaint(PaintEventArgs e)
        {
            FontSize = CreateGraphics().MeasureString(Text, Font);
            using (var p = new Pen(GlobalVariables.PrimaryTextColor))
            {
                e.Graphics.DrawRectangle(p, 0, FontSize.Height / 2, Width - 1, Height - FontSize.Height / 2 - 1);
            }

            using (Brush b = new SolidBrush(GlobalVariables.PrimaryTextColor))
            {
                var pos = (int) (Width / 2 - FontSize.Width / 2);

                using (Brush bb = new SolidBrush(GlobalVariables.PrimaryBackColor))
                {
                    e.Graphics.FillRectangle(bb, pos, 0, FontSize.Width - 2, FontSize.Height);
                }

                e.Graphics.DrawString(Text, Font, b, pos, 0);
            }
        }
    }
}