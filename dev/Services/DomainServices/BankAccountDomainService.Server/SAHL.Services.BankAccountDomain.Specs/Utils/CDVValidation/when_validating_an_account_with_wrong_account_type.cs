using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.SystemMessages;
using SAHL.Services.BankAccountDomain.Managers;
using System;
using System.Linq;
using System.Collections.Generic;
using SAHL.Services.BankAccountDomain.Utils;

namespace SAHL.Services.BankAccountDomain.Specs.Utils.CDVValidation
{
    internal class when_validating_an_account_with_wrong_account_type : WithFakes
    {
        private static CdvValidationManager validationManager;
        private static string branchCode = "100943";
        private static string accountNumber = "1903481333";
        private static int accountType = 2;
        private static ISystemMessageCollection systemMessages;
        private static ICDVDataManager dataManager;
        private static bool result;

        private static ACBBankDataModel bank;
        private static CDVDataModel cdv;

        private Establish context = () =>
        {
            systemMessages = SystemMessageCollection.Empty();
            dataManager = An<ICDVDataManager>();
            validationManager = new CdvValidationManager(systemMessages, dataManager);

            bank = new ACBBankDataModel(12, "Nedbank");
            cdv = new CDVDataModel(1356, 12, "100943", 2, 0, null, "0101090807060504030201", 11, 18, null, 4, "CIM900", new DateTime(2012, 05, 01));

            dataManager.WhenToldTo(x => x.GetBankForACBBranch(branchCode)).Return(new List<ACBBankDataModel> { bank });
            dataManager.WhenToldTo(x => x.GetAccountTypeRecognitions(bank.ACBBankCode, accountType)).Return(CDVConfigData.AccountTypeRecognitions);
            dataManager.WhenToldTo(x => x.GetAccountIndications()).Return(CDVConfigData.AccountIndications);
            dataManager.WhenToldTo(x => x.GetCDVs(bank.ACBBankCode, branchCode, accountType)).Return(new List<CDVDataModel> { cdv });
            dataManager.WhenToldTo(x => x.GetCDVExceptions(bank.ACBBankCode, cdv.ExceptionCode, accountType))
                .Return(CDVConfigData.CdvExceptions.Where(x=>x.ACBBankCode==bank.ACBBankCode && x.ExceptionCode == cdv.ExceptionCode && x.ACBTypeNumber == accountType));
        };

        private Because of = () =>
        {
            result = validationManager.ValidateAccountNumber(branchCode, accountType, accountNumber);
        };

        private It should_return_that_the_account_number_is_invalid = () =>
        {
            result.ShouldBeFalse();
        };
        private It should_return_an_error_message = () =>
        {
            systemMessages.ErrorMessages().First().Message.ShouldEqual("The CDV check for the account number failed.");
        };
    }
}