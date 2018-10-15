using System.Drawing;
using System.Windows.Forms;

namespace DarcEuphoria.Euphoric.Controls
{
    internal class DeRadioButton : RadioButton
    {
        private SizeF FontSize;

        public DeRadioButton()
        {
            DoubleBuffered = true;
            Appearance = Appearance.Button;
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            FontSize = CreateGraphics().MeasureString(Text, Font);

            if (Checked)
                e.Graphics.FillRectangle(new SolidBrush(GlobalVariables.PrimaryForeColor), ClientRectangle);
            else
                e.Graphics.FillRectangle(new SolidBrush(GlobalVariables.SecondaryBackColor), ClientRectangle);

            using (Brush b = new SolidBrush(GlobalVariables.PrimaryTextColor))
            {
                var pos = (int) (Width / 2 - FontSize.Width / 2 + 2);
                var pos2 = (int) (Height / 2 - FontSize.Height / 2);
                e.Graphics.DrawString(Text, Font, b, pos - 3, pos2 + 3);
            }
        }
    }
}