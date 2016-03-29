using ObjectMaps.NavigationControls;
using WatiN.Core;

namespace ObjectMaps
{
    public partial class CboControls
    {
        public abstract class Menu : BaseNavigation
        {
            [FindBy(Id = "__tab_ctl00_tabsMenu_pnlMenu")]
            protected Span tabMenu { get; set; }

            protected ImageCollection imageCollectionTreeImage
            {
                get
                {
                    return base.Document.Images.Filter(Find.By("onmouseover", "this.src='../../Images/delete.png'"));
                }
            }

            [FindBy(Title = "Legal Entity")]
            protected Link linkLegalEntity { get; set; }

            [FindBy(Title = "Administration")]
            protected Link linkAdministration { get; set; }

            [FindBy(Title = "Reports")]
            protected Link linkReports { get; set; }

            [FindBy(Title = "Calculators")]
            protected Link linkCalculators { get; set; }

            [FindBy(Title = "Create CAP 2 Offer")]
            protected Link linkCreateCAP2Offer { get; set; }

            [FindBy(Title = "Create Life Leads")]
            protected Link CreateLifeLeads { get; set; }

            [FindBy(Title = "Loss Control")]
            protected Link linkLossControl { get; set; }

            [FindBy(Text = "Debt Counselling")]
            protected Link linkDebtCounselling { get; set; }

            [FindBy(Text = "Create Case")]
            protected Link linkCreateCase { get; set; }

            [FindBy(Text = "Debt Counsellor Maintenance")]
            protected Link linkDebtCounsellorMaintenance { get; set; }

            [FindBy(Text = "PDA Maintenance")]
            protected Link linkPDAMaintenance { get; set; }

            [FindBy(Text = "Personal Loan")]
            protected Link linkPersonalLoan { get; set; }

            [FindBy(Text = "Disability Claim")]
            protected Link linkDisabilityClaims { get; set; }
        }
    }
}