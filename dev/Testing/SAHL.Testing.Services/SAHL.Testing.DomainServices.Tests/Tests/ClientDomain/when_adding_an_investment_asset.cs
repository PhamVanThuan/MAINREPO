using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Identity;
using SAHL.Services.Interfaces.ClientDomain;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Models;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System.Linq;

namespace SAHL.Testing.Services.Tests.ClientDomain
{
    [TestFixture]
    public class when_adding_an_investment_asset : ServiceTestBase<IClientDomainServiceClient>
    {
        [Test]
        public void when_successful()
        {
            var query = new GetApplicantOnActiveApplicationQuery();
            base.PerformQuery(query);
            int clientKey = query.Result.Results.FirstOrDefault().LegalEntityKey;
            var model = new InvestmentAssetModel(AssetInvestmentType.ListedInvestments, CombGuid.Instance.GenerateString(), 500000D);
            var command = new AddInvestmentAssetToClientCommand(clientKey, model);
            base.Execute<AddInvestmentAssetToClientCommand>(command);
            var clientAssetQuery = new GetClientAssetsAndLiabilitiesQuery(clientKey);
            base.PerformQuery(clientAssetQuery);
            var clientAssets = clientAssetQuery.Result.Results;
            Assert.That(clientAssets.Where(x => x.AssetTypeDescription == "Listed Investments"
                && x.AssetValue == model.AssetValue
                && x.CompanyName == model.CompanyName).First() != null, "Liability Loan not added correctly.");
        }
    }
}