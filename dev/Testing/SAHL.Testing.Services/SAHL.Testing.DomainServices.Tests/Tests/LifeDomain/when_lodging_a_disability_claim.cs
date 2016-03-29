using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using SAHL.Services.Interfaces.LifeDomain;
using SAHL.Services.Interfaces.LifeDomain.Commands;
using System.Linq;

namespace SAHL.Testing.Services.Tests.LifeDomain
{
    [TestFixture]
    public class when_lodging_a_disability_claim : ServiceTestBase<ILifeDomainServiceClient>
    {
        [TearDown]
        public void OnTearDown()
        {
            var removeDisabilityClaimCommand = new RemoveDisabilityClaimCommand(linkedKey);
            base.PerformCommand(removeDisabilityClaimCommand);
            base.OnTestTearDown();
        }

        [Test]
        public void when_successful()
        {
            var query = new GetLoanAccountWithLifeAccountAndAssuredLifeRoleQuery();
            base.PerformQuery(query);
            var command = new LodgeDisabilityClaimCommand(query.Result.Results.FirstOrDefault().LifeAccountKey, query.Result.Results.FirstOrDefault().LegalEntityKey, base.linkedGuid);
            base.Execute(command);
            linkedKey = linkedKeyManager.RetrieveLinkedKey(this.linkedGuid);
            var disabilityClaim = new GetDisabilityClaimQuery(linkedKey);
            base.PerformQuery(disabilityClaim);
            Assert.That(linkedKey == disabilityClaim.Result.Results.FirstOrDefault().DisabilityClaimKey);
            Assert.That(command.LifeAccountKey == disabilityClaim.Result.Results.FirstOrDefault().AccountKey);
            Assert.That(command.ClaimantLegalEntityKey == disabilityClaim.Result.Results.FirstOrDefault().LegalEntityKey);
            Assert.That((int)DisabilityClaimStatus.Pending == disabilityClaim.Result.Results.FirstOrDefault().DisabilityClaimStatusKey);
        }

        [Test]
        public void when_unsuccessful()
        {
            var query = new GetLoanAccountWithLifeAccountAndAssuredLifeRoleQuery();
            base.PerformQuery(query);
            var command = new LodgeDisabilityClaimCommand(query.Result.Results.FirstOrDefault().LifeAccountKey, query.Result.Results.FirstOrDefault().LegalEntityKey, base.linkedGuid);
            base.Execute(command);
            base.Execute(command).AndExpectThatErrorMessagesContain("The client already has a pending or approved disability claim.");
        }
    }
}