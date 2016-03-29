using ObjectMaps.NavigationControls;
using WatiN.Core;

namespace ObjectMaps.FloBO
{
    public abstract class PersonalLoanNodeControls : BaseNavigation
    {
        [FindBy(Title = "Close Personal Loan Account")]
        protected Link ClosePersonalLoanAccount { get; set; }

        [FindBy(Title = "Credit Life Policy Instatement Letter")]
        protected Link CreditLifePolicyInstatementLetter  { get; set; }

        [FindBy(Title = "Create Credit Life Policy")]
        protected Link CreateCreditLifePolicy { get; set; }

        [FindBy(Title = "Term Extension Letter")]
        protected Link TermExtensionLetter { get; set; }

        [FindBy(Title = "Correspondence")]
        protected Link Correspondence { get; set; }

        [FindBy(Title = "Personal Loan")]
        protected Link PersonalLoan { get; set; }

        [FindBy(Title = "Import Leads")]
        protected Link ImportLeads { get; set; }
    }
}