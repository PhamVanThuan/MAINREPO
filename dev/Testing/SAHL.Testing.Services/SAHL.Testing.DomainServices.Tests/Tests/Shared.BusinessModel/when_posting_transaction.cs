using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.FrontEndTest;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System;
using System.Linq;

namespace SAHL.Testing.Services.Tests.Shared.BusinessModel
{
    [TestFixture]
    public class when_posting_transaction : ServiceTestBase<IFrontEndTestServiceClient>
    {
        private FinancialServiceDataModel _financialService;
        private string _user;

        [SetUp]
        public void onSetup()
        {
            _user = @"SAHL\HaloUser";
            var account = TestApiClient.Get<AccountDataModel>(new { AccountStatusKey = (int)AccountStatus.Open, ProductKey = (int)Product.NewVariableLoan }, limit: 500)
                .OrderBy(x => Guid.NewGuid()).Take(1).FirstOrDefault();
            _financialService = TestApiClient.Get<FinancialServiceDataModel>(new { AccountKey = account.AccountKey, FinancialServiceTypeKey = 1 }).FirstOrDefault();
        }

        [Test]
        public void when_successful()
        {
            int transactionValue = new Random().Next(1, 1000);
            string transactionReference = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

            var command = new PostTransactionCommand(_financialService.FinancialServiceKey, 220, Convert.ToDecimal(transactionValue), DateTime.Now, transactionReference, _user);
            Execute(command).WithoutErrors();
            var query = new GetFinancialTransactionsQuery(command.financialServiceKey);
            base.PerformQuery(query);
            var financialTransactions = query.Result.Results;
            CollectionAssert.IsNotEmpty(financialTransactions.Where(x => x.TransactionTypeKey == 220
                && x.Amount == Convert.ToDecimal(command.amount) && x.Reference == command.reference && x.UserID == command.userId));
        }

        [Test]
        public void when_unsuccessful()
        {
            int transactionValue = new Random().Next(1, 1000);
            string transactionReference = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            var command = new PostTransactionCommand(_financialService.FinancialServiceKey, 99999999, Convert.ToDecimal(transactionValue), DateTime.Now, transactionReference, _user);
            Execute(command).AndExpectThatErrorMessagesContain("TransactionType provided is not valid");
            var query = new GetFinancialTransactionsQuery(command.financialServiceKey);
            base.PerformQuery(query);
            var financialTransactions = query.Result.Results;
            CollectionAssert.IsEmpty(financialTransactions.Where(x => x.FinancialTransactionKey == 99999999));
        }
    }
}