using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.BankAccountDomain.Managers;
using SAHL.Services.BankAccountDomain.Managers.Statements;
using SAHL.Services.Interfaces.BankAccountDomain.Models;

namespace SAHL.Services.BankAccountDomain.Specs.Managers.BankAccountData
{
    public class when_activating_a_client_bank_account : WithFakes
    {
        private static IBankAccountDataManager bankAccountDataManager;
        private static int clientBankAccountKey;
        private static FakeDbFactory dbFactory;
        private static BankAccountModel bankAccount;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            clientBankAccountKey = 100;
            bankAccount = new BankAccountModel("1352", "BranchName", "1352087464", Core.BusinessModel.Enums.ACBType.Current, "John Doe", "System");
            bankAccountDataManager = new BankAccountDataManager(dbFactory);
        };

        private Because of = () =>
        {
            bankAccountDataManager.ReactivateClientBankAccount(clientBankAccountKey);
        };

        private It should_update_system_record = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(
                x => x.Update<LegalEntityBankAccountDataModel>(Param<UpdateClientBankAccountStatusStatement>
                      .Matches(m => m.ClientBankAccountKey == clientBankAccountKey))
            );
        };
    }
}