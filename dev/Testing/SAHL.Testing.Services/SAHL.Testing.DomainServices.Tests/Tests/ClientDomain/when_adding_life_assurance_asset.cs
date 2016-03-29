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
    public class when_adding_life_assurance_asset : ServiceTestBase<IClientDomainServiceClient>
    {
        [Test]
        public void when_successful()
        {
            var query = new GetApplicantOnActiveApplicationQuery();
            base.PerformQuery(query);
            int clientKey = query.Result.Results.FirstOrDefault().LegalEntityKey;
            var lifeAssuranceAssetModel = new LifeAssuranceAssetModel(CombGuid.Instance.Generate().ToString(), 500000D);
            var command = new AddLifeAssuranceAssetToClientCommand(clientKey, lifeAssuranceAssetModel);
            base.Execute<AddLifeAssuranceAssetToClientCommand>(command);
            var clientAssetQuery = new GetClientAssetsAndLiabilitiesQuery(clientKey);
            base.PerformQuery(clientAssetQuery);
            var clientAssets = clientAssetQuery.Result.Results;
            Assert.That(clientAssets.Where(x => x.AssetTypeDescription == "Life Assurance"
                && x.CompanyName == lifeAssuranceAssetModel.CompanyName
                && x.AssetValue == lifeAssuranceAssetModel.SurrenderValue).First() != null, "Life Assurance Asset not added correctly.");
        }
    }
}