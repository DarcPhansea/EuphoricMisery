using System.Drawing;
using System.Windows.Forms;

namespace DarcEuphoria.Euphoric.Controls
{
    internal class DeComboBox : ComboBox
    {
        public DeComboBox()
        {
            FlatStyle = FlatStyle.Flat;

            BackColor = GlobalVariables.SecondaryBackColor;
            ForeColor = GlobalVariables.PrimaryTextColor;
            DropDownStyle = ComboBoxStyle.DropDownList;
            DrawMode = DrawMode.OwnerDrawFixed;
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            try
            {
                BackColor = GlobalVariables.SecondaryBackColor;
                ForeColor = GlobalVariables.PrimaryTextColor;

                var HotLight = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
                var editArea = (e.State & DrawItemState.ComboBoxEdit) == DrawItemState.ComboBoxEdit;

                if (HotLight && !editArea)
                    using (Brush b = new SolidBrush(GlobalVariables.PrimaryForeColor))
                    {
                        e.Graphics.FillRectangle(b, e.Bounds);
                    }
                else
                    using (Brush b = new SolidBrush(GlobalVariables.SecondaryBackColor))
                    {
                        e.Graphics.FillRectangle(b, e.Bounds);
                    }


                using (Brush b = new SolidBrush(GlobalVariables.PrimaryTextColor))
                {
                    e.Graphics.DrawString(Items[e.Index].ToString(), e.Font, b, e.Bounds.X, e.Bounds.Y + 4);
                }
            }
            catch
            {
            }
        }
    }
}