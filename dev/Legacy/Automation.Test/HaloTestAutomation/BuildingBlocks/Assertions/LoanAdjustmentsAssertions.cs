using Automation.DataAccess;
using BuildingBlocks.Services.Contracts;
using Common.Enums;
using NUnit.Framework;
using WatiN.Core.Logging;

namespace BuildingBlocks.Assertions
{
    /// <summary>
    ///
    /// </summary>
    public static class LoanAdjustmentsAssertions
    {
        private static readonly IX2WorkflowService x2Service;

        static LoanAdjustmentsAssertions()
        {
            x2Service = ServiceLocator.Instance.GetService<IX2WorkflowService>();
        }

        /// <summary>
        /// This asserts that a Loan Adjustment instance of the provided type exists at the specific state for the given accountkey
        /// </summary>
        /// <param name="accountKey">AccountKey</param>
        /// <param name="type">Loan Adjustment Type</param>
        /// <param name="state">Workflow State</param>
        public static int AssertCurrentState(int accountKey, int type, string state)
        {
            QueryResults results = x2Service.GetLoanAdjustmentInstance(accountKey, type, state);
            Logger.LogAction(string.Format(@"Asserting that a Loan Adjustment instance exists at the {0} state for Account {1}", state, accountKey));
            Assert.True(results.HasResults, string.Format("No Loan Adjustment instance exists at the {0} state for Account {1}", state, accountKey));
            return results.Rows(0).Column("InstanceID").GetValueAs<int>();
        }

        /// <summary>
        /// This asserts that all applicable loan transactions for the SPV transfer are written correctly
        /// </summary>
        /// <param name="accountKey"></param>
        public static void AssertSPVMovementTransactions(int accountKey)
        {
            Logger.LogAction("Asserting that transaction type '(900) SPV Transfer' has been inserted");
            TransactionAssertions.AssertLoanTransactionExists(accountKey, TransactionTypeEnum.SPVTransfer);

            Logger.LogAction("Asserting that transaction type '(940) Instalment Change' has been inserted");
            TransactionAssertions.AssertLoanTransactionExists(accountKey, TransactionTypeEnum.InstallmentChange);
        }

        /// <summary>
        /// Asserts whether the timeout activity has occured against the loan adjustments workflow instance
        /// </summary>
        /// <param name="accountKey"></param>
        public static void AssertTimeOut(int accountKey)
        {
            Logger.LogAction(string.Format("Asserting that the Time Out timer fired against the Term Change case for account {0}", accountKey));
            Assert.True(x2Service.IsActivityTimedOut(accountKey),
                string.Format("Time Out activity has not fired against the Term Change case for account {0}", accountKey));
        }
    }
}