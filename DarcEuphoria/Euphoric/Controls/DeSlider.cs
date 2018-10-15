using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DarcEuphoria.Euphoric.Controls
{
    internal class DeSlider : Panel
    {
        public double _Maximum = 100;
        public double _Minimum;
        public bool _Round = true;
        public int _RoundPlaces = 1;
        public double _Value = 50;

        public DeSlider()
        {
            Size = new Size(100, 15);
            DoubleBuffered = true;
            BackColor = Color.Transparent;
            ForeColor = Color.Transparent;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.Button == MouseButtons.Left)
            {
                var curPos = PointToClient(Cursor.Position);
                var value = _Minimum + (_Maximum - _Minimum) * curPos.X / Width;

                if (value < _Minimum)
                    value = _Minimum;

                if (value > _Maximum)
                    value = _Maximum;

                if (_Round) _Value = Math.Round(value);
                else _Value = value;

                Refresh();
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                var curPos = PointToClient(Cursor.Position);
                var value = _Minimum + (_Maximum - _Minimum) * curPos.X / Width;

                if (value < _Minimum)
                    value = _Minimum;

                if (value > _Maximum)
                    value = _Maximum;

                if (_Round) _Value = Math.Round(value);
                else _Value = value;

                Refresh();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var unit = Width / (_Maximum - Minimum);

            using (Brush b = new SolidBrush(GlobalVariables.SecondaryBackColor))
            {
                e.Graphics.FillRectangle(b, new Rectangle(0, 0, Width, Height));
            }

            using (Brush b = new SolidBrush(GlobalVariables.PrimaryForeColor))
            {
                e.Graphics.FillRectangle(b, 2, 2, (int) ((_Value - Minimum) * unit) - 4, Height - 4);
            }


            using (Brush b = new SolidBrush(GlobalVariables.PrimaryTextColor))
            {
                var sizeF = e.Graphics.MeasureString(Math.Round(_Value, _RoundPlaces).ToString(), Font);

                if ((int) ((_Value - Minimum) * unit) - sizeF.Width / 2 <= 0)
                    e.Graphics.DrawString(Math.Round(_Value, _RoundPlaces).ToString(), Font, b, 0,
                        Height / 2 - sizeF.Height / 2 + 2);
                else if ((int) ((_Value - Minimum) * unit) + sizeF.Width / 2 >= Width)
                    e.Graphics.DrawString(Math.Round(_Value, _RoundPlaces).ToString(), Font, b, Width - sizeF.Width,
                        Height / 2 - sizeF.Height / 2 + 2);
                else
                    e.Graphics.DrawString(Math.Round(_Value, _RoundPlaces).ToString(), Font, b,
                        (int) ((_Value - Minimum) * unit) - sizeF.Width / 2, Height / 2 - sizeF.Height / 2 + 2);
            }
        }

        #region Properties

        [Category(".Euphoric")]
        public int DecimalPlaces
        {
            get => _RoundPlaces;
            set
            {
                if (value < 0)
                    throw new Exception("Value is Too Low");

                _RoundPlaces = value;
            }
        }

        [Category(".Euphoric")]
        public bool Round
        {
            get => _Round;
            set => _Round = value;
        }

        [Category(".Euphoric")]
        public double Minimum
        {
            get => _Minimum;
            set
            {
                if (value > _Value)
                    throw new Exception("Value is Too High");

                _Minimum = value;
            }
        }

        [Category(".Euphoric")]
        public double Maximum
        {
            get => _Maximum;
            set => _Maximum = value;
        }

        [Category(".Euphoric")]
        public double Value
        {
            get => _Value;
            set
            {
                if (value > _Maximum)
                    throw new Exception("Value is Too High");

                if (value < _Minimum)
                    throw new Exception("Value is Too Low");

                _Value = value;
                Refresh();
            }
        }

        #endregion
    }
}