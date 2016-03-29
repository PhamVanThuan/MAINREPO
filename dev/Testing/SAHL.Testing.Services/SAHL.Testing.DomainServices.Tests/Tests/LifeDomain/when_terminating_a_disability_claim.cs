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
    public class when_terminating_a_disability_claim : ServiceTestBase<ILifeDomainServiceClient>
    {
        [SetUp]
        public void OnSetUp()
        {
            base.OnTestSetup();
            var query = new GetLoanAccountWithLifeAccountAndAssuredLifeRoleQuery();
            base.PerformQuery(query);
            var disabilityClaimDataModel = new DisabilityClaimDataModel(query.Result.Results.FirstOrDefault().LifeAccountKey, query.Result.Results.FirstOrDefault().LegalEntityKey,
                DateTime.Now, DateTime.Now.AddDays(-1), DateTime.Now.AddDays(-5), "Gold Miner", (int)DisabilityType.Respiratory_Asthma_COPD, null, null, (int)DisabilityClaimStatus.Approved, DateTime.Now.AddMonths(4),
                5, DateTime.Now.AddMonths(9));
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
            var command = new TerminateDisabilityClaimCommand(linkedKey, 123);
            base.Execute(command);
            var query = new GetDisabilityClaimQuery(linkedKey);
            base.PerformQuery(query);
            var disabilityPaymentQuery = new GetDisabilityPaymentsQuery(linkedKey);
            base.PerformQuery(disabilityPaymentQuery);
            var disabilityPayments = disabilityPaymentQuery.Result.Results.Where(x => x.DisabilityPaymentStatusKey == (int)DisabilityPaymentStatusEnum.Active).ToList();
            Assert.AreEqual((int)DisabilityClaimStatus.Terminated, query.Result.Results.FirstOrDefault().DisabilityClaimStatusKey);
            Assert.That(disabilityPayments.Count().Equals(0));
        }
    }
}