using Common.Constants;
using ObjectMaps.Pages;
using System.Collections.Generic;
using WatiN.Core;

namespace BuildingBlocks.Presenters.LoanServicing.LoanDetail
{
    public class LoanDetailUpdate : LoanDetailUpdateControls
    {
        /// <summary>
        /// Clicks the update button.
        /// </summary>
        public void ClickUpdate()
        {
            base.Update.Click();
        }

        /// <summary>
        /// A list of elements on the Loan Details Update view that should not exist.
        /// </summary>
        private List<Element> UpdateElementsDoNotExist
        {
            get
            {
                var list = new List<Element> { base.DetailClass, base.DetailType, base.DetailDate, base.Amount, base.TextDescription, base.CancellationType };

                return list;
            }
        }

        /// <summary>
        /// Asserts that the expected list of elements do not exist on the Loan Details Update view.
        /// </summary>
        public void AssertUpdateFieldsDoNotExist()
        {
            Assertions.WatiNAssertions.AssertFieldsDoNotExist(this.UpdateElementsDoNotExist);
        }

        /// <summary>
        /// A list of elements on the Loan Details Update view that should be disabled.
        /// </summary>
        private List<Element> DisabledUpdateElements
        {
            get
            {
                var list = new List<Element> { base.Update };

                return list;
            }
        }

        /// <summary>
        /// Asserts that the expected list of elements are disabled on the Loan Details Update view.
        /// </summary>
        public void AssertUpdateElementsDisabled()
        {
            Assertions.WatiNAssertions.AssertFieldsAreDisabled(this.DisabledUpdateElements);
        }

        /// <summary>
        /// Populates the relevant fields to update the loan detail record.
        /// </summary>
        /// <param name="loanDetail">Loan Detail Model</param>
        /// <param name="cancellationtype">Cancellation Type</param>
        public void PopulateUpdateView(Automation.DataModels.LoanDetail loanDetail, string cancellationtype)
        {
            base.DetailDate.Value = loanDetail.DetailDate.ToString(Formats.DateFormat);
            base.Amount.Value = loanDetail.Amount.ToString();
            base.TextDescription.Value = loanDetail.Description;
            if (base.CancellationType.Enabled && !string.IsNullOrEmpty(cancellationtype))
                base.CancellationType.Option(cancellationtype).Select();
        }
    }
}