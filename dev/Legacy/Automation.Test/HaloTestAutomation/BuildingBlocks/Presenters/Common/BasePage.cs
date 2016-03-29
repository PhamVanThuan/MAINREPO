using BuildingBlocks.Services.Contracts;
using NUnit.Framework;
using ObjectMaps.Pages;
using System.Collections.Generic;
using WatiN.Core;
using WatiN.Core.Logging;

namespace BuildingBlocks.Presenters.CommonPresenters
{
    public class BasePage : BasePageControls
    {
        private readonly IWatiNService watinService;

        public BasePage()
        {
            watinService = ServiceLocator.Instance.GetService<IWatiNService>();
        }

        /// <summary>
        /// Checks for any Halo validation/error messages and uses the WatiN.Core.Logging.Logger.LogAction
        /// method to output any error text.  Add to your TearDown method.
        /// </summary>
        public bool CheckForErrorMessages()
        {
            if (base.divValidationSummaryBody.Exists)
            {
                string warning = "WARNING: The following Halo validation messages were found on TearDown";
                foreach (Element message in base.listErrorMessages)
                {
                    warning = warning + "\r\n" + message.Text;
                }
                Logger.LogAction("{0}", warning);
                return false;
            }
            if (base.bodyErrorPage.Exists)
            {
                string stackTrace = base.StackTrace.Exists ? base.StackTrace.Text : string.Empty;
                string errorMessage = base.ErrorLabel.Exists ? base.ErrorLabel.Text : string.Empty;
                Logger.LogAction(string.Format(@"WARNING: {0}-{1}", errorMessage, stackTrace));
                base.HomePageButton.Click();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks for a specific error message
        /// </summary>
        /// <param name="expectedErrorMessage"></param>
        /// <returns></returns>
        public bool CheckForerrorMessages(string expectedErrorMessage)
        {
            bool result = false;

            if (base.divValidationSummaryBody.Exists)
            {
                foreach (Element message in base.listErrorMessages)
                {
                    if (message.Text == expectedErrorMessage)
                    {
                        result = true;
                        message.Flash(2);
                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Get all the validation message on this screen
        /// </summary>
        /// <returns></returns>
        public List<string> GetValidationMessages()
        {
            List<string> messages = new List<string>();
            foreach (Element ele in base.divValidationSummaryBody.ElementsWithTag("li"))
                messages.Add(ele.Text);
            return messages;
        }

        public bool ErrorLabelExists()
        {
            return base.ErrorLabel.Exists;
        }

        public void PleaseStartAgain()
        {
            base.HomePageButton.Click();
        }

        /// <summary>
        /// Checks if the domain validation warning appears on the screen and then clicks the button to continue if it does
        /// </summary>
        public void DomainWarningClickYes()
        {
            if (base.divValidationSummaryBody.Exists)
            {
                if (base.divValidationSummaryBody.Buttons.Count > 0)
                {
                    //Click on the Button in the Validation Summary
                    base.divValidationSummaryBody.Buttons[0].Click();
                }
            }
        }

        /// <summary>
        /// Checks if the domain validation warning appears on the screen and then clicks the button to continue if it does
        /// </summary>
        public void DomainWarningClickYesHandlingPopUp()
        {
            if (base.divValidationSummaryBody.Exists)
            {
                if (base.divValidationSummaryBody.Buttons.Count > 0)
                {
                    //Click on the Button in the Validation Summary
                    watinService.HandleConfirmationPopup(base.divValidationSummaryBody.Buttons[0]);
                }
            }
        }

        public bool LoggedInAs(string userName)
        {
            return base.User.Text == userName ? true : false;
        }
    }
}