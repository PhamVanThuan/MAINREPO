using System;
using System.Windows.Forms;
using SAHL.X2Designer.Documents;
using SAHL.X2Designer.Items;

namespace SAHL.X2Designer.Forms
{
    public partial class frmManageAppliedToStates : Form
    {
        private ProcessDocument m_ProcDoc;
        private AppliedStatesCollection m_AppliedCol;

        public frmManageAppliedToStates(AppliedStatesCollection AppliedCol, ProcessDocument pDoc)
        {
            InitializeComponent();
            m_ProcDoc = pDoc;
            m_AppliedCol = AppliedCol;
            foreach (BaseState state in pDoc.CurrentWorkFlow.States)
            {
                if ((state is CommonState) == false
                && (state is ArchiveState) == false)
                {
                    bool found = false;
                    foreach (BaseState mState in AppliedCol)
                    {
                        if (mState.Text == state.Text)
                        {
                            found = true;
                            break;
                        }
                    }
                    if (found)
                    {
                        checkListStates.Items.Add(state.Text, true);
                    }
                    else
                    {
                        checkListStates.Items.Add(state.Text, false);
                    }
                }
            }
        }

        private void cmdDone_Click(object sender, EventArgs e)
        {
            m_AppliedCol.Clear();
            for (int x = 0; x < checkListStates.Items.Count; x++)
            {
                if (checkListStates.GetItemChecked(x))
                {
                    foreach (BaseState state in m_ProcDoc.CurrentWorkFlow.States)
                    {
                        if (state.Text == checkListStates.Items[x].ToString())
                        {
                            m_AppliedCol.Add(state);
                        }
                    }
                }
            }
            if (MainForm.App.GetCurrentView() != null)
            {
                MainForm.App.GetCurrentView().setModified(true);
            }

            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        }
    }
}