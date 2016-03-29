using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SAHL.X2Designer.Forms
{
    internal partial class frmSelectConnStr : Form
    {
        List<SAHL.X2Designer.Helpers.ConnectionStringClass> Conns = new List<SAHL.X2Designer.Helpers.ConnectionStringClass>();
        internal SAHL.X2Designer.Helpers.ConnectionStringClass conn = null;

        internal frmSelectConnStr(List<SAHL.X2Designer.Helpers.ConnectionStringClass> Conns)
        {
            this.Conns = Conns;
            InitializeComponent();
        }

        private void frmSelectConnStr_Load(object sender, EventArgs e)
        {
            li.DataSource = Conns;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (null != li.SelectedValue)
            {
                conn = li.SelectedValue as SAHL.X2Designer.Helpers.ConnectionStringClass;
                this.Close();
            }
        }

        private void btnNone_Click(object sender, EventArgs e)
        {
            conn = null;
            this.Close();
        }
    }
}