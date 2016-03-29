using ObjectMaps.NavigationControls;
using WatiN.Core;

namespace ObjectMaps.FloboControls
{
    public abstract class ApplicationMemoNodeControls : BaseNavigation
    {
        [FindBy(Text = "Application Memo")]
        protected Link ApplicationMemo { get; set; }

        [FindBy(Text = "Application Memo Summary")]
        protected Link ApplicationMemoSummary { get; set; }

        [FindBy(Text = "Add Application Memo")]
        protected Link AddApplicationMemo { get; set; }

        [FindBy(Text = "Update Application Memo")]
        protected Link UpdateApplicationMemo { get; set; }
    }
}