using ObjectMaps.Pages;

namespace BuildingBlocks.Presenters.Origination
{
    public class ApplicationSummaryBase : ApplicationSummaryBaseControls
    {
        public int GetOfferKey()
        {
            int offerKey = 0;
            base.spanApplicationNumber.WaitUntilExists();
            offerKey = int.Parse(base.spanApplicationNumber.Text);
            return offerKey;
        }
    }
}