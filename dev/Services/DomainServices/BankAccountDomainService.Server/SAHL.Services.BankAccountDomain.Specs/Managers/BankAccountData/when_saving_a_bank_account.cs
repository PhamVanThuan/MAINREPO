using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.BankAccountDomain.Managers;
using SAHL.Services.Interfaces.BankAccountDomain.Models;

namespace SAHL.Services.BankAccountDomain.Specs.Managers.BankAccountData
{
    public class when_saving_a_bank_account : WithFakes
    {
        private static IBankAccountDataManager bankAccountDataManager;
        private static int bankAccountKey;
        private static FakeDbFactory dbFactory;
        private static BankAccountModel bankAccount;
        private static int result;

        Establish context = () =>
        {
            bankAccountDataManager = An<IBankAccountDataManager>();
            dbFactory = new FakeDbFactory();
            bankAccountKey = 1;
            bankAccount = new BankAccountModel("1352", "BranchName", "1352087464", Core.BusinessModel.Enums.ACBType.Current, "John Doe", "System");
            bankAccountDataManager = new BankAccountDataManager(dbFactory);
            dbFactory.FakedDb.DbContext.WhenToldTo<IReadWriteSqlRepository>(x => x.Insert<BankAccountDataModel>(Param.IsAny<BankAccountDataModel>()))
                .Callback<BankAccountDataModel>(y => { y.BankAccountKey = bankAccountKey; });
        };

        Because of = () =>
        {
            result = bankAccountDataManager.SaveBankAccount(bankAccount);
        };


        It should_insert_the_bank_account_into_system = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x => x.Insert<BankAccountDataModel>(Arg.Is<BankAccountDataModel>(
                y => y.ACBBranchCode == bankAccount.BranchCode
                && y.ACBTypeNumber == (int)bankAccount.AccountType
                && y.AccountName ==  y.AccountName
                && y.AccountNumber == y.AccountNumber)));
        };

        It should_return_a_bank_account_key = () =>
        {
            result.ShouldEqual(bankAccountKey);
        };
    }
}
