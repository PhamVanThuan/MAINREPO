using BuildingBlocks.Services.Contracts;
using NUnit.Framework;
using ObjectMaps.Pages;
using System;

namespace BuildingBlocks.Presenters.CommonPresenters
{
    /// <summary>
    /// Workflow Yes/No Screen
    /// </summary>
    public class WorkflowYesNo : WorkflowYesNoControls
    {
        private readonly IWatiNService watinService;

        public WorkflowYesNo()
        {
            watinService = ServiceLocator.Instance.GetService<IWatiNService>();
        }

        /// <summary>
        /// Handles the Workflow Yes/No screen
        /// </summary>
        /// <param name="Yes">TRUE = click Yes</param>
        /// <param name="continueWithWarnings">True = continue past any domain warnings</param>
        public void Confirm(bool Yes, bool continueWithWarnings)
        {
            if (Yes)
                base.btnYes.Click();
            else
                base.btnNo.Click();
            //continue with warnings
            if (continueWithWarnings)
            {
                base.Document.Page<BasePage>().DomainWarningClickYes();
            }
        }

        /// <summary>
        /// This continues the process once a validation warning message is displayed and handles the subsequent dialogue box popup
        /// </summary>
        /// <param name="HandlePopUp">Handles the dialogue box popup</param>
        public void ContinueWithWarnings(bool HandlePopUp)
        {
            if (HandlePopUp)
            {
                if (base.divValidationSummaryBody.Exists)
                {
                    var buttonCount = base.divValidationSummaryBody.Buttons.Count;
                    Assert.Greater(buttonCount, 0, "The popup validation summary was present with no buttons");
                    watinService.HandleConfirmationPopup(base.divValidationSummaryBody.Buttons[0]);
                }
            }
            else
            {
                if (base.divValidationSummaryBody.Exists)
                {
                    base.divValidationSummaryBody.Buttons[0].Click();
                }
            }
        }

        /// <summary>
        /// This continues the process once a validation warning message is displayed
        /// </summary>
        public void DoNotContinueWithWarnings()
        {
            if (base.divValidationSummaryBody.Exists)
            {
                base.divValidationSummaryBody.Buttons[1].Click();
            }
        }

        /// <summary>
        /// clicks the yes button
        /// </summary>
        public void ClickYes()
        {
            base.btnYes.Click();
        }

        public void ClickYesWithoutWait()
        {
            base.btnYes.ClickNoWait();
        }

        public void AssertViewDisplayed()
        {
            base.ViewName.Equals("CreatePersonalLoanLead");
        }

        public void AssertYesNoButtonsDisplayed()
        {
            Assert.True((base.btnYes.Exists && base.btnNo.Exists), "Yes No buttons are not displayed");
        }

        public void AssertMessageDisplayed(string message)
        {
            Assert.IsTrue(base.divValidationSummaryBody.Text.Contains(message));
        }

        public void AssertMessageDisplayed()
        {
            Assert.IsTrue(base.divValidationSummaryBody.Text.Contains(String.Empty));
        }

        public void AssertNotificationDisplayed(string message)
        {
            var failMsg = "The \"{0}\" notification is not displayed.";
            Assert.IsTrue(base.Notification.Exists, failMsg, message);
            Assert.IsTrue(base.Notification.Text.Contains(message), failMsg, message);
        }

        public void AssertNotificationNotDisplayed(string message)
        {
            var failMsg = "The \"{0}\" notification is displayed.";
            Assert.IsFalse(base.Notification.Exists, failMsg, message);
            Assert.IsFalse(base.Notification.Text.Contains(message), failMsg, message);
        }
    }
}