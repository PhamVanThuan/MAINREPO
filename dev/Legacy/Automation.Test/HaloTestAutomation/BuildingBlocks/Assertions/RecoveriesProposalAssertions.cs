using BuildingBlocks.Services.Contracts;
using NUnit.Framework;
using System;

namespace BuildingBlocks.Assertions
{
    public static class RecoveriesProposalAssertions
    {
        private static readonly IProposalService proposalService;

        static RecoveriesProposalAssertions()
        {
            proposalService = ServiceLocator.Instance.GetService<IProposalService>();
        }

        /// <summary>
        /// Asserts that an active Recoveries Proposal with the specified criteria exists on the account.
        /// </summary>
        /// <param name="accountKey">AccountKey</param>
        /// <param name="shortfallAmount">Shortfall Amount</param>
        /// <param name="repaymentAmount">Repayment Amount</param>
        /// <param name="date">Start Date</param>
        public static void AssertActiveRecoveriesProposalExists(int accountKey, double shortfallAmount, double repaymentAmount, DateTime date)
        {
            var proposal = proposalService.GetActiveRecoveriesProposalForAccount(accountKey);
            Assert.That(proposal.ShortfallAmount == shortfallAmount && proposal.RepaymentAmount == repaymentAmount && proposal.StartDate.Date == date.Date);
        }
    }
}