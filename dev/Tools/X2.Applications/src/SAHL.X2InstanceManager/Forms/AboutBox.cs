using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
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
            try
            {
                //lblVersion.Text += System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion;
                lblVersion.Text += Application.ProductVersion;
            }
            catch
            {
            }
        }
    }
}