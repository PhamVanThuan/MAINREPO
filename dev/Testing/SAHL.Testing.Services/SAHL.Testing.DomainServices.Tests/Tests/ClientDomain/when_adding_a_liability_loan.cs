using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Services.Interfaces.ClientDomain;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Models;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System;
using System.Linq;

namespace SAHL.Testing.Services.Tests.ClientDomain
{
    [TestFixture]
    public class when_adding_a_liability_loan : ServiceTestBase<IClientDomainServiceClient>
    {
        [Test]
        public void when_successful()
        {
            var model = new LiabilityLoanModel(Core.BusinessModel.Enums.AssetLiabilitySubType.PersonalLoan, "ABSA", DateTime.Now.AddYears(+1), 2500D, 35000D);
            var query = new GetApplicantOnActiveApplicationQuery();
            base.PerformQuery(query);
            int clientKey = query.Result.Results.FirstOrDefault().LegalEntityKey;
            var command = new AddLiabilityLoanToClientCommand(clientKey, model);
            base.Execute<AddLiabilityLoanToClientCommand>(command);
            var clientAssetQuery = new GetClientAssetsAndLiabilitiesQuery(clientKey);
            base.PerformQuery(clientAssetQuery);
            var clientAssetsLiabilities = clientAssetQuery.Result.Results;
            Assert.That(clientAssetsLiabilities.Where(x => x.AssetTypeDescription == "Liability Loan"
                && x.LiabilityValue == model.LiabilityValue
                && x.AssetLiabilitySubTypeKey == (int)AssetLiabilitySubType.PersonalLoan
                && x.Cost == model.InstalmentValue
                && x.LiabilityValue == model.LiabilityValue).First() != null, "Liability Loan not added correctly.");
        }
    }
}