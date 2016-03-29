using BuildingBlocks.Presenters.CommonPresenters;
using ObjectMaps.Pages;

namespace BuildingBlocks.Presenters.Origination.FurtherLending
{
    public class FurtherLendingApprove : FurtherLendingApplicationApproveControls
    {
        /// <summary>
        /// Approves an application that does not require a decision to be made on Quick Cash
        /// </summary>
        /// <param name="b"></param>
        public void Approve()
        {
            base.btnApprove.Click();
            base.Document.Page<BasePage>().DomainWarningClickYes();
        }

        /// <summary>
        /// Approves an application as well as approving Quick Cash
        /// </summary>
        /// <param name="b"></param>
        public void ApproveWithQC()
        {
            base.btnQuickCash.Click();
            base.Document.Page<BasePage>().DomainWarningClickYes();
            base.Document.Page<FurtherLendingQCApprove>().ApproveQC();
            base.btnApprove.Click();
            base.Document.Page<BasePage>().DomainWarningClickYes();
        }

        /// <summary>
        /// Approves an application but declines Quick Cash
        /// </summary>
        /// <param name="b"></param>
        public void ApproveWithDeclinedQC()
        {
            base.btnQuickCash.Click();
            base.Document.Page<BasePage>().DomainWarningClickYes();
            base.Document.Page<FurtherLendingQCApprove>().DeclineQC();
            base.btnApprove.Click();
            base.Document.Page<BasePage>().DomainWarningClickYes();
        }
    }
}