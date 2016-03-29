using NUnit.Framework;
using SAHL.Services.Interfaces.ClientDomain;
using SAHL.Services.Interfaces.ClientDomain.Models;
using SAHL.Services.Interfaces.ClientDomain.Queries;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System.Linq;

namespace SAHL.Testing.Services.Tests.ClientDomain
{
    [TestFixture]
    internal class when_checking_for_existing_client_by_id_number : ServiceTestBase<IClientDomainServiceClient>
    {
        [Test]
        public void when_successful()
        {
            GetNaturalPersonClientQuery getNaturalPersonClientQuery = new GetNaturalPersonClientQuery(isActive: true);
            base.PerformQuery(getNaturalPersonClientQuery);
            var applicant = getNaturalPersonClientQuery.Result.Results.First();
            FindClientByIdNumberQuery query = new FindClientByIdNumberQuery(applicant.IDNumber);
            base.Execute<FindClientByIdNumberQuery>(query);
            ClientDetailsQueryResult client = query.Result.Results.FirstOrDefault();
            Assert.That(applicant.IDNumber == client.IDNumber && client.LegalEntityKey == applicant.LegalEntityKey);
        }

        [Test]
        public void when_unsuccessful()
        {
            var query = new GetUnusedIDNumberQuery();
            base.PerformQuery(query);
            string IdNumber = query.Result.Results.FirstOrDefault().IDNumber;
            FindClientByIdNumberQuery findClientByIdNumberQuery = new FindClientByIdNumberQuery(IdNumber);
            base.Execute<FindClientByIdNumberQuery>(findClientByIdNumberQuery);
            Assert.IsNull(findClientByIdNumberQuery.Result.Results.FirstOrDefault());
        }
    }
}