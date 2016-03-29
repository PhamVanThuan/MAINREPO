using System;
using System.Windows.Forms;
using Northwoods.Go;
using SAHL.X2Designer.Items;

namespace SAHL.X2Designer.Forms
{
    public partial class frmAddEditRoles : Form
    {
        public RolesCollectionItem holdRoleItem;
        private bool iSetTheCheckBox = false;

        public frmAddEditRoles()
        {
            InitializeComponent();
        }

        private void frmAddEditRoles_Load(object sender, EventArgs e)
        {
            if (radioWorkFlow.Checked == true)
            {
                PopulateWorkFlowCbx();
            }
            else
            {
                cbxSelectWorkFlow.Enabled = false;
            }
        }

        public void PopulateWorkFlowCbx()
        {
            cbxSelectWorkFlow.Items.Clear();
            cbxSelectWorkFlow.Enabled = true;
            for (int x = 0; x < MainForm.App.GetCurrentView().Document.WorkFlows.Length; x++)
            {
                cbxSelectWorkFlow.Items.Add(MainForm.App.GetCurrentView().Document.WorkFlows[x].WorkFlowName);
            }
        }

        private void radioWorkFlow_CheckedChanged(object sender, EventArgs e)
        {
            if (radioWorkFlow.Checked == true)
            {
                cbxSelectWorkFlow.Items.Clear();
                PopulateWorkFlowCbx();
            }
            else
            {
                cbxSelectWorkFlow.Enabled = false;
            }
        }

        private void radioWorkFlow_Click(object sender, EventArgs e)
        {
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (cbxSelectWorkFlow.Enabled == true)
            {
                if (cbxSelectWorkFlow.Text.Length < 1)
                {
                    MessageBox.Show("Please Select a WorkFlow before continuing!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            if (txtRole.Text.Length < 1)
            {
                MessageBox.Show("Please Specify a Role before continuing!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            this.DialogResult = DialogResult.OK;
        }

        private void checkDynamic_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void checkDynamic_CheckStateChanged(object sender, EventArgs e)
        {
            if (iSetTheCheckBox == false)
            {
                for (int x = 0; x < MainForm.App.GetCurrentView().Document.WorkFlows.Length; x++)
                {
                    foreach (GoObject o in MainForm.App.GetCurrentView().Document.WorkFlows[x])
                    {
                        if (o is BaseActivity)
                        {
                            BaseItem bi = o as BaseItem;
                            foreach (CustomLink l in bi.Links)
                            {
                                if (l.FromNode.GetType() == typeof(ClapperBoard) || l.ToNode.GetType() == typeof(ClapperBoard))
                                {
                                    UserActivity ba = bi as UserActivity;
                                    if (ba != null)
                                    {
                                        for (int y = 0; y < ba.Access.Count; y++)
                                        {
                                            if (ba.Access[y].RoleItem == holdRoleItem && ba.Access[y].IsChecked)
                                            {
                                                iSetTheCheckBox = true;
                                                MessageBox.Show("This role cannot be set to dynamic!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                checkDynamic.Checked = false;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                iSetTheCheckBox = false;
            }
        }
    }
}