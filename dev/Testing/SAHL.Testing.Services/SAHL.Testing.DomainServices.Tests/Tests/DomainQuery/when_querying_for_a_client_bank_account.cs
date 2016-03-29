using NUnit.Framework;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.DomainQuery;
using SAHL.Services.Interfaces.DomainQuery.Queries;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System;
using System.Linq;

namespace SAHL.Testing.Services.Tests.DomainQuery
{
    [TestFixture]
    public class when_querying_for_a_client_bank_account : ServiceTestBase<IDomainQueryServiceClient>
    {
        [Test]
        public void when_successful()
        {
            var query = new GetApplicationWithApplicantBankAccountQuery(false);
            base.PerformQuery(query);
            var model = query.Result.Results.FirstOrDefault();
            var bankAccount = TestApiClient.GetByKey<LegalEntityBankAccountDataModel>(model.ClientBankAccountKey);
            var getClientBankAccountQuery = new GetClientBankAccountQuery(model.ClientBankAccountKey);
            base.Execute<GetClientBankAccountQuery>(getClientBankAccountQuery);
            var bankAccountExists = getClientBankAccountQuery.Result.Results.Where(x => x.BankAccountKey == bankAccount.BankAccountKey).First();
            Assert.IsNotNull(bankAccountExists);
        }

        [Test]
        public void when_unsuccessful()
        {
            var query = new GetClientBankAccountQuery(Int32.MaxValue);
            base.Execute<GetClientBankAccountQuery>(query);
            Assert.AreEqual(query.Result.Results.Count(), 0d);
        }
    }
}