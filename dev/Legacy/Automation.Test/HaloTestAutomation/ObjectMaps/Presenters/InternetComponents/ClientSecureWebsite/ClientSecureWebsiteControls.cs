using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class ClientSecureWebsiteLoginControls : BasePageControls
    {
        [FindBy(Id = "UserName")]
        protected TextField txtEmailAddress { get; set; }

        [FindBy(Id = "Password")]
        protected TextField txtPassword { get; set; }

        [FindBy(Value = "Log in")]
        protected Button btnLogin { get; set; }

        [FindBy(Text = "reset password")]
        protected Link ResetPassword { get; set; }

        [FindBy(Value = "Send")]
        protected Button btnSend { get; set; }

        [FindBy(Value = "Cancel")]
        protected Button btnCancel { get; set; }

        [FindBy(Id = "AccountKey")]
        protected SelectList AccountNumber { get; set; }

        [FindBy(Id = "ReportFormatKey")]
        protected SelectList Type { get; set; }

        [FindBy(Value = "View Report")]
        protected Button ViewReport { get; set; }

        //Loan Statement
        [FindBy(Text = "Loan Statement")]
        protected Link LoanStatement { get; set; }

        [FindBy(Id = "FromDate")]
        protected TextField FromDate { get; set; }

        [FindBy(Id = "ToDate")]
        protected TextField ToDate { get; set; }

        //Tax Certificate
        [FindBy(Text = "Tax Certificate")]
        protected Link TaxCertificate { get; set; }

        [FindBy(Id = "Year")]
        protected TextField TaxYear { get; set; }

        //Change Password
        [FindBy(Text = "Change Password")]
        protected Link ChangePassword { get; set; }

        [FindBy(Id = "NewPassword")]
        protected TextField NewPassword { get; set; }

        [FindBy(Id = "ConfirmPassword")]
        protected TextField ConfirmPassword { get; set; }

        [FindBy(Value = "Change")]
        protected Button Change { get; set; }

        //Request Additional Funds
        [FindBy(Text = "Request Additional Funds")]
        protected Link RequestAdditionalFunds { get; set; }

        [FindBy(Id = "Amount")]
        protected TextField Amount { get; set; }

        [FindBy(Value = "Create")]
        protected Button Create { get; set; }

        //Logoff
        [FindBy(Text = "Logoff")]
        protected Link Logoff { get; set; }
    }
}