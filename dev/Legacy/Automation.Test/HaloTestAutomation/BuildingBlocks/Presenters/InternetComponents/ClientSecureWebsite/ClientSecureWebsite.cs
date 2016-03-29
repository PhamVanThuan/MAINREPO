using ObjectMaps.Pages;

namespace BuildingBlocks.Presenters.InternetComponents.ClientSecureWebsite
{
    public class ClientSecureWebsiteLogin : ClientSecureWebsiteLoginControls
    {
        public bool LogOff()
        {
            base.Logoff.Click();
            return false;
        }

        public bool Login(string emailAddress, string password)
        {
            base.txtEmailAddress.Value = emailAddress;
            base.txtPassword.Value = password;
            base.btnLogin.Click();
            return true;
        }

        public void ResetPassword(string emailAddress)
        {
            base.ResetPassword.Click();
            base.txtEmailAddress.Value = emailAddress;
            base.btnSend.Click();
        }

        public void ClickLogin()
        {
            base.btnLogin.Click();
        }

        public void ClickCancel()
        {
            base.btnCancel.Click();
        }
    }
}