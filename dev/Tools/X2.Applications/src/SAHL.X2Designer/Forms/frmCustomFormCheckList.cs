using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SAHL.X2Designer.Documents;
using SAHL.X2Designer.Misc;

namespace SAHL.X2Designer.Forms
{
    public partial class frmCustomFormCheckList : Form
    {
        private ProcessDocument m_ProcDoc;
        private List<CustomFormItem> m_SelectedForms;

        public frmCustomFormCheckList(List<CustomFormItem> selectedCustomForms, ProcessDocument pDoc)
        {
            InitializeComponent();
            m_ProcDoc = pDoc;
            m_SelectedForms = selectedCustomForms;

            foreach (CustomFormItem mCustomForms in selectedCustomForms)
            {
                lsSelectedForms.Items.Add(mCustomForms.Name.ToString());
            }

            for (int x = 0; x < MainForm.App.GetCurrentView().Document.CurrentWorkFlow.Forms.Count; x++)
            {
                bool found = false;
                for (int y = 0; y < lsSelectedForms.Items.Count; y++)
                {
                    if (MainForm.App.GetCurrentView().Document.CurrentWorkFlow.Forms[x].Name == lsSelectedForms.Items[y].ToString())
                    {
                        found = true;
                        break;
                    }
                }
                if (found == false)
                {
                    lstAvailableForms.Items.Add(MainForm.App.GetCurrentView().Document.CurrentWorkFlow.Forms[x].Name);
                }
            }
        }

        private void cmdDone_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmCustomFormCheckList_Load(object sender, EventArgs e)
        {
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            string[] holdItems = new string[lsSelectedForms.Items.Count];
            string switchItem = "";
            CustomFormItem mHoldItem;
            int holdSelIndex = lsSelectedForms.SelectedIndex;
            if (lsSelectedForms.SelectedIndex != -1)
            {
                if (lsSelectedForms.SelectedIndex > 0)
                {
                    for (int x = 0; x < lsSelectedForms.Items.Count; x++)
                    {
                        holdItems[x] = lsSelectedForms.Items[x].ToString();
                    }
                    switchItem = holdItems[lsSelectedForms.SelectedIndex - 1];
                    mHoldItem = m_SelectedForms[lsSelectedForms.SelectedIndex - 1];
                    holdItems[lsSelectedForms.SelectedIndex - 1] = holdItems[lsSelectedForms.SelectedIndex];
                    m_SelectedForms[lsSelectedForms.SelectedIndex - 1] = m_SelectedForms[lsSelectedForms.SelectedIndex];
                    holdItems[lsSelectedForms.SelectedIndex] = switchItem;
                    m_SelectedForms[lsSelectedForms.SelectedIndex] = mHoldItem;
                    lsSelectedForms.Items.Clear();
                    lsSelectedForms.Items.AddRange(holdItems);
                    lsSelectedForms.SelectedIndex = holdSelIndex - 1;
                }
            }
        }

        private void btnDwn_Click(object sender, EventArgs e)
        {
            string[] holdItems = new string[lsSelectedForms.Items.Count];
            string switchItem = "";
            CustomFormItem mHoldItem;

            int holdSelIndex = lsSelectedForms.SelectedIndex;
            if (lsSelectedForms.SelectedIndex != -1)
            {
                if (lsSelectedForms.SelectedIndex < lsSelectedForms.Items.Count - 1)
                {
                    for (int x = 0; x < lsSelectedForms.Items.Count; x++)
                    {
                        holdItems[x] = lsSelectedForms.Items[x].ToString();
                    }

                    switchItem = holdItems[lsSelectedForms.SelectedIndex + 1];
                    mHoldItem = m_SelectedForms[lsSelectedForms.SelectedIndex + 1];
                    holdItems[lsSelectedForms.SelectedIndex + 1] = holdItems[lsSelectedForms.SelectedIndex];
                    m_SelectedForms[lsSelectedForms.SelectedIndex + 1] = m_SelectedForms[lsSelectedForms.SelectedIndex];
                    holdItems[lsSelectedForms.SelectedIndex] = switchItem;
                    m_SelectedForms[lsSelectedForms.SelectedIndex] = mHoldItem;
                    lsSelectedForms.Items.Clear();
                    lsSelectedForms.Items.AddRange(holdItems);
                    lsSelectedForms.SelectedIndex = holdSelIndex + 1;
                }
            }
        }

        private void cmdAdd_Click(object sender, EventArgs e)
        {
            if (lstAvailableForms.SelectedIndex != -1)
            {
                lsSelectedForms.Items.Add(lstAvailableForms.SelectedItem.ToString());
                for (int x = 0; x < MainForm.App.GetCurrentView().Document.CurrentWorkFlow.Forms.Count; x++)
                {
                    if (MainForm.App.GetCurrentView().Document.CurrentWorkFlow.Forms[x].Name == lstAvailableForms.SelectedItem.ToString())
                    {
                        m_SelectedForms.Add(MainForm.App.GetCurrentView().Document.CurrentWorkFlow.Forms[x]);
                    }
                }
                lstAvailableForms.Items.RemoveAt(lstAvailableForms.SelectedIndex);
            }
        }

        private void cmdRemove_Click(object sender, EventArgs e)
        {
            if (lsSelectedForms.SelectedIndex != -1)
            {
                lstAvailableForms.Items.Add(lsSelectedForms.SelectedItem.ToString());
                m_SelectedForms.RemoveAt(lsSelectedForms.SelectedIndex);
                lsSelectedForms.Items.RemoveAt(lsSelectedForms.SelectedIndex);
            }
        }
    }
}