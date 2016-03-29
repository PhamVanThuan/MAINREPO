using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SAHL.X2InstanceManager.Items;
using SAHL.X2InstanceManager.Misc;

namespace SAHL.X2InstanceManager.Forms
{
    public partial class frmFindByWorkflow : Form
    {
        private string m_Value;
        private AndOr m_CriteriaType;
        private EqualsType m_EqualsType;

        public frmFindByWorkflow(string value, AndOr criteriaType,EqualsType equalsType)
        {
            MainForm.SetThreadPrincipal("X2");
            InitializeComponent();
            m_Value = value;
            m_CriteriaType = criteriaType;
            m_EqualsType = equalsType;
        }

        private void frmFindByWorkflow_Load(object sender, EventArgs e)
        {
            List<TreeNode> lstNodes = MainForm.App.GetAllWorkFlowNodes(MainForm.App.treeMain.SelectedNode);
            if(lstNodes != null)
            {
                for(int x=0;x<lstNodes.Count;x++)
                {
                    cbxWorkflow.Items.Add(lstNodes[x].Text.Substring(0,lstNodes[x].Text.IndexOf(" (")));
                }
            }
            TreeNode mNode = MainForm.App.GetWorkFlowNode(MainForm.App.treeMain.SelectedNode);
            if(mNode != null)
            {
                WorkFlowItem itm = mNode.Tag as WorkFlowItem;
                int idx = cbxWorkflow.FindString(itm.WorkFlowName);
                if(idx != -1)
                {
                    cbxWorkflow.SelectedIndex = idx;
                }
            }
            if (m_Value != null)
            {
                int idx = cbxWorkflow.FindString(m_Value);
                if (idx != -1)
                {
                    cbxWorkflow.SelectedIndex = idx;
                }
            }
            if (m_CriteriaType != AndOr.NotSet)
            {
                if (m_CriteriaType == AndOr.And)
                {
                    radioAnd.Checked = true;
                }
                else
                {
                    radioOr.Checked = true;
                }
            }
            if (m_EqualsType != EqualsType.NotSet)
            {
                if (m_EqualsType == EqualsType.EqualTo)
                {
                    radioEqual.Checked = true;
                }
                else
                {
                    radioNotEqual.Checked = true;
                }

            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (cbxWorkflow.Text.Length == 0 || (radioOr.Checked == false && radioAnd.Checked == false))
            {
                MessageBox.Show("Please fill in all values first!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.None;
            }
            else
            {
                this.DialogResult = DialogResult.OK;
            }
        }
    }
}