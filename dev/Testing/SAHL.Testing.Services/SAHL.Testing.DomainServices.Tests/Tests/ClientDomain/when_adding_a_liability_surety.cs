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
    public class when_adding_a_liability_surety : ServiceTestBase<IClientDomainServiceClient>
    {
        [Test]
        public void when_successful()
        {
            var query = new GetApplicantOnActiveApplicationQuery();
            base.PerformQuery(query);
            int clientKey = query.Result.Results.FirstOrDefault().LegalEntityKey;
            var model = new LiabilitySuretyModel(125000D, 250000D, CombGuid.Instance.Generate().ToString());
            var command = new AddLiabilitySuretyToClientCommand(model, clientKey);
            base.Execute<AddLiabilitySuretyToClientCommand>(command);
            var clientAssetQuery = new GetClientAssetsAndLiabilitiesQuery(clientKey);
            base.PerformQuery(clientAssetQuery);
            var clientAssets = clientAssetQuery.Result.Results;
            Assert.That(clientAssets.Where(x => x.AssetTypeDescription == "Liability Surety"
                && x.LiabilityValue == model.LiabilityValue
                && x.AssetValue == model.AssetValue
                && x.AssetLiabilityDescription == model.Description).First() != null, "Liability Surety not added correctly.");
        }
    }
}