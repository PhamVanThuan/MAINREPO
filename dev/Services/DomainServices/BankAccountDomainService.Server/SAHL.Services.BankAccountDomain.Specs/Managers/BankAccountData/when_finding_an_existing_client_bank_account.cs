using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.BankAccountDomain.Managers;
using SAHL.Services.BankAccountDomain.Managers.Statements;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.BankAccountDomain.Specs.Managers.BankAccountData
{
    public class when_finding_an_existing_client_bank_account : WithFakes
    {
        private static BankAccountDataManager dataManager;
        private static int clientKey;
        private static int bankAccountKey;
        private static IEnumerable<LegalEntityBankAccountDataModel> dataModels;
        private static IEnumerable<LegalEntityBankAccountDataModel> results;
        private static FakeDbFactory dbFactory;

        private Establish context = () =>
        {
            clientKey = 1;
            bankAccountKey = 2;
            dataModels = new LegalEntityBankAccountDataModel[] { new LegalEntityBankAccountDataModel(1, 2, (int)GeneralStatus.Active, "System", null) };
            dbFactory = new FakeDbFactory();
            dataManager = new BankAccountDataManager(dbFactory);
            dbFactory.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.Select<LegalEntityBankAccountDataModel>(Param.IsAny<ISqlStatement<LegalEntityBankAccountDataModel>>())).Return(dataModels);
        };

        private Because of = () =>
        {
            results = dataManager.FindExistingClientBankAccount(clientKey, bankAccountKey);
        };

        private It should_run_a_select_using_the_model_provided = () =>
        {
            dbFactory.FakedDb.DbReadOnlyContext.WasToldTo(x => x.Select(Arg.Is<GetClientBankAccountsStatement>(y => y.BankAccountKey == bankAccountKey && y.ClientKey == clientKey)));
        };

        private It should_return_the_results_from_the_database = () =>
        {
            results.ShouldBeTheSameAs(dataModels);
        };
    }
}