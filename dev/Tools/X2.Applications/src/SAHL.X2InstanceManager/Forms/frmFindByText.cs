using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SAHL.X2InstanceManager.Misc;

namespace SAHL.X2InstanceManager.Forms
{
    public partial class frmFindByText : Form
    {
        private string m_Value;
        private AndOr m_CriteriaType;
        private EqualsType m_EqualsType;
        private bool m_Explicit;

        public frmFindByText(string value, AndOr criteriaType,EqualsType equalsType, bool Explicit)
        {
            MainForm.SetThreadPrincipal("X2");
            InitializeComponent();
            m_Value = value;
            m_CriteriaType = criteriaType;
            m_EqualsType = equalsType;
            m_Explicit = Explicit;
        }

        private void frmFindByText_Load(object sender, EventArgs e)
        {
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

            if (m_Value != null && m_Value.Length > 0)
            {
                txtValue.Text = m_Value;
            }

            if (m_Explicit)
            {
                checkExplicit.Checked = true;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (txtValue.Text.Length == 0 || (radioOr.Checked == false && radioAnd.Checked == false))
            {
                MessageBox.Show("Please fill in all values first!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.None;
            }
            else
            {
                this.DialogResult = DialogResult.OK;
            }
         
        }

        private void radioAnd_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}