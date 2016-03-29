using BuildingBlocks.Services.Contracts;
using Common.Enums;
using NUnit.Framework;
using ObjectMaps.Pages;
using System;
using System.Linq;

namespace BuildingBlocks.Presenters.CommonPresenters
{
    public class CorrespondenceProcessing : CorrespondenceProcessingControls
    {
        private readonly IWatiNService watinService;

        public CorrespondenceProcessing()
        {
            watinService = ServiceLocator.Instance.GetService<IWatiNService>();
        }

        /// <summary>
        /// This method will send the correspondence via the correspondence processing screen. If the correspondence has
        /// been previously sent then the warning pop up will be handled.
        /// </summary>
        /// <param name="b">The IE TestBrowser Object</param>
        /// <param name="method">The Correspondence Medium</param>
        public void SendCorrespondence(CorrespondenceSendMethodEnum method)
        {
            switch (method)
            {
                case CorrespondenceSendMethodEnum.Email:
                    if (!String.IsNullOrEmpty(base.EmailAddress.Value)
                            && base.EmailAddress.Value.Length > 0)
                    {
                        if (!base.chkEmail.Checked)
                            base.chkEmail.Click();
                    }
                    else
                    {
                        base.EmailAddress.Value = "clintons@sahomeloans.com";

                        if (!base.chkEmail.Checked)
                            base.chkEmail.Click();
                    }
                    break;

                case CorrespondenceSendMethodEnum.Fax:
                    if (!String.IsNullOrEmpty(base.FaxCode.Value)
                            && !String.IsNullOrEmpty(base.FaxNumber.Value)
                            && base.FaxCode.Value.Length > 0
                            && base.FaxNumber.Value.Length > 0)
                    {
                        if (!base.chkFax.Checked)
                            base.chkFax.Click();
                    }
                    else
                    {
                        base.FaxCode.TypeText("001");
                        base.FaxNumber.TypeText("1234567");

                        if (!base.chkFax.Checked)
                            base.chkFax.Click();
                    }
                    break;

                case CorrespondenceSendMethodEnum.Post:
                    if (!base.chkPost.Checked)
                        base.chkPost.Click();
                    break;

                case CorrespondenceSendMethodEnum.SMS:
                    if (!base.chkSMS.Checked)
                        base.chkSMS.Click();
                    break;
            }
            SendCorrespondence();
        }

        /// <summary>
        /// This method will send the correspondence via the correspondence processing screen. If the correspondence has
        /// been previously sent then the warning pop up will be handled.
        /// </summary>
        /// <param name="b">The IE TestBrowser Object</param>
        /// <param name="method">The Correspondence Medium</param>
        public void SendCorrespondence()
        {
            watinService.HandleConfirmationPopup(base.SendButton);
            if (base.divValidationSummaryBody.Exists)
            {
                if (base.divValidationSummaryBody.Buttons.Count > 0)
                {
                    watinService.HandleConfirmationPopup(base.divValidationSummaryBody.Buttons[0]);
                }
            }
        }

        /// <summary>
        /// This method will send the correspondence via the correspondence processing screen. If the correspondence has
        /// been previously sent then the warning pop up will be handled.
        /// </summary>
        /// <param name="b">The IE TestBrowser Object</param>
        /// <param name="method">The Correspondence Medium</param>
        public void Submit(CorrespondenceSendMethodEnum method)
        {
            switch (method)
            {
                case CorrespondenceSendMethodEnum.Email:
                    if (base.EmailAddress.Value.Length > 0)
                    {
                        if (!base.chkEmail.Checked)
                            base.chkEmail.Click();
                    }
                    else
                    {
                        base.EmailAddress.Value = "clintons@sahomeloans.com";

                        if (!base.chkEmail.Checked)
                            base.chkEmail.Click();
                    }
                    break;

                case CorrespondenceSendMethodEnum.Fax:
                    if (base.FaxCode.Value.Length > 0 && base.FaxNumber.Value.Length > 0)
                    {
                        if (!base.chkFax.Checked)
                            base.chkFax.Click();
                    }
                    else
                    {
                        base.FaxCode.Value = "001";
                        base.FaxNumber.Value = "1234567";

                        if (!base.chkFax.Checked)
                            base.chkFax.Click();
                    }
                    break;

                case CorrespondenceSendMethodEnum.Post:
                    if (!base.chkPost.Checked)
                        base.chkPost.Click();
                    break;

                case CorrespondenceSendMethodEnum.SMS:
                    if (!base.chkSMS.Checked)
                        base.chkSMS.Click();
                    break;
            }
            base.SendButton.Click();
        }

        /// <summary>
        /// This will assert that all the base/common controls are present on a correspondence screen.
        /// </summary>
        public void AssertControlsValid()
        {
            var message = "";
            if (!base.chkFax.Exists)
                message += "Could not locate Fax CheckBox control";
            if (!base.chkPost.Exists)
                message += "Could not locate Post CheckBox control";
            if (!base.chkSMS.Exists)
                message += "Could not locate SMS CheckBox control";
            if (!base.FaxCode.Exists)
                message += "Could not locate FaxCode Input control";
            if (!base.FaxNumber.Exists)
                message += "Could not locate FaxNumber Input control";
            if (!base.EmailAddress.Exists)
                message += "Could not locate Email Address control";
            NUnit.Framework.Assert.AreEqual("", message, message);
        }

        public void AssertAllValidationMessagesExist()
        {
            Assert.True(base.divValidationSummaryBody.Text.Contains("Must select at least one Correspondence Option"));
        }

        public void AssertPostAvailable()
        {
            Assert.True(base.chkPost.Exists, "Post method is not available.");
        }

        public void AssertFaxAvailable()
        {
            Assert.True(base.chkFax.Exists, "Fax method is not available.");
        }

        public void AssertEmailAvailable()
        {
            Assert.True(base.chkEmail.Exists, "Email method is not available.");
        }

        public void AssertViewDisplayed(string viewName)
        {
            Assert.True(base.ViewName.Text.Equals(viewName), "The correspondence view was not displayed.");
        }

        public void CheckEmailCheckbox()
        {
            base.chkEmail.Checked = true;
        }

        public void AssertLegalEntityExistsOnCorrespondence(int legalEntityKey)
        {
            var leKey = (from cell in base.Recipients.TableCells select cell.Text).FirstOrDefault();
            Assert.AreEqual(legalEntityKey.ToString(), leKey, "Legal Entity is not displayed on view.");
        }
    }
}