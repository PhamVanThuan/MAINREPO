using Automation.DataAccess;
using Common.Enums;

namespace BuildingBlocks.Services.Contracts
{
    public interface IFurtherLendingService
    {
        QueryResults GetLegalEntityWhoQualifiesForMoreThanOneFLApplication();

        QueryResults GetFLAutomationRowByTestIdentifier(string identifier);

        int ReturnFurtherLendingOfferKeyByTestGroup(QueryResults results);

        void UpdateFLAutomation(string colToUpdate, string newValue, string identifier);

        double GetLAAExceededAmountForFurtherAdvance(int offerKey);

        void UpdateAssignedCreditUser(int offerKey, string identifier, out string creditADUser, out OfferRoleTypeEnum creditOfferRoleType);

        void InsertFLOfferKeys(int accountKey);

        void CleanUpOfferData(int offerKey);

        int GetFurtherLendingOfferWithLightstonePropertyID(bool lightstoneValOlderThan2Months);

        QueryResults ReassignMultipleApps();

        int GetAccountForLTPTest();
    }
}