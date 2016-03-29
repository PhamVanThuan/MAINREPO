using ObjectMaps.NavigationControls;
using WatiN.Core;

namespace ObjectMaps.FloBO
{
    /// <summary>
    ///
    /// </summary>
    public abstract class ApplicationOverviewNodeControls : BaseNavigation
    {
        protected Link ApplicationOverview(int offerKey, string offerTypeDescription)
        {
            DivCollection divs = base.Document.Divs.Filter(Find.ById("ctl00_tabsMenu_pnlTasks_ctl01_Tasks"));
            foreach (Div d in divs)
            {
                foreach (Element e in d.Elements)
                {
                    if (e is Link)
                    {
                        string desc = offerTypeDescription + " : " + offerKey;
                        if (e.Text.Contains(desc))
                            return e as Link;
                    }
                }
            }
            return null;
        }

        [FindBy(Text = "Loan Details")]
        protected Link LoanDetails { get; set; }

        [FindBy(Text = "Property Summary")]
        protected Link PropertySummary { get; set; }

        [FindBy(Text = "Valuations")]
        protected Link Valuations { get; set; }

        [FindBy(Text = "Application Memo")]
        protected Link ApplicationMemo { get; set; }

        [FindBy(Text = "Home Owners Cover")]
        protected Link HomeOwnersCover { get; set; }

        [FindBy(Text = "Loan Conditions")]
        protected Link LoanConditions { get; set; }

        [FindBy(Text = "Applicant Summary")]
        protected Link ApplicantSummary { get; set; }

        [FindBy(Text = "ITC Summary")]
        protected Link ITCSummary { get; set; }

        [FindBy(Text = "Employment")]
        protected Link Employment { get; set; }

        [FindBy(Text = "Affordability")]
        protected Link Affordability { get; set; }

        [FindBy(Text = "Assets and Liabilities")]
        protected Link AssetsAndLiabilities { get; set; }

        [FindBy(Text = "Group Exposure")]
        protected Link GroupExposure { get; set; }

        [FindBy(Text = "Application Warnings")]
        protected Link ApplicationWarnings { get; set; }

        [FindBy(Text = "Loan Attributes")]
        protected Link LoanAttributes { get; set; }

        [FindBy(Text = "Revisions")]
        protected Link Revisions { get; set; }

        [FindBy(Text = "Decline Reasons")]
        protected Link DeclineReasons { get; set; }

        [FindBy(Text = "Application Reasons")]
        protected Link ApplicationReasons { get; set; }
    }
}