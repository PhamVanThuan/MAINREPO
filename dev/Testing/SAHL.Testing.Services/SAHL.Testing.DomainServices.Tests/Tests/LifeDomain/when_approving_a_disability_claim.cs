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
    public class when_approving_a_disability_claim : ServiceTestBase<ILifeDomainServiceClient>
    {
        private int loanAccountKey;

        [SetUp]
        public void OnSetUp()
        {
            base.OnTestSetup();
            var query = new GetLoanAccountWithLifeAccountAndAssuredLifeRoleQuery();
            base.PerformQuery(query);
            loanAccountKey = query.Result.Results.FirstOrDefault().LoanAccountKey;
            var disabilityClaimDataModel = new DisabilityClaimDataModel(query.Result.Results.FirstOrDefault().LifeAccountKey, query.Result.Results.FirstOrDefault().LegalEntityKey,
                DateTime.Now, DateTime.Now.AddDays(-1), DateTime.Now.AddDays(-5), "Electrical Engineer", (int)DisabilityType.HeadInjury, null, DateTime.Now.AddMonths(19),
                (int)DisabilityClaimStatus.Pending, null, null, null);
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
            var paymentStartDate = DateTime.Now.AddMonths(4);
            var paymentEndDate = DateTime.Now.AddMonths(18);
            var command = new ApproveDisabilityClaimCommand(linkedKey, loanAccountKey, new DateTime(paymentStartDate.Year, paymentStartDate.Month, paymentStartDate.Day),
                14, new DateTime(paymentEndDate.Year, paymentEndDate.Month, paymentEndDate.Day));
            base.Execute(command);
            var query = new GetDisabilityClaimQuery(linkedKey);
            base.PerformQuery(query);
            var disabilityPaymentQuery = new GetDisabilityPaymentsQuery(linkedKey);
            base.PerformQuery(disabilityPaymentQuery);
            Assert.That(command.DisabilityClaimKey == query.Result.Results.FirstOrDefault().DisabilityClaimKey, string.Format("Expected a disability payment record to be found for DisabilityClaimKey: {0}", command.DisabilityClaimKey));
            Assert.That(command.PaymentStartDate == query.Result.Results.FirstOrDefault().PaymentStartDate, string.Format("Expected a disability payment record for DisabilityClaimKey: {0} to have a PaymentStartDate: {1}", command.DisabilityClaimKey,command.PaymentStartDate));
            Assert.That(command.PaymentEndDate == query.Result.Results.FirstOrDefault().PaymentEndDate, string.Format("Expected a disability payment record for DisabilityClaimKey: {0} to have a PaymentEndDate: {1}", command.DisabilityClaimKey,command.PaymentEndDate));
            var disabilityPayments = disabilityPaymentQuery.Result.Results.Where(x => x.DisabilityPaymentStatusKey == (int)DisabilityPaymentStatusEnum.Active).ToList();
            Assert.That(disabilityPaymentQuery.Result.Results.Count().Equals(command.NumberOfInstalmentsAuthorised), string.Format("Expected {0} disability payment records for DisabilityClaimKey: {1} to have a PaymentEndDate: {2}", command.NumberOfInstalmentsAuthorised, command.DisabilityClaimKey, command.PaymentEndDate));
            Assert.That(disabilityPayments.Count().Equals(command.NumberOfInstalmentsAuthorised), string.Format("Expected {0} active disability payment records for DisabilityClaimKey: {1} to have a PaymentEndDate: {2}", command.NumberOfInstalmentsAuthorised, command.DisabilityClaimKey, command.PaymentEndDate));
        }

        [Test]
        public void when_unsuccessful()
        {
            var command = new ApproveDisabilityClaimCommand(linkedKey, loanAccountKey, DateTime.Now.AddMonths(4), 100, DateTime.Now.AddMonths(18));
            base.Execute(command).AndExpectThatErrorMessagesContain("No. of Authorised Instalments cannot be greater than 99.");
            var query = new GetDisabilityClaimQuery(linkedKey);
            base.PerformQuery(query);
            Assert.That(command.DisabilityClaimKey == query.Result.Results.FirstOrDefault().DisabilityClaimKey, string.Format("Expected a disability claim record to be found for DisabilityClaimKey: {0}", command.DisabilityClaimKey));
            Assert.IsNull(query.Result.Results.FirstOrDefault().PaymentStartDate, string.Format("Expected a disability claim record for DisabilityClaimKey: {0} to have a PaymentStartDate: {1}", command.DisabilityClaimKey, command.PaymentStartDate));
            Assert.IsNull(query.Result.Results.FirstOrDefault().PaymentEndDate, string.Format("Expected a disability claim record for DisabilityClaimKey: {0} to have a PaymentEndDate: {1}", command.DisabilityClaimKey, command.PaymentEndDate));
        }
    }
}