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
using System.Collections.Generic;

namespace SAHL.Services.BankAccountDomain.Specs.Managers.BankAccountData
{
    public class when_finding_existing_branch : WithFakes
    {
        private static IBankAccountDataManager bankAccountDataManager;
        private static BankAccountModel bankAccount;
        private static IEnumerable<ACBBranchDataModel> branches;
        private static IEnumerable<ACBBranchDataModel> results;
        private static FakeDbFactory dbFactory;

        Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            bankAccountDataManager = new BankAccountDataManager(dbFactory);
            bankAccount = new BankAccountModel("1352", "BranchName", "1352087464", Core.BusinessModel.Enums.ACBType.Current, "John Doe", "System");
            branches = new List<ACBBranchDataModel> { new ACBBranchDataModel("05404", 12, "Durban", "1") };
            
            dbFactory.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.Select<ACBBranchDataModel>(Param.IsAny<ISqlStatement<ACBBranchDataModel>>())).Return(branches);
        };

        Because of = () =>
        {
            results = bankAccountDataManager.FindExistingBranch(bankAccount);
        };

        It should_run_a_select_using_the_model_provided = () =>
        {
            dbFactory.FakedDb.DbReadOnlyContext.WasToldTo(x => x.Select(Arg.Is<GetBranchStatement>(y => y.BranchCode == bankAccount.BranchCode)));
        };

        It should_return_results_from_the_database = () =>
        {
            results.ShouldBeTheSameAs(branches);
        };
    }
}
