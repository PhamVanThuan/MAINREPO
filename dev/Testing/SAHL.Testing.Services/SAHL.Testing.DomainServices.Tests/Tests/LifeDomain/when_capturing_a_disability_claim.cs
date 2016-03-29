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
    public class when_capturing_a_disability_claim : ServiceTestBase<ILifeDomainServiceClient>
    {
        [SetUp]
        public void OnSetUp()
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
            var dateOfDiagnosis = DateTime.Now.AddDays(-5);
            var lastDateWorked = DateTime.Now.AddDays(-1);
            var expectedReturnToWorkDate = DateTime.Now.AddMonths(12);
            var command = new CapturePendingDisabilityClaimCommand(linkedKey, new DateTime(dateOfDiagnosis.Year, dateOfDiagnosis.Month, dateOfDiagnosis.Day), (int)DisabilityType.Fractureoflimbs,
                "Mental", "Skydiving Instructor", new DateTime(lastDateWorked.Year, lastDateWorked.Month, lastDateWorked.Day), new DateTime(expectedReturnToWorkDate.Year, expectedReturnToWorkDate.Month,
                    expectedReturnToWorkDate.Day));
            base.Execute(command);
            var query = new GetDisabilityClaimQuery(linkedKey);
            base.PerformQuery(query);
            Assert.That(command.DisabilityClaimKey == query.Result.Results.FirstOrDefault().DisabilityClaimKey);
            Assert.That(command.DateOfDiagnosis == query.Result.Results.FirstOrDefault().DateOfDiagnosis);
            Assert.That(command.DisabilityTypeKey == query.Result.Results.FirstOrDefault().DisabilityTypeKey);
            Assert.That(command.OtherDisabilityComments == query.Result.Results.FirstOrDefault().OtherDisabilityComments);
            Assert.That(command.ClaimantOccupation == query.Result.Results.FirstOrDefault().ClaimantOccupation);
            Assert.That(command.LastDateWorked == query.Result.Results.FirstOrDefault().LastDateWorked);
            Assert.That(command.ExpectedReturnToWorkDate == query.Result.Results.FirstOrDefault().ExpectedReturnToWorkDate);
        }

        [Test]
        public void whe_unsuccessful()
        {
            var dateOfDiagnosis = DateTime.Now.AddDays(-5);
            var lastDateWorked = DateTime.Now.AddDays(-1);
            var expectedReturnToWorkDate = DateTime.Now.AddDays(-2);
            var command = new CapturePendingDisabilityClaimCommand(linkedKey, new DateTime(dateOfDiagnosis.Year, dateOfDiagnosis.Month, dateOfDiagnosis.Day), (int)DisabilityType.Cardiac_HeartAttack,
                "The was attack", "Scientist", new DateTime(lastDateWorked.Year, lastDateWorked.Month, lastDateWorked.Day), new DateTime(expectedReturnToWorkDate.Year, expectedReturnToWorkDate.Month,
                    expectedReturnToWorkDate.Day));
            base.Execute(command).AndExpectThatErrorMessagesContain("Expected Return to Work Date cannot be before Claimant's Last Date Worked.");
            var query = new GetDisabilityClaimQuery(linkedKey);
            base.PerformQuery(query);
            Assert.That(command.DisabilityClaimKey == query.Result.Results.FirstOrDefault().DisabilityClaimKey);
            Assert.IsNull(query.Result.Results.FirstOrDefault().DateOfDiagnosis);
            Assert.IsNull(query.Result.Results.FirstOrDefault().DisabilityTypeKey);
            Assert.IsNull(query.Result.Results.FirstOrDefault().OtherDisabilityComments);
            Assert.IsNull(query.Result.Results.FirstOrDefault().ClaimantOccupation);
            Assert.IsNull(query.Result.Results.FirstOrDefault().LastDateWorked);
            Assert.IsNull(query.Result.Results.FirstOrDefault().ExpectedReturnToWorkDate);
        }
    }
}