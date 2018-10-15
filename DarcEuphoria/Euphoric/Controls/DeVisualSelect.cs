using System.Windows.Forms;

namespace DarcEuphoria.Euphoric.Controls
{
    public partial class DeVisualSelect : UserControl
    {
        public DeVisualSelect()
        {
            InitializeComponent();
        }

        public int SelectedIndex
        {
            get
            {
                foreach (DeRadioButton derdb in dePanel1.Controls)
                    if (derdb.Checked)
                        return int.Parse(derdb.Name.Substring(3));
                return 0;
            }
        }
    }
}