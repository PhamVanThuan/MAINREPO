using ObjectMaps.NavigationControls;
using WatiN.Core;

namespace ObjectMaps.FLoBO
{
    public abstract class ApplicantsNodeControls : BaseNavigation
    {
        [FindBy(Text = "Applicants")]
        protected Link Applicants { get; set; }

        [FindBy(Text = "Add Legal Entity")]
        protected Link AddLegalEntity { get; set; }

        [FindBy(Text = "Remove Legal Entity")]
        protected Link RemoveLegalEntity { get; set; }

        [FindBy(Text = "ITC Details")]
        protected Link ITCDetails { get; set; }
    }
}