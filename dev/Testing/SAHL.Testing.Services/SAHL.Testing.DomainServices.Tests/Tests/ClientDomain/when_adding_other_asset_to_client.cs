using NUnit.Framework;
using SAHL.Core.Identity;
using SAHL.Services.Interfaces.ClientDomain;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Models;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System;
using System.Linq;

namespace SAHL.Testing.Services.Tests.ClientDomain
{
    [TestFixture]
    public class when_adding_other_asset_to_client : ServiceTestBase<IClientDomainServiceClient>
    {
        [Test]
        public void when_successful()
        {
            OtherAssetModel model = new OtherAssetModel(
                string.Format("Asset{0}", CombGuid.Instance.Generate()),
                Convert.ToDouble(this.randomizer.Next(0, 150000)),
                Convert.ToDouble(this.randomizer.Next(0, 50000)));
            var query = new GetApplicantOnActiveApplicationQuery();
            base.PerformQuery(query);
            int clientKey = query.Result.Results.FirstOrDefault().LegalEntityKey;
            AddOtherAssetToClientCommand command = new AddOtherAssetToClientCommand(clientKey, model);
            base.Execute<AddOtherAssetToClientCommand>(command);
            var clientAssetQuery = new GetClientAssetsAndLiabilitiesQuery(clientKey);
            base.PerformQuery(clientAssetQuery);
            var clientAssets = clientAssetQuery.Result.Results;
            Assert.That(clientAssets.Where(x => x.AssetTypeDescription == "Other Asset"
                && x.AssetValue == model.AssetValue
                && x.LiabilityValue == model.LiabilityValue
                && x.AssetLiabilityDescription == model.Description).First()
                != null);
        }
    }
}