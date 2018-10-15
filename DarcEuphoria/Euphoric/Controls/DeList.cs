using System.Drawing;
using System.Windows.Forms;

namespace DarcEuphoria.Euphoric.Controls
{
    public class DeList : ListBox
    {
        public DeList()
        {
            DrawMode = DrawMode.OwnerDrawFixed;
            BorderStyle = BorderStyle.None;
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            try
            {
                BackColor = GlobalVariables.SecondaryBackColor;
                var selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
                using (Brush b =
                    new SolidBrush(selected ? GlobalVariables.PrimaryForeColor : GlobalVariables.SecondaryBackColor))
                {
                    e.Graphics.FillRectangle(b, e.Bounds);
                }

                using (Brush b = new SolidBrush(GlobalVariables.PrimaryTextColor))
                {
                    e.Graphics.DrawString(Items[e.Index].ToString(), Font, b, e.Bounds.X, e.Bounds.Y);
                }
            }
            catch
            {
            }
        }
    }
}