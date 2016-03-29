using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using SAHL.Services.Interfaces.LifeDomain;
using SAHL.Services.Interfaces.LifeDomain.Commands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Testing.Services.Tests.LifeDomain
{
    [TestFixture]
    public class when_repudiating_a_disability_claim : ServiceTestBase<ILifeDomainServiceClient>
    {
        [SetUp]
        public void OnSetUp()
        {
            base.OnTestSetup();
            var query = new GetLoanAccountWithLifeAccountAndAssuredLifeRoleQuery();
            base.PerformQuery(query);
            var disabilityClaimDataModel = new DisabilityClaimDataModel(query.Result.Results.FirstOrDefault().LifeAccountKey, query.Result.Results.FirstOrDefault().LegalEntityKey,
                DateTime.Now, DateTime.Now.AddDays(-1), DateTime.Now.AddDays(-5), "Soldier", (int)DisabilityType.Depression_PTSD, null, null, (int)DisabilityClaimStatus.Pending, null,
                null, null);
            var insertDisabilityClaimCommand = new InsertDisabilityClaimCommand(disabilityClaimDataModel, base.linkedGuid);
            base.PerformCommand(insertDisabilityClaimCommand);
            linkedKey = linkedKeyManager.RetrieveLinkedKey(this.linkedGuid);
        }

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
            var command = new RepudiateDisabilityClaimCommand(linkedKey, new List<int>() { 123, 456, 789 });
            base.Execute(command);
            var query = new GetDisabilityClaimQuery(linkedKey);
            base.PerformQuery(query);
            Assert.That(command.DisabilityClaimKey == query.Result.Results.FirstOrDefault().DisabilityClaimKey);
            Assert.That((int)DisabilityClaimStatus.Repudiated == query.Result.Results.FirstOrDefault().DisabilityClaimStatusKey);
        }
    }
}