using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.CATS.Managers;
using SAHL.Services.CATS.Managers.Statements;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.CATS.Server.Specs.Managers.CatsDataManagerSpec
{
    public class when_getting_bank_account_by_key : WithFakes
    {
        private static ICATSDataManager dataManager;
        private static int bankAccountKey;
        private static IDbFactory dbFactory;
        private static BankAccountDataModel bankAccount;
        private static BankAccountDataModel expectedBankAccount;

        Establish context = () =>
        {
            bankAccountKey = 44;
            dbFactory = new FakeDbFactory();
            dataManager = new CATSDataManager(dbFactory);
            expectedBankAccount =  new BankAccountDataModel("632005", "9212020000", (int)ACBType.Current, "P. Miller", null, null);
            dbFactory.NewDb().InReadOnlyAppContext().WhenToldTo(x =>
                x.SelectOne<BankAccountDataModel>(Param.IsAny<GetBankAccountDataModelByKeyStatement>()))
                .Return(expectedBankAccount);
        };

        Because of = () =>
        {
            bankAccount = dataManager.GetBankingAccountByKey(bankAccountKey);
        };

        It should_return_bank_account = () =>
        {
            bankAccount.ShouldEqual(expectedBankAccount);
        };
    }
}
