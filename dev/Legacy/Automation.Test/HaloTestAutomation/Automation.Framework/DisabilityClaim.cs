using Automation.Framework.DataAccess;
using System.Collections.Generic;

namespace Automation.Framework
{
    public class DisabilityClaim : WorkflowBase
    {
        public void PriorToCaptureDetails(int keyValue)
        {
            var parameters = new Dictionary<string, string> { { "@disabilityClaimKey", keyValue.ToString() } };
            DataHelper.ExecuteProcedure("test.CaptureDisabilityClaim", parameters);
        }

        public void PriorToApprove(int keyValue)
        {
            var parameters = new Dictionary<string, string> { { "@disabilityClaimKey", keyValue.ToString() } };
            DataHelper.ExecuteProcedure("test.ApproveDisabilityClaim", parameters);
        }
    }
}