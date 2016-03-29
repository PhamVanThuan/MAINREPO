namespace Automation.Framework
{
    public class ApplicationCapture : WorkflowBase
    {
        public void CleanupNewBusinessOffer(int keyValue)
        {
            base.CleanupNewBusinessOffer(keyValue);
            base.InsertOfferMailingAddress(keyValue);
            base.InsertEmploymentRecords(keyValue);
            base.CleanUpOfferDebitOrder(keyValue);
            base.InsertSeller(keyValue);
            base.InsertSettlementBanking(keyValue);
            base.InsertOfferAssetLiability(keyValue);
            base.InsertITCv4(keyValue, -1, -1);
            base.CleanUpOfferDomicilium(keyValue);
        }
    }
}