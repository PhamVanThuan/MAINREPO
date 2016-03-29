using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using SAHL.Services.Interfaces.LifeDomain;
using SAHL.Services.Interfaces.LifeDomain.Commands;
using System;
using System.Linq;

namespace SAHL.Testing.Services.Tests.LifeDomain
{
    [TestFixture]
    public class when_compensating_a_disability_claim : ServiceTestBase<ILifeDomainServiceClient>
    {
        [SetUp]
        public void SetUp()
        {
            base.OnTestSetup();
            var query = new GetLoanAccountWithLifeAccountAndAssuredLifeRoleQuery();
            base.PerformQuery(query);
            var disabilityClaimDataModel = new DisabilityClaimDataModel(query.Result.Results.FirstOrDefault().LifeAccountKey, query.Result.Results.FirstOrDefault().LegalEntityKey,
                DateTime.Now, null, null, null, null, null, null, (int)DisabilityClaimStatus.Pending, null, null, null);
            var insertDisabilityClaimCommand = new InsertDisabilityClaimCommand(disabilityClaimDataModel, base.linkedGuid);
            base.PerformCommand(insertDisabilityClaimCommand);
            linkedKey = linkedKeyManager.RetrieveLinkedKey(this.linkedGuid);
        }

        [Test]
        public void when_successful()
        {
            var command = new CompensateLodgeDisabilityClaimCommand(linkedKey);
            base.Execute(command);
            var query = new GetDisabilityClaimQuery(linkedKey);
            base.PerformQuery(query);
            Assert.That(query.Result.Results.Count().Equals(0));
        }
    }
}