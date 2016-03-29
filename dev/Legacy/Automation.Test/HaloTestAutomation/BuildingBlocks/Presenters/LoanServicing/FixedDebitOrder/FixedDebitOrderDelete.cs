using BuildingBlocks.Services.Contracts;
using NUnit.Framework;
using ObjectMaps.Pages;

namespace BuildingBlocks.Presenters.LoanServicing.FixedDebitOrders
{
    public class FixedDebitOrderDelete : FixedDebitOrderDeleteControls
    {
        private readonly IWatiNService watinService;

        public FixedDebitOrderDelete()
        {
            watinService = ServiceLocator.Instance.GetService<IWatiNService>();
        }

        /// <summary>
        /// Checks that the delete button exists or does not exist on the view.
        /// </summary>
        public void AssertDeleteButtonExists(bool exists)
        {
            Assert.That(btnDelete.Exists == exists);
        }

        /// <summary>
        /// Selects a future dated change debit order by using it key
        /// </summary>
        /// <param name="futureDatedChangeKey">futureDatedChangeKey</param>
        public void SelectFutureDatedFixedDebitOrder(int futureDatedChangeKey)
        {
            base.FutureDatedDebitOrderGridSelectRow(futureDatedChangeKey.ToString()).Click();
        }

        /// <summary>
        /// Delete the fixed debit order
        /// </summary>
        /// <param name="fdcKey"></param>
        public void DeleteFixedDebitOrder(int fdcKey)
        {
            this.SelectFutureDatedFixedDebitOrder(fdcKey);
            watinService.HandleConfirmationPopup(this.btnDelete);
        }
    }
}