using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SAHL.X2Designer.Items;

namespace SAHL.X2Designer.Forms
{
    public partial class frmInvalidBusinessStageTransitionWarning : Form
    {
        public frmInvalidBusinessStageTransitionWarning(List<InvalidStageTransitionItem> lstInvalidStages)
        {
            InitializeComponent();
            PopulateListView(lstInvalidStages);
        }

        private void PopulateListView(List<InvalidStageTransitionItem> lstInvalidStages)
        {
            for (int x = 0; x < lstInvalidStages.Count; x++)
            {
                ListViewItem itm = new ListViewItem(new string[] {lstInvalidStages[x].workFlow.WorkFlowName,
                                                                  lstInvalidStages[x].ActivityName,
                                                                  lstInvalidStages[x].DefinitionGroupDescription,
                                                                  lstInvalidStages[x].DefinitionDescription});
                itm.Tag = lstInvalidStages[x];
                listView1.Items.Add(itm);
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This will remove the invalid stage transitions from the relevant activities!\n"
                + "Are you sure you want to continue?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                //Remove all the invalid stage transitions
                for (int x = 0; x < listView1.Items.Count; x++)
                {
                    InvalidStageTransitionItem item = listView1.Items[x].Tag as InvalidStageTransitionItem;
                    foreach (BaseActivity a in item.workFlow.Activities)
                    {
                        if (a.Name == item.ActivityName)
                        {
                            switch (a.WorkflowItemType)
                            {
                                case WorkflowItemType.UserActivity:
                                    {
                                        UserActivity act = a as UserActivity;
                                        for (int y = act.BusinessStageTransitions.Count - 1; y > -1; y--)
                                        {
                                            if (act.BusinessStageTransitions[y].SDSDGKey == item.SDSDGKey
                                                && act.BusinessStageTransitions[y].DefinitionGroupDescription == item.DefinitionGroupDescription
                                                && act.BusinessStageTransitions[y].DefinitionDescription == item.DefinitionDescription)
                                            {
                                                act.BusinessStageTransitions.RemoveAt(y);
                                            }
                                        }
                                        break;
                                    }
                                case WorkflowItemType.ExternalActivity:
                                    {
                                        ExternalActivity act = a as ExternalActivity;
                                        for (int y = act.BusinessStageTransitions.Count - 1; y > -1; y--)
                                        {
                                            if (act.BusinessStageTransitions[y].SDSDGKey == item.SDSDGKey
                                              && act.BusinessStageTransitions[y].DefinitionGroupDescription == item.DefinitionGroupDescription
                                                && act.BusinessStageTransitions[y].DefinitionDescription == item.DefinitionDescription)
                                            {
                                                act.BusinessStageTransitions.RemoveAt(y);
                                            }
                                        }
                                        break;
                                    }
                                case WorkflowItemType.ConditionalActivity:
                                    {
                                        ConditionalActivity act = a as ConditionalActivity;
                                        for (int y = act.BusinessStageTransitions.Count - 1; y > -1; y--)
                                        {
                                            if (act.BusinessStageTransitions[y].SDSDGKey == item.SDSDGKey
                                              && act.BusinessStageTransitions[y].DefinitionGroupDescription == item.DefinitionGroupDescription
                                                && act.BusinessStageTransitions[y].DefinitionDescription == item.DefinitionDescription)
                                            {
                                                act.BusinessStageTransitions.RemoveAt(y);
                                            }
                                        }
                                        break;
                                    }

                                case WorkflowItemType.TimedActivity:
                                    {
                                        TimedActivity act = a as TimedActivity;
                                        for (int y = act.BusinessStageTransitions.Count - 1; y > -1; y--)
                                        {
                                            if (act.BusinessStageTransitions[y].SDSDGKey == item.SDSDGKey
                                                && act.BusinessStageTransitions[y].DefinitionGroupDescription == item.DefinitionGroupDescription
                                                && act.BusinessStageTransitions[y].DefinitionDescription == item.DefinitionDescription)
                                            {
                                                act.BusinessStageTransitions.RemoveAt(y);
                                            }
                                        }
                                        break;
                                    }
                            }
                        }
                    }
                }
                MainForm.App.GetCurrentView().setModified(true);
                this.DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}