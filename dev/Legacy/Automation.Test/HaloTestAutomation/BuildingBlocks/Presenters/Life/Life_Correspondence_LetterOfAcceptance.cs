using Common.Enums;
using ObjectMaps;
using ObjectMaps.Pages;

namespace BuildingBlocks.Presenters.Life
{
    public class Life_Correspondence_LetterOfAcceptance : LifeCorrespondenceLetterOfAcceptanceControls
    {
        /// <summary>
        /// Click the Send Correspondence button on the Life_Correspondence_LetterOfAcceptance view.
        /// </summary>
        /// <param name="b">TestBrowser instance that is being used</param>
        /// <param name="option">Correspondence Option</param>
        public void SendPolicyDocument(CorrespondenceMediumEnum option, string emailAddress, string faxCode, string faxNumber)
        {
            if (option == CorrespondenceMediumEnum.Email)
            {
                base.ctl00MainchkEmail.Checked = true;
                if (base.ctl00MaintxtEmail.Text == null)
                    base.ctl00MaintxtEmail.TypeText(emailAddress);
            }
            if (option == CorrespondenceMediumEnum.Fax)
            {
                base.ctl00MainchkFax.Checked = true;
                if (base.ctl00MaintxtFaxCode.Text == null)
                    base.ctl00MaintxtFaxCode.TypeText(faxCode);
                if (base.ctl00MaintxtFax.Text == null)
                    base.ctl00MaintxtFax.TypeText(faxNumber);
            }
            base.ctl00MainchkPost.Checked = option == CorrespondenceMediumEnum.Post ? true : false;
            CommonIEDialogHandler.SelectOK(base.ctl00MainbtnSend);
            base.Document.DomContainer.WaitForComplete();
        }
    }
}