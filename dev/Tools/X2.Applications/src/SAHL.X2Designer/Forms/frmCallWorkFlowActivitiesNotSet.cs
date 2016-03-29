using System.Collections.Generic;
using System.Windows.Forms;
using SAHL.X2Designer.Documents;
using SAHL.X2Designer.Items;

namespace SAHL.X2Designer.Forms
{
    public partial class frmCallWorkFlowActivitiesNotSet : Form
    {
        public frmCallWorkFlowActivitiesNotSet(List<CallWorkFlowActivity> lstCallWorkFlowActivities)
        {
            InitializeComponent();
            PopulateList(lstCallWorkFlowActivities);
        }

        private void PopulateList(List<CallWorkFlowActivity> lstCallWorkFlowActivities)
        {
            for (int x = 0; x < lstCallWorkFlowActivities.Count; x++)
            {
                WorkFlow itmParent = lstCallWorkFlowActivities[x].Parent as WorkFlow;
                ListViewItem mItem = new ListViewItem(new string[] { itmParent.WorkFlowName, lstCallWorkFlowActivities[x].Name });
                listViewCallWorkFlows.Items.Add(mItem);
            }
        }
    }
}