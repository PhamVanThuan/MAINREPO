using System;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;

//using SAHL.Common.DomainMessages;
using SAHL.Common.CacheData;
using SAHL.Common.Globals;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;

namespace SAHL.Web.Views.Common.Presenters.ValuationDetails
{

    public class ValuationADCheckDirectPresenter : ValuationADCheckPresenter
    {

        private InstanceNode instanceNode;
        private CBOMenuNode node;
        private IProperty property;
        private int requiredKey;
        private IApplicationMortgageLoan applicationMortgageLoan;

        public ValuationADCheckDirectPresenter(IValuationAdCheckView view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }


        protected override void OnViewInitialised(object sender, EventArgs e)
        {

            base.OnViewInitialised(sender, e);
            if (!View.ShouldRunPage) return;
            // get the latest pending adcheck valuation and display the results

            node = CBOManager.GetCurrentCBONode(View.CurrentPrincipal) as CBOMenuNode;

            if (GlobalCacheData.ContainsKey("ValuationKey"))
                valuationkey = Convert.ToInt32(GlobalCacheData["ValuationKey"]);
            else
            {
                // There is no valuationkey - get it...
                // Get the AccountKey/OfferKey from the CBO
                if (node is InstanceNode)
                {
                    instanceNode = node as InstanceNode;
                    requiredKey = Convert.ToInt32(instanceNode.GenericKey);
                }
                else if (node != null) requiredKey = Convert.ToInt32(node.GenericKey);

                if (node != null)
                    switch (node.GenericKeyTypeKey)
                    {
                        case (int)GenericKeyTypes.Account:
                            IMortgageLoanAccount account = (IMortgageLoanAccount)AccountRepository.GetAccountByKey(requiredKey);
                            property = account.SecuredMortgageLoan.Property;
                            break;
                        case (int)GenericKeyTypes.Property:
                            property = PropertyRepository.GetPropertyByKey(requiredKey);
                            break;
                        case (int)GenericKeyTypes.Offer:
                            applicationMortgageLoan = ApplicationRepository.GetApplicationByKey(requiredKey) as IApplicationMortgageLoan;
                            if (applicationMortgageLoan != null) property = applicationMortgageLoan.Property;
                            break;
                        default:
                            break;
                    }

                // If the valuation status is pending for adcheck
                foreach (IValuation val in property.Valuations)
                {
                    if ((val.ValuationStatus.Key == 1) && (val.ValuationDataProviderDataService.DataProviderDataService.DataProvider.Key == (int)DataProviders.AdCheck))
                    {
                        GlobalCacheData.Remove("ValuationKey");
                        GlobalCacheData.Add("ValuationKey", val.Key, new List<ICacheObjectLifeTime>());
                        break;
                    }
                }

                //if (dsValuations.Rows.Count == 0)
                //    View.Messages.Add(new Warning("No pending Adcheck valuation is available.", "No pending Adcheck valuation is available."));
            }

        }

    }
}


