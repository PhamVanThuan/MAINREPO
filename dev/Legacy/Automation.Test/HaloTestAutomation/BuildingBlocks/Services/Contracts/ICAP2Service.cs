using Automation.DataAccess;
using Common.Enums;

namespace BuildingBlocks.Services.Contracts
{
    public interface ICAP2Service
    {
        string GetCAP2WorkflowCurrentState(int accountKey);

        void GetCAP2PreviousStateAndStatus(int accountKey, out string prevState, out string prevStatus);

        Automation.DataModels.CapOffer GetTestCapOffer(string identifier);

        void CAP2AutomationSetup(string brokerAdUserName, string creditAdUserName);

        int GetCAP2InstanceID(int accountKey);

        string GetCapExpiryDate(int accountKey);

        QueryResults GetLatestCapOfferByAccountKey(int accountKey);

        void OptIntoCAP2();

        QueryResults GetLegalEntitiesWithMoreThanOneCAPTestCase();

        QueryResults GetLatestCap2X2DataByAccountKeyAndCapOfferKey(int accountKey, string capOfferKey);

        QueryResults GetCapOfferDetailByAccountKey(int accountKey);

        QueryResults GetCapPaymentOptionByAccountKey(int accountKey);

        QueryResults GetCap2X2Data(int accountKey);

        QueryResults GetCap2FinancialAdjustmentDetailByAccountKey(int accountKey, FinancialAdjustmentStatusEnum financialAdjustmentStatus);

        QueryResults GetCapOfferDetailByAccountAndStatus(int accountKey, int capStatusKey);

        QueryResults GetCapTypeConfigurationEndDateForCapOffer(int accountKey);

        QueryResults GetCapNTUReasons(string reasonType);

        QueryResults GetAccountEmploymentType(int accountKey);
    }
}