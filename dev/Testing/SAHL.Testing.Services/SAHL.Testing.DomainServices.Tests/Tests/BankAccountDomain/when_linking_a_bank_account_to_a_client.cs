using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Identity;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.BankAccountDomain;
using SAHL.Services.Interfaces.BankAccountDomain.Commands;
using SAHL.Services.Interfaces.BankAccountDomain.Models;
using SAHL.Services.Interfaces.FrontEndTest.Models;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System.Linq;

namespace SAHL.Testing.Services.Tests.BankAccountDomain
{
    [TestFixture]
    public class when_linking_a_bank_account_to_a_client : ServiceTestBase<IBankAccountDomainServiceClient>
    {
        private GetApplicantOnActiveApplicationQueryResult _clientRole;

        [SetUp]
        public void OnSetup()
        {
            var query = new GetApplicantOnActiveApplicationQuery();
            base.PerformQuery(query);
            _clientRole = query.Result.Results.FirstOrDefault();
        }

        [TearDown]
        public void OnTeardown()
        {
            _clientRole = null;
        }

        [Test]
        public void when_successful()
        {
            GetUnusedTestBankAccountQuery query = new GetUnusedTestBankAccountQuery();
            base.PerformQuery(query);
            var bankAccountDataModel = query.Result.Results.FirstOrDefault();
            var bankAccountModel = new BankAccountModel(bankAccountDataModel.ACBBranchCode, bankAccountDataModel.ACBBranchDescription, bankAccountDataModel.AccountNumber, bankAccountDataModel.ACBTypeNumber, "Test User", "System");
            var command = new LinkBankAccountToClientCommand(bankAccountModel, _clientRole.LegalEntityKey, this.linkedGuid);
            base.Execute<LinkBankAccountToClientCommand>(command);
            this.linkedKey = linkedKeyManager.RetrieveLinkedKey(this.linkedGuid);

            LegalEntityBankAccountDataModel clientBankAccount = TestApiClient.GetByKey<LegalEntityBankAccountDataModel>(linkedKey);
            Assert.IsTrue(clientBankAccount.LegalEntityKey == _clientRole.LegalEntityKey);
            BankAccountDataModel bankAccount = TestApiClient.GetByKey<BankAccountDataModel>(clientBankAccount.BankAccountKey);
            Assert.IsTrue(bankAccount.AccountName == bankAccountModel.AccountName
                && bankAccount.AccountNumber == bankAccountModel.AccountNumber
                && bankAccount.ACBBranchCode == bankAccountModel.BranchCode
                && bankAccount.ACBTypeNumber == (int)bankAccountModel.AccountType);
        }

        [Test]
        public void when_unsuccessful()
        {
            var clientBankAccountGuid = CombGuid.Instance.Generate();
            var bankAccountModel = new BankAccountModel("632005", "ABSA ELECTRONIC SETTLEMENT CNT", "1234567890", ACBType.Current, "Test User", "System");
            var command = new LinkBankAccountToClientCommand(bankAccountModel, _clientRole.LegalEntityKey, clientBankAccountGuid);
            base.Execute<LinkBankAccountToClientCommand>(command).AndExpectThatErrorMessagesContain("Bank Account failed CDV validation.");
        }
    }
}