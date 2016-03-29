using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.CacheData;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;

namespace SAHL.Web.Views.Common.Presenters
{
    public class HOCFSSummaryApplication : HOCFSSummaryBase
    {
        int _applicationKey;
        CBOMenuNode _node;
        IApplicationMortgageLoan _applicationMortgageLoan;
        IList<ICacheObjectLifeTime> LifeTimes;
        IHOCRepository hocRepository;

        public HOCFSSummaryApplication(IHOCFSSummary view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            {
                _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
                if (_node != null)
                    _applicationKey = _node.ParentNode.GenericKey;
            }
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!View.ShouldRunPage) return;

            hocRepository = RepositoryFactory.GetRepository<IHOCRepository>();
            IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            IApplication application = appRepo.GetApplicationByKey(_applicationKey);
            IHOC applicationHOC = null;

            IAccount hocAccount = application.RelatedAccounts.Where(x => x.Product.Key == (int)Products.HomeOwnersCover).SingleOrDefault();
            if (hocAccount != null)
            {
                IFinancialService hocFinancialService = hocAccount.FinancialServices.Where(y => y.FinancialServiceType.Key == (int)SAHL.Common.Globals.FinancialServiceTypes.HomeOwnersCover).SingleOrDefault();
                if (hocAccount != null && hocFinancialService != null)
                {
                    applicationHOC = hocRepository.GetHOCByKey(hocFinancialService.Key);
                }
            }

            _applicationMortgageLoan = appRepo.GetApplicationByKey(_applicationKey) as IApplicationMortgageLoan;

            if (_applicationMortgageLoan != null && _applicationMortgageLoan.Property != null)
            {
                GlobalCacheData.Remove(ViewConstants.Application);
                GlobalCacheData.Add(ViewConstants.Application, _applicationMortgageLoan, LifeTimes);

                _view.valuation = _applicationMortgageLoan.Property.LatestCompleteValuation;

                if (application.ApplicationType.Key == (int)OfferTypes.ReAdvance
                || application.ApplicationType.Key == (int)OfferTypes.FurtherAdvance
                || application.ApplicationType.Key == (int)OfferTypes.FurtherLoan)
                {
                    _view.Navigator.Navigate("HOCFSSummary");
                }
                else
                {
                    if (applicationHOC == null)
                        _view.Navigator.Navigate("HOCFSSummaryAddApplication");
                    else
                        _view.Navigator.Navigate("HOCFSSummaryUpdateApplication");
                }
            }
            else
                _view.ShowDefaultView("Property and Valuation Records must be captured");

            #region Old Code

            //base.OnViewInitialised(sender, e);
            //if (!View.ShouldRunPage) return;

            //IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            //_applicationMortgageLoan = appRepo.GetApplicationByKey(_applicationKey) as IApplicationMortgageLoan;

            //if (_applicationMortgageLoan != null && _applicationMortgageLoan.Property != null && _applicationMortgageLoan.Property.LatestCompleteValuation != null)
            //if (_applicationMortgageLoan != null && _applicationMortgageLoan.Property != null )
            //{
            //    GlobalCacheData.Remove(ViewConstants.Application);
            //    GlobalCacheData.Add(ViewConstants.Application, _applicationMortgageLoan, LifeTimes);

            //    _view.valuation = _applicationMortgageLoan.Property.LatestCompleteValuation;

            //    if (_applicationMortgageLoan.Property.HOC == null)
            //        _view.Navigator.Navigate("HOCFSSummaryAddApplication");
            //    else
            //        _view.Navigator.Navigate("HOCFSSummaryUpdateApplication");
            //}
            //else
            //    _view.ShowDefaultView("Property and Valuation Records must be captured");

            #endregion Old Code
        }
    }
}