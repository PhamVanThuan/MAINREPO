using BuildingBlocks.Services.Contracts;
using Common.Enums;
using NUnit.Framework;
using ObjectMaps.Pages;
using System.Collections.Generic;
using System.Threading;
using WatiN.Core.Logging;

namespace BuildingBlocks.Presenters.DebtCounselling
{
    public class CorrespondenceProcessingMultipleWorkflowDebtCounsellor : CorrespondenceProcessingMultipleWorkflowDebtCounsellorControls
    {
        private readonly IWatiNService watinService;

        public CorrespondenceProcessingMultipleWorkflowDebtCounsellor()
        {
            watinService = ServiceLocator.Instance.GetService<IWatiNService>();
        }

        public void SelectCorrespondenceRecipient(int legalEntityKey, bool faxCheckbox, string faxCode, string faxNumber, bool emailCheckbox, string emailAddress, bool postCheckbox, ButtonTypeEnum buttonName)
        {
            bool atLeastOneSet = false;
            //fax option
            if (faxCheckbox && base.chkFax(legalEntityKey).Enabled)
            {
                base.chkFax(legalEntityKey).Checked = faxCheckbox;
                if (!string.IsNullOrEmpty(faxCode))
                    base.txtFaxCODE(legalEntityKey).TypeText(faxCode);
                if (!string.IsNullOrEmpty(faxNumber))
                    base.txtFaxNUMB(legalEntityKey).TypeText(faxNumber);
                atLeastOneSet = true;
            }
            //email option
            if (emailCheckbox && base.chkEmail(legalEntityKey).Enabled)
            {
                base.chkEmail(legalEntityKey).Checked = emailCheckbox;
                if (!string.IsNullOrEmpty(emailAddress))
                {
                    base.txtEmail(legalEntityKey).Clear();
                    base.txtEmail(legalEntityKey).Value = emailAddress;
                }
                atLeastOneSet = true;
            }
            //post option
            if (postCheckbox && base.chkPost(legalEntityKey).Enabled)
            {
                base.chkPost(legalEntityKey).Checked = postCheckbox;
                atLeastOneSet = true;
            }
            //check that one has been set
            if (!atLeastOneSet && base.chkPost(legalEntityKey).Enabled)
            {
                Logger.LogAction("The correspondence options for Fax and Email were not available, post was selected as a default.");
                base.chkPost(legalEntityKey).Checked = true;
            }
            ClickButton(buttonName);
        }

        public void SelectCorrespondenceRecipient(int legalEntityKey)
        {
            SelectCorrespondenceRecipient(legalEntityKey, false, null, null, false, null, true, ButtonTypeEnum.SendCorrespondence);
        }

        public void ClickButton(ButtonTypeEnum buttonName)
        {
            switch (buttonName)
            {
                case ButtonTypeEnum.Finish:
                    break;

                case ButtonTypeEnum.Cancel:
                    base.ctl00MainbtnCancel.Click();
                    break;

                case ButtonTypeEnum.Preview:
                    base.ctl00MainbtnPreview.Click();
                    break;

                case ButtonTypeEnum.SendCorrespondence:
                    watinService.HandleConfirmationPopup(base.ctl00MainbtnSend);
                    Thread.Sleep(2500);
                    if (base.divValidationSummaryBody.Exists)
                    {
                        if (base.divContinueButton.Exists)
                        {
                            watinService.HandleConfirmationPopup(base.divContinueButton);
                        }
                    }
                    break;

                default:
                    break;
            }
        }

        public void AssertCorrespondenceRecipientExists(int legalEntityKey)
        {
            Assert.True(base.chkPost(legalEntityKey).Exists || base.chkFax(legalEntityKey).Exists || base.chkEmail(legalEntityKey).Exists, "Correspondence Recipient doesn't exist.");
        }

        public void SelectMultipleCorrespondenceRecipientsForPost(List<Automation.DataModels.ExternalRole> roles)
        {
            foreach (var item in roles)
                SelectCorrespondenceRecipient(item.LegalEntityKey, false, null, null, false, null, true, ButtonTypeEnum.None);
        }

        public void SelectMultipleCorrespondenceRecipientsForEmail(List<Automation.DataModels.ExternalRole> roles, string email)
        {
            foreach (var item in roles)
                SelectCorrespondenceRecipient(item.LegalEntityKey, false, null, null, true, email, false, ButtonTypeEnum.None);
        }

        public void ClickSendCorrespondence()
        {
            ClickButton(ButtonTypeEnum.SendCorrespondence);
        }

        public void SelectMultipleRecipientsForPostAndEmailCorrespondence(List<Automation.DataModels.ExternalRole> roles)
        {
            foreach (var item in roles)
            {
                SelectCorrespondenceRecipient(item.LegalEntityKey, false, null, null, true, "marchuanv@sahomeloans.com", true, ButtonTypeEnum.None);
            }
            ClickButton(ButtonTypeEnum.SendCorrespondence);
        }
    }
}