using System.Windows.Forms;

namespace DarcEuphoria.Euphoric.Controls
{
    internal class DePanel : Panel
    {
        public DePanel()
        {
            DoubleBuffered = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            BackColor = GlobalVariables.SecondaryBackColor;
        }
    }
}