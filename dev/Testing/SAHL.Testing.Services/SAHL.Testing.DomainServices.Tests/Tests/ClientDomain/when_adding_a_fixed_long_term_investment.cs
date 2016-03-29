using NUnit.Framework;
using SAHL.Core.Identity;
using SAHL.Services.Interfaces.ClientDomain;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Models;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System.Linq;

namespace SAHL.Testing.Services.Tests.ClientDomain
{
    [TestFixture]
    public class when_adding_a_fixed_long_term_investment : ServiceTestBase<IClientDomainServiceClient>
    {
        [Test]
        public void when_successful()
        {
            var query = new GetApplicantOnActiveApplicationQuery();
            base.PerformQuery(query);
            var clientRole = query.Result.Results.FirstOrDefault();
            var model = new FixedLongTermInvestmentLiabilityModel(CombGuid.Instance.GenerateString(), 333500D);
            var command = new AddFixedLongTermInvestmentLiabilityToClientCommand(model, clientRole.LegalEntityKey);
            base.Execute<AddFixedLongTermInvestmentLiabilityToClientCommand>(command);
            var clientAssetQuery = new GetClientAssetsAndLiabilitiesQuery(command.ClientKey);
            base.PerformQuery(clientAssetQuery);
            var clientAssets = clientAssetQuery.Result.Results;
            Assert.That(clientAssets.Where(x => x.AssetTypeDescription == "Fixed Long Term Investment"
                && x.LiabilityValue == model.LiabilityValue
                && x.CompanyName == model.CompanyName).First() != null, "Fixed Long Term Investment not added correctly");
        }
    }
}