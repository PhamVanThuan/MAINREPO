using System;
using System.Drawing;
using System.Windows.Forms;

namespace SAHL.X2Designer.Forms
{
    public partial class AboutBox : Form
    {
        public AboutBox()
        {
            InitializeComponent();
        }

        private void AboutBox_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AboutBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }

        private void AboutBox_Load(object sender, EventArgs e)
        {
            lblVersion.Text += String.IsNullOrEmpty(Application.ProductVersion) ? "" : Application.ProductVersion;

            this.pictureBox2.BackColor = Color.Transparent;
        }
    }
}