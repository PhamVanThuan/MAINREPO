using BuildingBlocks.Services.Contracts;
using ObjectMaps.Pages;
using System.Collections.Generic;
using WatiN.Core;

namespace BuildingBlocks.Presenters.LoanServicing.LoanDetail
{
    public class LoanDetailDelete : LoanDetailDeleteControls
    {
        private readonly IWatiNService watinService;

        public LoanDetailDelete()
        {
            watinService = ServiceLocator.Instance.GetService<IWatiNService>();
        }

        /// <summary>
        /// Clicks the delete button.
        /// </summary>
        public void ClickDelete()
        {
            watinService.HandleConfirmationPopup(base.Delete);
        }

        /// <summary>
        /// A list of elements on the Loan Details Delete view that should not exist.
        /// </summary>
        private List<Element> DeleteElementsDoNotExist
        {
            get
            {
                var list = new List<Element> { base.DetailClass, base.DetailType, base.DetailDate, base.Amount, base.TextDescription, base.CancellationType };

                return list;
            }
        }

        /// <summary>
        /// Asserts that the expected list of elements do not exist on the Loan Details Delete view.
        /// </summary>
        public void AssertDeleteFieldsDoNotExist()
        {
            Assertions.WatiNAssertions.AssertFieldsDoNotExist(this.DeleteElementsDoNotExist);
        }

        /// <summary>
        /// A list of elements on the Loan Details Delete view that should be disabled.
        /// </summary>
        private List<Element> DisabledDeleteElements
        {
            get
            {
                var list = new List<Element> { base.Delete };

                return list;
            }
        }

        /// <summary>
        /// Asserts that the expected list of elements are disabled on the Loan Details Delete view.
        /// </summary>
        public void AssertDeleteElementsDisabled()
        {
            Assertions.WatiNAssertions.AssertFieldsAreDisabled(this.DisabledDeleteElements);
        }
    }
}