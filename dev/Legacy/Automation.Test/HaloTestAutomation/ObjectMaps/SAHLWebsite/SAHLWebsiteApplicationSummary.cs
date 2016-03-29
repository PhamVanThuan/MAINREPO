using WatiN.Core;

namespace ObjectMaps.Pages
{
    public class SAHLWebsiteApplicationSummaryControls : BasePageControls
    {
        private Browser b;

        public SAHLWebsiteApplicationSummaryControls(Browser browser)
        {
            b = browser;
        }

        public Span spReferenceNumber
        {
            get
            {
                return b.Span(Find.ById("dnn_ctr838_XSSmartModule_ctl00_lblSummaryReferenceNumber"));
            }
        }
    }
}