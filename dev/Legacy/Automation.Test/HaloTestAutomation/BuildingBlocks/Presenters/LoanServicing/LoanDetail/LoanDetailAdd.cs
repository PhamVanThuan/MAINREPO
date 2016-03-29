using Common.Constants;
using ObjectMaps.Pages;
using WatiN.Core;

namespace BuildingBlocks.Presenters.LoanServicing.LoanDetail
{
    public class LoanDetailAdd : LoanDetailAddControls
    {
        public void PopulateView(Automation.DataModels.LoanDetail loanDetail, string cancellationtype)
        {
            base.DetailClass.Option(loanDetail.LoanDetailType.LoanDetailClass.Description).Select();
            base.DetailType.Option(loanDetail.LoanDetailType.Description).Select();
            base.DetailDate.Value = loanDetail.DetailDate.ToString(Formats.DateFormat);
            base.Amount.Value = loanDetail.Amount.ToString();
            base.TextDescription.Value = loanDetail.Description;
            if (base.CancellationType.Enabled && !string.IsNullOrEmpty(cancellationtype))
                base.CancellationType.Option(cancellationtype).Select();
        }

        /// <summary>
        /// Clicks the add button.
        /// </summary>
        public void ClickAdd()
        {
            base.Submit.Click();
        }

        /// <summary>
        /// Clicks the cancel button.
        /// </summary>
        public void ClickCancel()
        {
            base.Cancel.Click();
        }

        /// <summary>
        /// Fethces the list of detail classes.
        /// </summary>
        /// <returns>List of Detail Classes</returns>
        public SelectList GetDetailClassList()
        {
            return base.DetailClass;
        }

        /// <summary>
        /// Fetches the list of detail types.
        /// </summary>
        /// <returns>List of Detail Types</returns>
        public SelectList GetDetailTypeList()
        {
            return base.DetailType;
        }

        /// <summary>
        /// Selects the specified detail class from the dropdown.
        /// </summary>
        /// <param name="detailClass">Detail Class</param>
        public void SelectDetailClass(string detailClass)
        {
            base.DetailClass.Option(detailClass).Select();
        }

        /// <summary>
        /// Selects the specified detail type from the dropdown.
        /// </summary>
        /// <param name="detailType">Detail Type</param>
        public void SelectDetailType(string detailType)
        {
            base.DetailType.Option(detailType).Select();
        }

        /// <summary>
        /// Fetches the list of cancellation types.
        /// </summary>
        /// <returns>List of Cancellation Types</returns>
        public SelectList GetCancellationTypeList()
        {
            return base.CancellationType;
        }
    }
}