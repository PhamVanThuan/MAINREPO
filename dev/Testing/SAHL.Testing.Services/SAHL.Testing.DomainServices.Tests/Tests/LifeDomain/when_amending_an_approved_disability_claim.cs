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
    public class when_amending_an_approved_disability_claim : ServiceTestBase<ILifeDomainServiceClient>
    {
        [SetUp]
        public void OnSetUp()
        {
            base.OnTestSetup();
            var query = new GetLoanAccountWithLifeAccountAndAssuredLifeRoleQuery();
            base.PerformQuery(query);
            var disabilityClaimDataModel = new DisabilityClaimDataModel(query.Result.Results.FirstOrDefault().LifeAccountKey, query.Result.Results.FirstOrDefault().LegalEntityKey,
                DateTime.Now, DateTime.Now.AddDays(-1), DateTime.Now.AddDays(-5), null, (int)DisabilityType.HeadInjury, null, null, (int)DisabilityClaimStatus.Approved, DateTime.Now.AddMonths(3),
                14, DateTime.Now.AddMonths(17));
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
            var expectedReturnToWorkDate = DateTime.Now.AddMonths(20);
            var command = new AmendApprovedDisabilityClaimCommand(linkedKey, (int)DisabilityType.Other, "Beyond Repair", "Ship Captain", DateTime.Now.AddDays(-5),
                new DateTime(expectedReturnToWorkDate.Year, expectedReturnToWorkDate.Month, expectedReturnToWorkDate.Day));
            base.Execute(command);
            var query = new GetDisabilityClaimQuery(linkedKey);
            base.PerformQuery(query);
            Assert.That(command.DisabilityClaimKey == query.Result.Results.FirstOrDefault().DisabilityClaimKey);
            Assert.That(command.DisabilityTypeKey == query.Result.Results.FirstOrDefault().DisabilityTypeKey);
            Assert.That(command.OtherDisabilityComments == query.Result.Results.FirstOrDefault().OtherDisabilityComments);
            Assert.That(command.ClaimantOccupation == query.Result.Results.FirstOrDefault().ClaimantOccupation);
            Assert.That(command.ExpectedReturnToWorkDate == query.Result.Results.FirstOrDefault().ExpectedReturnToWorkDate);
        }

        [Test]
        public void when_unsuccessful()
        {
            var expectedReturnToWorkDate = DateTime.Now.AddDays(-7);
            var command = new AmendApprovedDisabilityClaimCommand(linkedKey, (int)DisabilityType.Other, "Its Bad", "Crane Operator", DateTime.Now.AddDays(-5),
                new DateTime(expectedReturnToWorkDate.Year, expectedReturnToWorkDate.Month, expectedReturnToWorkDate.Day));
            base.Execute(command).AndExpectThatErrorMessagesContain("Expected Return to Work Date cannot be before Claimant's Last Date Worked.");
            var query = new GetDisabilityClaimQuery(linkedKey);
            base.PerformQuery(query);
            Assert.That(command.DisabilityClaimKey == query.Result.Results.FirstOrDefault().DisabilityClaimKey);
            Assert.AreNotEqual(command.DisabilityTypeKey, query.Result.Results.FirstOrDefault().DisabilityTypeKey);
            Assert.AreNotEqual(command.OtherDisabilityComments, query.Result.Results.FirstOrDefault().OtherDisabilityComments);
            Assert.AreNotEqual(command.ClaimantOccupation, query.Result.Results.FirstOrDefault().ClaimantOccupation);
            Assert.AreNotEqual(command.ExpectedReturnToWorkDate, query.Result.Results.FirstOrDefault().ExpectedReturnToWorkDate);
        }
    }
}