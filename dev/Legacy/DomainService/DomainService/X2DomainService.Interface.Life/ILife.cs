namespace X2DomainService.Interface.Life
{
    using System;
    using SAHL.Common.Collections.Interfaces;
    using SAHL.X2.Common;

    public interface ILife : IX2WorkflowService
    {
        DateTime GetActivityTimeWaitForCallback(IDomainMessageCollection messages, int applicationKey);

        bool ContinueSale(IDomainMessageCollection messages, int applicationKey);

        void AwaitingConfirmationTimeout(IDomainMessageCollection messages, int applicationKey);

        void AcceptBenefits(IDomainMessageCollection messages, int applicationKey);

        void OlderThan45Days(IDomainMessageCollection messages, int applicationKey, long instanceID);

        void ActivateLifePolicy(IDomainMessageCollection messages, int applicationKey);

        void CreateInstance(IDomainMessageCollection messages, int loanNumber, long instanceID, string assignTo, out int applicationKey, out string name, out string subject, out int priority);

        void PolicyNTUd(IDomainMessageCollection messages, int applicationKey);

        void ReadyToCallback(IDomainMessageCollection messages, int applicationKey, long instanceID);

        void DeclineQuote(IDomainMessageCollection messages, int applicationKey);

        void NTUPolicy(IDomainMessageCollection messages, int applicationKey);

        void ReactivatePolicy(IDomainMessageCollection messages, int applicationKey);

        int GetPolicyTypeKeyForOfferLife(IDomainMessageCollection messages, int applicationKey);
    }
}