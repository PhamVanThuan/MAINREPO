using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.FurtherLending.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using System.Linq;
using SAHL.Common.CacheData;
using System.Collections.Generic;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Web.Views.FurtherLending.Presenters
{
    public class ApplicationSummaryBase : SAHLCommonBasePresenter<IApplicationSummary>
    {
        #region locals

        private int _applicationKey;
        private IAccount _account;
        private IMortgageLoan _vML;
        private IMortgageLoan _fML;
        private int _ArrearMonthCheck = 6;
        private float _ArrearMinimumValueCheck = 500;
        private IApplication _application;

        private IAccountRepository _accountRepo;
        private IApplicationRepository _applicationRepo;
        
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public ApplicationSummaryBase(IApplicationSummary view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage) return;

            if (_view.IsMenuPostBack)
                GlobalCacheData.Clear();

            _view.OnTransitionHistoryClicked += new EventHandler(_view_OnOnTransitionHistoryClicked);

            //No need to cache this key, not time to test or figure out why this is here in the first place....
            if (GlobalCacheData.ContainsKey(ViewConstants.ApplicationKey))
            {
                // get the applicatonkey from the global cache and clear it out IMMEDIATELY, otherwise we run 
                // the risk of the user clicking a menu option and seeing the wrong application - can't rely on 
                // button clicks etc - if you need this for postbacks use a PrivateCache
                _applicationKey = Convert.ToInt32(GlobalCacheData[ViewConstants.ApplicationKey]);
                GlobalCacheData.Remove(ViewConstants.ApplicationKey);
            }
            else
            {
                // get the cbo node
                CBOMenuNode node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;

                //set the application key
                if (node.GenericKeyTypeKey == (int)GenericKeyTypes.Offer)
                    _applicationKey = int.Parse(node.GenericKey.ToString());
            }


            //Populate the Private Cache for navigate to History if it is valid
            if (_applicationKey > 0)
            {
                if (PrivateCacheData.ContainsKey(ViewConstants.ApplicationKey))
                    PrivateCacheData.Remove(ViewConstants.ApplicationKey);

                PrivateCacheData.Add(ViewConstants.ApplicationKey, _applicationKey);
            }

            if (_applicationKey > 0)
            {
                _view.CurrentApplicationKey = _applicationKey;

                _application = ApplicationRepo.GetApplicationByKey(_applicationKey);
                // Get the Account Object
                _account = AccountRepo.GetAccountByApplicationKey(_applicationKey);

                //Make sure we have a Valid account 
                if ((_account as IMortgageLoanAccount) == null)
                {
                    throw new Exception("Not a Mortgage Loan Account!");
                }

                // if we are a 30yr and there are more then 240months remaining, then we want also show 20yr figures
                _view.Show20YearFigures = ApplicationRepo.Display20YearFiguresOn30YearLoan(_account);

                SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
                IRuleService svc = ServiceFactory.GetService<IRuleService>();
                List<string> rules = new List<string>();
                rules.Add("AccountBaselIIDecline");
                rules.Add("AccountBaselIIRefer");
                svc.ExecuteRuleSet(spc.DomainMessages, rules, _application);

                //Get the variable ML
                IMortgageLoanAccount mla = _account as IMortgageLoanAccount;
                _vML = mla.SecuredMortgageLoan;

                //Get the Fixed ML  
                if ((_account as IAccountVariFixLoan) != null)
                {
                    IAccountVariFixLoan _fAccount = _account as IAccountVariFixLoan;
                    _fML = _fAccount.FixedSecuredMortgageLoan;
                }

                _view.HasArrears = _account.HasBeenInArrears(_ArrearMonthCheck, _ArrearMinimumValueCheck);

                _view.TitleDeedOnFile = AccountRepo.TitleDeedsOnFile(_account.Key);

                _view.ShowStopOrderDiscountEligibility = _application.HasAttribute(OfferAttributeTypes.StopOrderDiscount);

                
                ExecuteRules();
            }
        }

        /// <summary>
        /// Set the relevant properties for displaying controls within the view
        /// </summary>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {

            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage) return;

            if (_vML != null)
                _view.BindDisplay(_vML, _fML);
			if (_application != null)
			{
				_view.BindGrid(_application);
			}
        }

        void _view_OnOnTransitionHistoryClicked(object sender, EventArgs e)
        {
            _view.ShouldRunPage = false;
            if (GlobalCacheData.ContainsKey(ViewConstants.ApplicationKey))
                GlobalCacheData.Remove(ViewConstants.ApplicationKey);

            IList<ICacheObjectLifeTime> LifeTimes = new List<ICacheObjectLifeTime>();
            GlobalCacheData.Add(ViewConstants.ApplicationKey, PrivateCacheData[ViewConstants.ApplicationKey], LifeTimes);

            Navigator.Navigate("TransitionHistory");
        }

        protected IAccountRepository AccountRepo
        {
            get
            {
                if (_accountRepo == null)
                    _accountRepo = RepositoryFactory.GetRepository<IAccountRepository>();

                return _accountRepo;
            }
        }

        protected IApplicationRepository ApplicationRepo
        {
            get 
            {
                if (_applicationRepo == null)
                    _applicationRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

                return _applicationRepo;
            }
        }

        private void ExecuteRules()
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
            IRuleService svc = ServiceFactory.GetService<IRuleService>();
			svc.ExecuteRule(spc.DomainMessages, "AccountDebtCounseling", _application);
			svc.ExecuteRule(spc.DomainMessages, "LegalEntitiesUnderDebtCounsellingForAccount", _application);
            svc.ExecuteRule(spc.DomainMessages, "OfferroleMatchAccountroleandLEKeySuretyCheck", _application);
            svc.ExecuteRule(_view.Messages, "AccountIsAlphaHousing", _application.Account);
        }
    }
}
