using Common.Constants;
using ObjectMaps.Pages;
using System.Text.RegularExpressions;
using System.Threading;

namespace BuildingBlocks.Presenters.Origination
{
    public class ApplicationMailingAddressUpdate : ApplicationMailingAddressUpdateControls
    {
        /// <summary>
        /// Selects the first option in the 'Email Address' and 'Address' dropdowns.  OnlineStatementFormat is nullable
        /// </summary>
        /// <param name="formattedAddress"></param>
        /// <param name="correspondenceMedium"></param>
        /// <param name="correspondenceLanguage"></param>
        /// <param name="onlineStatement"></param>
        /// <param name="onlineStatementformat"></param>
        /// <param name="legalentityemailaddress"></param>
        public void UpdateApplicationMailingAddress
            (
                string formattedAddress,
                string correspondenceMedium,
                string correspondenceLanguage,
                bool onlineStatement,
                string onlineStatementformat,
                string legalentityemailaddress = null
            )
        {
            Thread.Sleep(1000);
            base.ddlMailingAddress.Option(formattedAddress).Select();
            Thread.Sleep(1000);
            base.ddlCorrespondenceMedium.Option(correspondenceMedium).Select();
            //we need to wait for a bit here
            Thread.Sleep(1000);
            if (correspondenceMedium == CorrespondenceMedium.Email)
            {
                if (string.IsNullOrEmpty(legalentityemailaddress))
                    throw new System.Exception("CorrespondenceMedium of 'Email' was passed as a parameter, but no emailaddress provided.");
                base.ddlCorrespondenceMailAddress.Option(new Regex(legalentityemailaddress)).Select();
            }
            base.Document.DomContainer.WaitForComplete();
            base.ddlCorrespondenceLanguage.Option(correspondenceLanguage).Select();
            if (!base.chkOnlineStatement.Checked && onlineStatement)
                base.chkOnlineStatement.Click();
            else if (base.chkOnlineStatement.Checked && !onlineStatement)
                base.chkOnlineStatement.Click();
            //we need to wait for a bit here
            Thread.Sleep(2000);
            if (base.chkOnlineStatement.Checked && onlineStatementformat != null)
                base.ddlOnlineStatementFormat.Option(onlineStatementformat).Select();
            base.btnSubmit.Click();
        }
    }
}