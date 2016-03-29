using SAHL.Common.Collections.Interfaces;
using SAHL.X2.Common;

namespace X2DomainService.Interface.Cap2
{
    public interface ICap2 : IX2WorkflowService
    {
        //bool OnCompleteActivity_Promotion_Client(IDomainMessageCollection messages, int applicationKey, int applicationDetailKey, int promotion, int capStatusKey, int capPaymentOptionKey, out string name);

        bool CheckReadvanceDoneRules(IDomainMessageCollection messages, bool ignoreWarnings, int applicationKey);

        void UpdateCapOfferStatus(IDomainMessageCollection messages, int applicationKey, int CapStatusKey);

        bool IsLANotRequired(IDomainMessageCollection messages, int applicationKey);

        bool IsCreditCheckRequired(IDomainMessageCollection messages, int applicationKey);
    }
}