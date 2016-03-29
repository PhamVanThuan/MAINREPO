using System;
using System.Windows.Forms;
using SAHL.X2Designer.Documents;
using SAHL.X2Designer.Items;

namespace SAHL.X2Designer.Forms
{
    public partial class frmProperties : Form
    {
        BaseItem Item;
        ProcessDocument pd;

        public frmProperties(ProcessDocument pd)
        {
            InitializeComponent();
            this.pd = pd;
        }

        private void frmProperties_Load(object sender, EventArgs e)
        {
            MainForm.App.OnProcessViewActivated += new MainForm.ProcessViewActivatedHandler(App_OnProcessViewActivated);
            MainForm.App.OnProcessViewDeactivated += new MainForm.ProcessViewDeactivatedHandler(App_OnProcessViewDeactivated);
            MainForm.App.OnGeneralCustomFormClosed += new MainForm.GeneralCustomFormClosedHandler(App_OnGeneralCustomFormClosed);

            if (MainForm.App.GetCurrentView() != null)
            {
                MainForm.App.GetCurrentView().OnWorkFlowItemSelected += new SAHL.X2Designer.Views.ProcessView.OnWorkFlowItemSelectedHandler(frmProperties_OnWorkFlowItemSelected);
                MainForm.App.GetCurrentView().OnWorkFlowItemUnSelected += new SAHL.X2Designer.Views.ProcessView.OnWorkFlowItemUnSelectedHandler(frmProperties_OnWorkFlowItemUnSelected);
            }
        }

        #region Events

        private void frmProperties_OnWorkFlowItemUnSelected(IWorkflowItem UnSelectedItem)
        {
        }

        private void frmProperties_OnWorkFlowItemSelected(BaseItem SelectedItem)
        {
            if (MainForm.App.documentIsBeingOpened) return;
            OnItemSelected(SelectedItem);
        }

        private void App_OnGeneralCustomFormClosed(GeneralCustomFormType formType)
        {
        }

        private void App_OnProcessViewDeactivated(SAHL.X2Designer.Views.ProcessView v)
        {
        }

        private void App_OnProcessViewActivated(SAHL.X2Designer.Views.ProcessView v)
        {
        }

        #endregion Events

        private void OnItemSelected(BaseItem Item)
        {
            this.Item = Item;
            tc.TabPages.Clear();
            string t = Item.GetType().ToString();
            int idx = t.LastIndexOf('.');
            t = t.Substring(idx + 1, (t.Length - idx - 1));
            if (!Enum.IsDefined(typeof(WorkflowItemType), t)) return;
            WorkflowItemType dt = (WorkflowItemType)Enum.Parse(typeof(WorkflowItemType), t);
            switch (dt)
            {
                case WorkflowItemType.UserActivity:
                    {
                        BuildGeneralTab(dt, Item);

                        break;
                    }
                case WorkflowItemType.UserState:
                    {
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        private void BuildGeneralTab(WorkflowItemType dt, BaseItem item)
        {
            TabPage tpGeneral = new TabPage("General");
            ucBaseProperties bp = new ucBaseProperties(Item);
            tpGeneral.Controls.Add(bp);
            tc.TabPages.Add(tpGeneral);
        }

        private void BuildStageTransitionTab(WorkflowItemType dt, BaseItem item)
        {
            TabPage tpStageTransitions = new TabPage("State Transitions");
            tc.TabPages.Add(tpStageTransitions);
        }
    }
}