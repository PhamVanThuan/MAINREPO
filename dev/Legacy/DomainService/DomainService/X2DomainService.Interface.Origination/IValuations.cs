using System.Collections.Generic;
using SAHL.Common.Collections.Interfaces;
using SAHL.X2.Common;

namespace X2DomainService.Interface.Origination
{
    public interface IValuations : IX2WorkflowService
    {
        Dictionary<string, object> GetValuationData(IDomainMessageCollection messages, int applicationKey);

        void SendEmailToAllOpenApplicationConsultantsForValuationComplete(IDomainMessageCollection messages, int valuationKey, int applicationKey); //EmailOnCompleteValuation

        void SetValuationStatus(IDomainMessageCollection messages, int valuationKey, int status);

        void SetValuationIsActive(IDomainMessageCollection messages, int valuationKey);

        void SetValuationActiveAndSave(IDomainMessageCollection messages, int valuationKey);

        void RecalcHOC(IDomainMessageCollection messages, int valuationKey, int applicationKey, bool ignoreWarnings);

        bool CheckIfCanPerformFurtherValuation(IDomainMessageCollection messages, long appManInstanceID);

        bool CheckValuationExistsRecentRules(IDomainMessageCollection messages, int applicationKey, bool ignoreWarnings);

        bool CheckValuationInOrderRules(IDomainMessageCollection messages, int valuationKey, bool ignoreWarnings);
    }
}