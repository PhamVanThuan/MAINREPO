namespace BuildingBlocks.Services.Contracts
{
    public interface IHOCService
    {
        Automation.DataModels.HOCAccount GetHOCAccountDetails(int accountKey, int relatedMortgageLoanAccount = 0);

        Automation.DataModels.HOCAccount GetHOCAccountByPropertyKey(int propertyKey);

        bool MortgageLoanAccountHasOpenHOC(int accountKey);

        void CreateSAHLHOCAccount(int mortgageLoanOfferKey);
    }
}