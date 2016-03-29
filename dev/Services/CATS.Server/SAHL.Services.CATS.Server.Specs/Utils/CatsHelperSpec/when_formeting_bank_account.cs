using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using SAHL.Services.CATS.ConfigExtension;
using SAHL.Services.CATS.Managers;
using SAHL.Services.CATS.Utils;
using SAHL.Services.CATS.Managers.CATS;
using System.IO.Abstractions;

namespace SAHL.Services.CATS.Server.Specs.HandlerSpec
{
    public class when_formeting_bank_account : WithCoreFakes
    {
        private static ICATSManager catsHelper;
        private static BankAccountDataModel Account;
        private static BankAccountDataModel resultingAccount;
        private static string expectedAccountName;


        Establish context = () =>
            {
                Account = new BankAccountDataModel("632005", "9212020000", (int)ACBType.Current, "The Thekwini Warehousing Conduit (Pty) Ltd", null, null);
                resultingAccount = new BankAccountDataModel("632005", "9212020222", (int)ACBType.Current, "P. Miller", null, null);
                expectedAccountName = "The Thekwini Warehousing Condu";
                catsHelper = new CATSManager(An<ICATSDataManager>(), An<IFileSystem>(), An<ICatsAppConfigSettings>());
            };
        Because of = () =>
            {
                resultingAccount = catsHelper.ApplyCATsFormat(Account);
            };

        It Should_tractate_bank_account_name = () =>
        {
            resultingAccount.AccountName.Equals(expectedAccountName).ShouldBeTrue();
        };
    }
}