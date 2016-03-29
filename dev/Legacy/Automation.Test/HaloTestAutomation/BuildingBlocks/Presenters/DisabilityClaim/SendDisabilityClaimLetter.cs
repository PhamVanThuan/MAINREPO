using BuildingBlocks.Services.Contracts;
using Common.Enums;
using ObjectMaps;
using ObjectMaps.Pages;

namespace BuildingBlocks.Presenters.DisabilityClaim
{
    public class SendDisabilityClaimLetter : SendDisabilityClaimLetterControls
    {
        public void SelectCorrespondenceOption(CorrespondenceMediumEnum correspondenceMedium)
        {
            switch (correspondenceMedium)
            {
                case CorrespondenceMediumEnum.Email:
                    base.EmailCheckBox.Click();
                    break;
                case CorrespondenceMediumEnum.Fax:
                    base.FaxCode.Click();
                    break;
                case CorrespondenceMediumEnum.Post:
                    base.PostCheckBox.Click();
                    break;
                default:
                    break;
            }
        }

        public void SendCorrespondence(CorrespondenceMediumEnum correspondenceMedium, int legalEntityKey)
        {
            switch (correspondenceMedium)
            {
                case CorrespondenceMediumEnum.Email:
                    if (base.EmailAddress.Value == null)
                    {
                        ServiceLocator.Instance.GetService<ILegalEntityService>().UpdateEmailAddress(legalEntityKey, "test@test.com");
                        base.EmailAddress.Value = "test@test.com";
                    }
                    CommonIEDialogHandler.SelectOK(base.SendCorrespondence);
                    base.Document.DomContainer.WaitForComplete();
                    break;
                default:
                    CommonIEDialogHandler.SelectOK(base.SendCorrespondence);
                    base.Document.DomContainer.WaitForComplete();
                    break;
            }
        }

        public void CancelSendingCorrespondenceBeforeSubmission()
        {
            base.CancelButton.Click();
        }
    }
}