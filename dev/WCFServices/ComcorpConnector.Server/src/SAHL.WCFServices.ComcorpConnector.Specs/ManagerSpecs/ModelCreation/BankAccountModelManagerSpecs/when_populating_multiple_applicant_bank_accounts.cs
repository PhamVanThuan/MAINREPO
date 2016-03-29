using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.BusinessModel.Validation;
using SAHL.Core.Testing;
using SAHL.DomainProcessManager.Models;
using SAHL.WCFServices.ComcorpConnector.Managers.ModelCreation;
using SAHL.WCFServices.ComcorpConnector.Objects;
using SAHL.WCFServices.ComcorpConnector.Specs.HandlerSpecs.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.WCFServices.ComcorpConnector.Specs.ManagerSpecs.ModelCreation.BankAccountModelManagerSpecs
{
    public class when_populating_multiple_applicant_bank_accounts : WithCoreFakes
    {
        private static BankAccountModelManager modelManager;
        private static ValidationUtils _validationUtils;
        private static List<BankAccountModel> bankAccounts;
        private static List<BankAccount> comcorpBankAccounts;

        private Establish context = () =>
        {
            _validationUtils = new ValidationUtils();
            modelManager = new BankAccountModelManager(_validationUtils);
            comcorpBankAccounts = IntegrationServiceTestHelper.PopulateBankAccounts();
        };

        private Because of = () =>
        {
            bankAccounts = modelManager.PopulateBankAccounts(comcorpBankAccounts);
        };

        private It should_add_both_bank_accounts = () =>
        {
            bankAccounts.Count().ShouldEqual(2);
        };

        private It should_contain_the_details_of_the_first_bank_account = () =>
        {
            bankAccounts.Where(x => x.AccountName == comcorpBankAccounts.First().AccountName
                && x.AccountNumber == comcorpBankAccounts.First().AccountNumber
                && x.AccountType == ACBType.Current
                && x.BranchCode == comcorpBankAccounts.First().STDAccountBranchCode
                && x.BranchName == comcorpBankAccounts.First().AccountBranch
                && x.IsDebitOrderBankAccount == comcorpBankAccounts.First().isMainAccount
                ).FirstOrDefault().ShouldNotBeNull();
        };

        private It should_contain_the_details_of_the_second_bank_account = () =>
        {
            bankAccounts.Where(x => x.AccountName == comcorpBankAccounts.Last().AccountName
                && x.AccountNumber == comcorpBankAccounts.Last().AccountNumber
                && x.AccountType == ACBType.Savings
                && x.BranchCode == comcorpBankAccounts.Last().STDAccountBranchCode
                && x.BranchName == comcorpBankAccounts.Last().AccountBranch
                && x.IsDebitOrderBankAccount == comcorpBankAccounts.Last().isMainAccount
                ).FirstOrDefault().ShouldNotBeNull();
        };
    }
}