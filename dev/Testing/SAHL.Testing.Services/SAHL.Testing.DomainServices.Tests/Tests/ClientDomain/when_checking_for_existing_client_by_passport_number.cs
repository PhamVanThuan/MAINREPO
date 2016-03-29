using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models.FETest;
using SAHL.Core.Identity;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.ClientDomain;
using SAHL.Services.Interfaces.ClientDomain.Queries;
using System.Linq;

namespace SAHL.Testing.Services.Tests.ClientDomain
{
    [TestFixture]
    public class when_checking_for_existing_client_by_passport_number : ServiceTestBase<IClientDomainServiceClient>
    {
        [Test]
        public void when_successful()
        {
            var foreigner = TestApiClient.Get<ForeignNaturalPersonClientsDataModel>(new { citizenshiptypekey = (int)CitizenType.Non_Resident_ContractWorker }).First();
            FindClientByPassportNumberQuery query = new FindClientByPassportNumberQuery(foreigner.PassportNumber);
            base.Execute<FindClientByPassportNumberQuery>(query);
            string passportNumber = query.Result.Results.FirstOrDefault().PassportNumber;
            Assert.That(foreigner.PassportNumber == passportNumber);
        }

        [Test]
        public void when_unsuccessful()
        {
            var query = new FindClientByPassportNumberQuery(CombGuid.Instance.GenerateString());
            base.Execute<FindClientByPassportNumberQuery>(query);
            Assert.IsNull(query.Result.Results.FirstOrDefault());
        }
    }
}