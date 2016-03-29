using WatiN.Core;

namespace ObjectMaps.Pages
{
    public class SAHLWebsiteMainControls : BasePageControls
    {
        private Browser b;

        public SAHLWebsiteMainControls(Browser browser)
        {
            b = browser;
        }

        public Link Calculators
        {
            get
            {
                return b.Link(Find.ByText("CALCULATORS"));
            }
        }
    }
}