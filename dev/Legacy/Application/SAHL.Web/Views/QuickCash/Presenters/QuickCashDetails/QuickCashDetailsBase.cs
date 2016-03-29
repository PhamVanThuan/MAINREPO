using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.QuickCash.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI.Events;


namespace SAHL.Web.Views.QuickCash.Presenters.QuickCashDetails
{
    public class QuickCashDetailsBase : SAHLCommonBasePresenter<IQuickCashDetails>
    {
        protected IApplicationRepository appRepo;
        protected ISupportsQuickCashApplicationInformation quickCashApplicationInformation;
        protected IQuickCashRepository qcRepo;
        protected ILookupRepository lookups;
        protected IApplicationInformationQuickCash appInfoQuickCash;
        protected IApplication application;
        protected int selectedPaymentType;
        IControlRepository controlRepo;
        protected int _offerKey;

        public QuickCashDetailsBase(IQuickCashDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!View.ShouldRunPage) return;

            CBOMenuNode _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            _offerKey = _node.GenericKey; //315267; 

            appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            controlRepo = RepositoryFactory.GetRepository<IControlRepository>();

            application = appRepo.GetApplicationByKey(_offerKey);
            quickCashApplicationInformation = application as ISupportsQuickCashApplicationInformation;
            appInfoQuickCash = quickCashApplicationInformation.ApplicationInformationQuickCash;

            qcRepo = RepositoryFactory.GetRepository<IQuickCashRepository>();
            lookups = RepositoryFactory.GetRepository<ILookupRepository>();

            _view.onPaymentTypeSelectedIndexChanged += new KeyChangedEventHandler(_view_onPaymentTypeSelectedIndexChanged);
        }


        protected void _view_onPaymentTypeSelectedIndexChanged(object sender, KeyChangedEventArgs e)
        {
            if (e.Key.ToString() != "-select-")
                selectedPaymentType = Convert.ToInt32(e.Key);
        }

        protected IMargin GetMarginForLinkRate(string marginVal, IMargin margin)
        {
            for (int i = 0; i < lookups.Margins.Count; i++)
            {
                if (lookups.Margins[i].Value.ToString() == marginVal)
                {
                    margin = lookups.Margins[i];
                    break;
                }
            }
            return margin;
        }

        protected IRateConfiguration GetControlNumericForLinkRate(string controlDescription)
        {
            // Screen has been designed to display multiple link rates, but at this point, there is only one , when there are more 
            // then storing these vals on the Control Table will become inefficient - will have to be redesigned using the credit matrix
            ICreditMatrixRepository crRepo = RepositoryFactory.GetRepository<ICreditMatrixRepository>();
            IControl control = controlRepo.GetControlByDescription(controlDescription);
            IMarketRate marketRate = lookups.MarketRates.ObjectDictionary[((int)MarketRates.PrimeLendingRate).ToString()];
            string marginVal = ((control.ControlNumeric) / 100).ToString();
            IMargin margin = null;
            margin = GetMarginForLinkRate(marginVal, margin);
            IRateConfiguration rateConfig = crRepo.GetRateConfigurationByMarginKeyAndMarketRateKey(margin.Key, marketRate.Key);

            return rateConfig;
        }

    }
}
