using BuildingBlocks.Services.Contracts;
using NUnit.Framework;
using ObjectMaps.Pages;
using System.Collections.Generic;
using System.Linq;

namespace BuildingBlocks.Presenters.DebtCounselling
{
    /// <summary>
    /// DebtCounsellingCancelled
    /// </summary>
    public class DebtCounsellingCancelled : DebtCounsellingCancelledControls
    {
        private readonly IDebtCounsellingService debtCounsellingService;

        public DebtCounsellingCancelled()
        {
            debtCounsellingService = ServiceLocator.Instance.GetService<IDebtCounsellingService>();
        }

        /// <summary>
        /// Cancels the debt counselling case.
        /// </summary>
        /// <param name="CancellationReason">Cancellation Reason</param>
        public void CancelDebtCounselling(string CancellationReason)
        {
            base.CancellationReason.Option(CancellationReason).Select();
            base.Submit.Click();
        }

        /// <summary>
        /// Cancels the debt counselling case without selecting a cancellation reason.
        /// </summary>
        public void CancelDebtCounsellingSubmitNoReason()
        {
            base.Submit.Click();
        }

        /// <summary>
        /// Cancels the process of cancelling a debt counselling case.
        /// </summary>
        public void CancelDebtCounsellingCancelled()
        {
            base.Cancel.Click();
        }

        /// <summary>
        /// Asserts that the correct list of accounts under debt counselling linked to the specified debt counselling case are displayed on the Cancellation screen.
        /// </summary>
        /// <param name="debtCounsellingKey">debtCounsellingKey</param>
        public void AssertGroupedAccountsExistOnCancellation(int debtCounsellingKey)
        {
            int accountKey = 0;
            List<int> accountList = debtCounsellingService.GetRelatedDebtCounsellingAccounts(debtCounsellingKey);
            accountKey = (from acc in accountList where base.GroupedAccountsExist(acc.ToString()) == false select acc).FirstOrDefault();
            Assert.That(accountKey == 0, string.Format("{0} was not found.", accountKey));
        }
    }
}