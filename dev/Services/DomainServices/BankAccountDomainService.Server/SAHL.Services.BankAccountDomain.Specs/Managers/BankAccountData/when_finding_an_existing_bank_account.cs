using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.BankAccountDomain.Managers;
using SAHL.Services.BankAccountDomain.Managers.Statements;
using SAHL.Services.Interfaces.BankAccountDomain.Models;
using System;
using System.Collections.Generic;

namespace SAHL.Services.BankAccountDomain.Specs.Managers.BankAccountData
{
    public class when_finding_an_existing_bank_account : WithFakes
    {
        private static BankAccountDataManager bankAccountDataManager;
        private static BankAccountModel bankAccountModel;
        private static FakeDbFactory dbFactory;
        private static IEnumerable<BankAccountDataModel> dataModels;
        private static IEnumerable<BankAccountDataModel> results;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            bankAccountDataManager = new BankAccountDataManager(dbFactory);
            dataModels = new BankAccountDataModel[] { new BankAccountDataModel(1234, "Code", "AccountNumber", 2, "AccountName", "System", DateTime.Now) };
            bankAccountModel = new BankAccountModel("12345", "BranchName", "1234567", Core.BusinessModel.Enums.ACBType.Current, "Test", "System");
            
            dbFactory.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.Select<BankAccountDataModel>(Param.IsAny<ISqlStatement<BankAccountDataModel>>())).Return(dataModels);
        };

        private Because of = () =>
        {
            results = bankAccountDataManager.FindExistingBankAccount(bankAccountModel);
        };

        private It should_run_a_select_using_the_model_provided = () =>
        {
            dbFactory.FakedDb.DbReadOnlyContext.WasToldTo(x => x.Select(Arg.Is<GetBankAccountStatement>(y => y.AccountNumber == bankAccountModel.AccountNumber)));
        };

        private It should_return_the_results_from_the_database = () =>
        {
            results.ShouldBeTheSameAs(dataModels);
        };
    }
}