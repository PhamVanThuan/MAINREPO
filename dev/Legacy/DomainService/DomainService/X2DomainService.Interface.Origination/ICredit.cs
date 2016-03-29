using SAHL.Common.Collections.Interfaces;
using SAHL.X2.Common;
using System;
using System.Collections.Generic;

namespace X2DomainService.Interface.Origination
{
    public interface ICredit : IX2WorkflowService
    {
        void PerformCreditMandateCheck(IDomainMessageCollection messages, int applicationKey, Int64 instanceID, List<string> loadBalanceStates, bool loadBalanceIncludeStates, bool loadBalance1stPass, bool loadBalance2ndPass);

        void CreditResub(IDomainMessageCollection messages, int applicationKey);

        bool CheckCreditApprovalRules(IDomainMessageCollection messages, int applicationKey, bool ignoreWarnings);

        bool DoesNotMeetCreditSignatureRequirements(IDomainMessageCollection messages, int applicationKey, Int64 instanceID);

        void UpdateConditions(IDomainMessageCollection messages, int applicationKey);

        void SendResubMail(IDomainMessageCollection messages, int applicationKey);

        bool IsReviewRequired(IDomainMessageCollection messages, Int64 instanceID, string actionSource);

        bool IsCreditSecondPass(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, long instanceID);

        bool IsValuationApprovalRequired(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, long instanceID);

        void SendCreditDecisionMail(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, long instanceID, string action, int offerKey);

        bool CheckEmploymentTypeConfirmedRule(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, long instanceID, bool ignoreWarnings);

        bool CheckApplicationIsNewBusinessRule(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, int applicationKey, bool ignoreWarnings);

        void DisqualifyApplicationForGEPF(IDomainMessageCollection messages, int applicationKey);
    }
}