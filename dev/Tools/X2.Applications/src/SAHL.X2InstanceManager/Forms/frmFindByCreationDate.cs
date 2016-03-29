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
    public partial class frmFindByDate : Form
    {
        private string m_Value;
        private AndOr m_CriteriaType;

        public frmFindByDate(string value, AndOr criteriaType)
        {
            MainForm.SetThreadPrincipal("X2");
            InitializeComponent();
            m_Value = value;
            m_CriteriaType = criteriaType;
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


        private void frmFindByDate_Load(object sender, EventArgs e)
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

            if (m_Value != null && m_Value.Length > 0)
            {
                if (m_Value.Contains(" -> "))
                {
                    radioRange.Checked = true;
                    int pos = m_Value.IndexOf(" -> ");
                    string val1 = m_Value.Substring(0, pos);
                    dateTimePicker1.Value = DateTime.Parse(val1);
                    string val2 = m_Value.Substring(pos + 3, m_Value.Length - (pos + 3));
                    dateTimePicker2.Value = DateTime.Parse(val2);
                }
                else
                {
                    dateTimePicker1.Value = DateTime.Parse(m_Value);
                }
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (groupAndOr.Enabled == true && radioOr.Checked == false && radioAnd.Checked == false)
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