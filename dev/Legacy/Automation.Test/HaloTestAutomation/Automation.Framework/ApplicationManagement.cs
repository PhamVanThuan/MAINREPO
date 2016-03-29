using Common.Constants;
using System.Linq;

namespace Automation.Framework
{
    public class ApplicationManagement : WorkflowBase
    {
        public void ApplicationInOrderStart(int keyValue)
        {
            base.UpdateDeedsOfficeDetails(keyValue);
        }

        public void PriorToStartDecline(int keyValue)
        {
            base.InsertReason(keyValue, 441);
        }

        public void PriorToStartNTU(int keyValue)
        {
            base.InsertReason(keyValue, 228);
        }

        public void PriorToQueryOnApplication(int keyValue)
        {
            base.InsertReason(keyValue, 350);
        }

        public void PostQAComplete(int keyValue)
        {
            var x2Data = base.GetAppManInstanceDetails(keyValue, false);
            var instanceID = x2Data.FirstOrDefault().Columns.SingleOrDefault(x => x.Name.ToLower() == "instanceid").GetValueAs<int>();
            base.WaitForX2WorkflowHistoryActivity(WorkflowActivities.ApplicationManagement.NewPurchase, instanceID, 1);
        }

        public void PostApplicationInOrder(int keyValue)
        {
            var x2Data = base.GetAppManInstanceDetails(keyValue, false);
            var instanceID = x2Data.FirstOrDefault().Columns.SingleOrDefault(x => x.Name.ToLower() == "instanceid").GetValueAs<int>();
            base.WaitForX2WorkflowHistoryActivity(WorkflowActivities.ApplicationManagement.ApplicationinOrder, instanceID, 1);
        }

        public void PriorToStartNTUPipeline(int keyValue)
        {
            base.InsertReason(keyValue, 333);
        }
    }
}