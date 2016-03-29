using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.BankAccountDomain.Managers;

namespace SAHL.Services.BankAccountDomain.Specs.Managers.BankAccountData
{
    public class when_adding_bank_account_to_client : WithFakes
    {
        private static IBankAccountDataManager bankAccountDataManager;
        private static int clientKey;
        private static int bankAccountKey;
        private static int clientBankAccountKey;
        private static int result;
        private static FakeDbFactory dbFactory;

        Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            bankAccountDataManager = new BankAccountDataManager(dbFactory);
            clientKey = 5;
            bankAccountKey = 1;
            clientBankAccountKey = 100;
            dbFactory.FakedDb.DbContext.WhenToldTo<IReadWriteSqlRepository>(x => x.Insert<LegalEntityBankAccountDataModel>(Param.IsAny<LegalEntityBankAccountDataModel>()))
                .Callback<LegalEntityBankAccountDataModel>(y => { y.LegalEntityBankAccountKey = clientBankAccountKey; });
        };

        Because of = () =>
        {
            result = bankAccountDataManager.AddBankAccountToClient(clientKey, bankAccountKey);
        };

        It should_insert_client_bank_account_to_system = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x => x.Insert<LegalEntityBankAccountDataModel>(Arg.Is<LegalEntityBankAccountDataModel>(
                y => y.LegalEntityKey == clientKey 
                    && y.BankAccountKey == bankAccountKey 
                    && y.GeneralStatusKey == (int)GeneralStatus.Active)));
        };

        It should_return_a_client_bank_acccount_key = () =>
        {
            result.ShouldEqual(clientBankAccountKey);
        };
    }
}
