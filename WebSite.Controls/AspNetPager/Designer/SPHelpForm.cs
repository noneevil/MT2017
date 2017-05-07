using System.Windows.Forms;

namespace WebSite.Controls
{
    public partial class SPHelpForm : Form
    {
        public SPHelpForm()
        {
            InitializeComponent();
        }

        private void SPHelpForm_Load(object sender, System.EventArgs e)
        {
            textBox1.Select(0, 0);
        }
    }
}
