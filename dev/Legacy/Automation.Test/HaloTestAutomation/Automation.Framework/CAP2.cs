using Automation.Framework.DataAccess;
using Common.Enums;
using System.Collections.Generic;

namespace Automation.Framework
{
    public class CAP2 : WorkflowBase
    {
        public void FormsSentComplete(int keyValue)
        {
            this.UpdateCapStatus(keyValue, (int)CapStatusEnum.AwaitingDocuments);
        }

        public void FurtherAdvanceDecisionComplete(int keyValue)
        {
            this.UpdateCapStatus(keyValue, (int)CapStatusEnum.PrepareForCredit);
        }

        public void CreditApprovalComplete(int keyValue)
        {
            this.UpdateCapStatus(keyValue, (int)CapStatusEnum.CreditApproval);
        }

        public void GrantCAP2OfferStart(int keyValue)
        {
            this.UpdateCapStatus(keyValue, (int)CapStatusEnum.GrantedCap2Offer);
        }

        public void LASentComplete(int keyValue)
        {
            this.UpdateCapStatus(keyValue, (int)CapStatusEnum.AwaitingLA);
        }

        public void ReadyForReadvanceComplete(int keyValue)
        {
            this.UpdateCapStatus(keyValue, (int)CapStatusEnum.ReadvanceRequired);
        }

        private void UpdateCapStatus(int capOfferKey, int capStatusKey)
        {
            var parameters = new Dictionary<string, string>
                                 {
                                     {"@CapOfferKey", capOfferKey.ToString()},
                                     {"@CapStatusKey", capStatusKey.ToString()}
                                 };
            DataHelper.ExecuteProcedure("test.UpdateCAPStatus", parameters);
        }
    }
}