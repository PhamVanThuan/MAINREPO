using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Data.Models.FETest;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.DomainQuery;
using SAHL.Services.Interfaces.DomainQuery.Queries;
using System.Linq;

namespace SAHL.Testing.Services.Tests.DomainQuery
{
    [TestFixture]
    public class when_checking_if_a_client_is_a_natural_person : ServiceTestBase<IDomainQueryServiceClient>
    {
        private ActiveNewBusinessApplicantsDataModel _applicant;

        [Test]
        public void when_successful()
        {
            _applicant = TestApiClient.GetAny<ActiveNewBusinessApplicantsDataModel>(new { isincomecontributor = 1 }, limit: 500);
            int clientKey = _applicant.LegalEntityKey;
            IsClientANaturalPersonQuery query = new IsClientANaturalPersonQuery(clientKey);
            base.Execute<IsClientANaturalPersonQuery>(query);
            Assert.That(query.Result.Results.First().ClientIsANaturalPerson == true, string.Format("Query failed for LegalEntityKey: {0}", query.ClientKey));
        }

        [Test]
        public void when_unsuccessful()
        {
            var client = TestApiClient.Get<LegalEntityDataModel>(new { legalentitytypekey = (int)LegalEntityType.Company }, limit: 1).FirstOrDefault();
            IsClientANaturalPersonQuery query = new IsClientANaturalPersonQuery(client.LegalEntityKey);
            base.Execute<IsClientANaturalPersonQuery>(query);
            Assert.That(query.Result.Results.First().ClientIsANaturalPerson == false);
        }
    }
}