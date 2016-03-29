using ObjectMaps.NavigationControls;
using WatiN.Core;

namespace ObjectMaps.FloboControls
{
    public abstract class CorrespondenceNodeControls : BaseNavigation
    {
        [FindBy(Text = "Correspondence")]
        protected Link Correspondence { get; set; }

        [FindBy(Text = "Correspondence Summary")]
        protected Link CorrespondenceSummary { get; set; }

        [FindBy(Text = "ITC Dispute Indicated (English)")]
        protected Link ITCDisputeIndicatedEng { get; set; }

        [FindBy(Text = "ITC Dispute Indicated (Afrikaans)")]
        protected Link ITCDisputeIndicatedAfr { get; set; }

        [FindBy(Text = "Correspondence History")]
        protected Link CorrespondenceHistory { get; set; }

        [FindBy(Text = "Send Email")]
        protected Link SendEmail { get; set; }

        [FindBy(Text = "Send SMS")]
        protected Link SendSMS { get; set; }
    }
}