using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Configuration;
using System.Configuration;
using SAHL.Common.DomainMessages;
using SAHL.Common.X2.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using System.Data;
using SAHL.Common.Globals;
using SAHL.Common.CacheData;

namespace SAHL.Web.Views.Common.Presenters
{
    public class RedirectScheduledValuation : SAHLCommonBasePresenter<IRedirect>
    {
        public RedirectScheduledValuation(IRedirect view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage) 
                return;

            string source = "default";
            string errorMessage = null;
            SAHLRedirectionSection RedirectionSection = null;

            try
            {
                RedirectionSection = (SAHLRedirectionSection)ConfigurationManager.GetSection("RedirectionConfiguration");

                CBONode node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
                IX2Repository X2Repository = RepositoryFactory.GetRepository<IX2Repository>();
                IInstance instance = X2Repository.GetLatestInstanceForGenericKey(node.GenericKey, SAHL.Common.Constants.WorkFlowName.Valuations, SAHL.Common.Constants.WorkFlowProcessName.Origination);
                X2Data x2Data = X2Repository.GetX2DataForInstance(instance);

                if (x2Data == null || x2Data.Data == null || x2Data.Data.Rows.Count < 1)
                {
                    errorMessage = "X2Data not found";
                    throw new Exception();
                }

                DataRow x2Row = x2Data.Data.Rows[0];
                int valuationKey = -1;
                int valuationDataProviderDataServiceKey = -1;

                if (x2Data.Data.Columns.Contains("ValuationKey") && x2Row["ValuationKey"] != DBNull.Value)
                    valuationKey = Convert.ToInt32(x2Row["ValuationKey"]);

                if (valuationKey < 1)
                {
                    errorMessage = "No valuation exists for this case";
                    throw new Exception();
                }

                //we will assume it's an adcheck case, as older cases will not have ValuationDataProviderDataServiceKey stored in x2Data 
 
                if (x2Data.Data.Columns.Contains("ValuationDataProviderDataServiceKey") && x2Row["ValuationDataProviderDataServiceKey"] != DBNull.Value)
                    valuationDataProviderDataServiceKey = Convert.ToInt32(x2Row["ValuationDataProviderDataServiceKey"]);

                if (valuationDataProviderDataServiceKey > 0)
                {
                    //IPropertyRepository pRepo = RepositoryFactory.GetRepository<IPropertyRepository>();
                    //IValuationDataProviderDataService vdpds = pRepo.GetValuationDataProviderDataServiceByKey((ValuationDataProviderDataServices)valuationDataProviderDataServiceKey);
                    ValuationDataProviderDataServices vdpds = (ValuationDataProviderDataServices)valuationDataProviderDataServiceKey;
                    source = Enum.GetName(typeof(ValuationDataProviderDataServices), vdpds);
                }

                if (string.IsNullOrEmpty(source))
                    source = "default";
            }
            catch (Exception ex)
            {
                //navigate to an error page
                source = "Error";

                if (string.IsNullOrEmpty(errorMessage))
                    errorMessage = string.Format("An unexpected error occured:\n{0}", ex.Message);

                SimplePageCacheObjectLifeTime lifeTime = new SimplePageCacheObjectLifeTime(new List<string> { _view.ViewName, "Notification" });
                base.GlobalCacheData.Add("NotificationMessage", errorMessage, lifeTime);
            }

            RedirectionElement Redirect = RedirectionSection.GetRedirection(source, base._view.ViewName);

            if (Redirect == null)
                throw new Exception(string.Format("Invalid RedirectionSection entry: {0}", source));

            base._view.Navigator.Navigate(Redirect.NavigationView);
                      
        }
    }
}
