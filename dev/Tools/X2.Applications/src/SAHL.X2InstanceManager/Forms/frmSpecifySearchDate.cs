using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SAHL.X2InstanceManager.Forms
{
    public partial class frmSpecifySearchDate : Form
    {
        public frmSpecifySearchDate()
        {
            InitializeComponent();
        }

        private void radioRange_CheckedChanged(object sender, EventArgs e)
        {
            if (radioRange.Checked)
            {
                lblAnd.Visible = true;
                dateTimePicker2.Visible = true;
            }
            else
            {
                lblAnd.Visible = false;
                dateTimePicker2.Visible = false;
            }
        }

        private void btnFind_Click(object sender, EventArgs e)
        {

        }

       
    }
}