using BuildingBlocks.Assertions;
using BuildingBlocks.Services.Contracts;
using Common.Enums;
using ObjectMaps.Pages;
using System.Collections.Generic;
using System.Threading;
using WatiN.Core;

namespace BuildingBlocks.Presenters.LoanServicing
{
    /// <summary>
    ///
    /// </summary>
    public class MarkNonPerforming : MarkNonPerformingControls
    {
        private readonly IWatiNService watinService;

        public MarkNonPerforming()
        {
            watinService = ServiceLocator.Instance.GetService<IWatiNService>();
        }

        public void ClickButton(ButtonTypeEnum button)
        {
            switch (button)
            {
                case ButtonTypeEnum.Proceed:
                    watinService.HandleConfirmationPopup(base.Proceed);
                    break;

                case ButtonTypeEnum.Cancel:
                    base.Cancel.Click();
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Sets the value of the check box
        /// </summary>
        /// <param name="markNonPerforming"></param>
        /// <param name="button"></param>
        public void SetMarkAsNonPerformingCheckbox(bool markNonPerforming, ButtonTypeEnum button)
        {
            base.MarkNonPerformingCheckBox.Checked = markNonPerforming;
            ClickButton(button);
        }

        /// <summary>
        /// Checks if the non performing checkbox exists on the screen
        /// </summary>
        /// <param name="exists"></param>
        public void AssertMarkAsNonPerformingCheckboxExists(bool exists)
        {
            var controls = new List<Element>() { base.MarkNonPerformingCheckBox };
            if (exists)
                WatiNAssertions.AssertFieldsExist(controls);
            else
                WatiNAssertions.AssertFieldsDoNotExist(controls);
        }

        /// <summary>
        /// Checks the state of the mark non performing checkbox
        /// </summary>
        /// <param name="markAsNonPerforming"></param>
        public void AssertMarkAsNonPerformingCheckboxChecked(bool markAsNonPerforming)
        {
            WatiNAssertions.AssertCheckboxValue(markAsNonPerforming, base.MarkNonPerformingCheckBox);
        }

        /// <summary>
        /// Checks whether or not the proceed button exists
        /// </summary>
        /// <param name="exists"></param>
        public void AssertProceedButtonExists(bool exists)
        {
            var controls = new List<Element>() { base.Proceed };
            if (exists)
                WatiNAssertions.AssertFieldsExist(controls);
            else
                WatiNAssertions.AssertFieldsDoNotExist(controls);
        }

        /// <summary>
        /// Checks that the value of the mtd interest field matches the expected value
        /// </summary>
        /// <param name="mtdInterest"></param>
        public void AssertMTDInterestValueVariableLeg(decimal mtdInterest)
        {
            WatiNAssertions.AssertCurrencyLabel(base.MonthToDateInterestAmountVariable, mtdInterest);
        }

        /// <summary>
        /// Checks that the value of the suspended interest field matches the expected value
        /// </summary>
        /// <param name="suspendedInterest"></param>
        public void AssertSuspendedInterestValueVariableLeg(decimal suspendedInterest)
        {
            WatiNAssertions.AssertCurrencyLabel(base.SuspendedInterestAmountVariable, suspendedInterest);
        }

        /// <summary>
        /// Checks that the value of the mtd interest field matches the expected value
        /// </summary>
        /// <param name="mtdInterest"></param>
        public void AssertMTDInterestValueFixedLeg(decimal mtdInterest)
        {
            WatiNAssertions.AssertCurrencyLabel(base.MonthTodateInterestAmountFixed, mtdInterest);
        }

        /// <summary>
        /// Checks that the value of the suspended interest field matches the expected value
        /// </summary>
        /// <param name="suspendedInterest"></param>
        public void AssertSuspendedInterestValueFixedLeg(decimal suspendedInterest)
        {
            Thread.Sleep(2500);
            WatiNAssertions.AssertCurrencyLabel(base.SuspendedInterestAmountFixed, suspendedInterest);
        }
    }
}