using System.Drawing;
using System.Windows.Forms;
using DarcEuphoria.Euphoric.Structs;

namespace DarcEuphoria.Euphoric.Controls
{
    internal class DeTextBox : TextBox
    {
        public DeTextBox()
        {
            BorderStyle = BorderStyle.None;
            AutoSize = false;
            Multiline = true;
            SetPadding(this, new Padding(8, 3, 8, 3));
            Refresh();
        }

        public void SetPadding(TextBox textBox, Padding padding)
        {
            var rect = new Rectangle(padding.Left, padding.Top, textBox.ClientSize.Width - padding.Left - padding.Right,
                textBox.ClientSize.Height - padding.Top - padding.Bottom);
            var rc = new RECT(rect);
            WinApi.SendMessageRefRect(Handle, 0xB3, 0, ref rc);
        }

        public override void Refresh()
        {
            base.Refresh();
            BackColor = GlobalVariables.SecondaryBackColor;
            ForeColor = GlobalVariables.PrimaryTextColor;
        }
    }
}