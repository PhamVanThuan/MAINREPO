using BuildingBlocks.Services.Contracts;
using Common.Enums;
using ObjectMaps.FloBO;

namespace BuildingBlocks.Navigation.FLOBO.Origination
{
    public class SellerLENode : SellersNodeControls
    {
        private readonly IApplicationService applicationService;

        public SellerLENode()
        {
            applicationService = ServiceLocator.Instance.GetService<IApplicationService>();
        }

        public void SellerName(int OfferKey)
        {
            //retrieve the name
            var results = applicationService.GetSellerLegalName(OfferKey);
            string LegalEntityName = results.Rows(0).Column(0).Value;
            //dispose
            results.Dispose();
            //now we can click the node
            base.SellerName(LegalEntityName).Click();
        }

        public void SellersDetails(NodeTypeEnum n)
        {
            base.SellerDetails.Click();
            switch (n)
            {
                case NodeTypeEnum.View:
                    break;

                case NodeTypeEnum.Update:
                    base.UpdateSellerDetails.Click();
                    break;
            }
        }

        public void Sellers(NodeTypeEnum n)
        {
            base.Sellers.Click();
            switch (n)
            {
                case NodeTypeEnum.View:
                    base.SellerSummary.Click();
                    break;

                case NodeTypeEnum.Add:
                    base.AddSeller.Click();
                    break;

                case NodeTypeEnum.Delete:
                    base.RemoveSeller.Click();
                    break;
            }
        }
    }
}