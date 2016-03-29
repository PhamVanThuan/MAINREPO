using System;
using System.Collections.Generic;
using SAHL.Common;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.CacheData;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;

namespace SAHL.Web.Views.Common.Presenters
{
    /// <summary>
    ///
    /// </summary>
    public class ApplicationSummaryBase : SAHLCommonBasePresenter<IApplicationSummary>
    {
        private CBOMenuNode _node;
        private int _applicationKey;
        private ILookupRepository _lookupRepo;
        private IApplicationRepository _appRepo;
        private IRuleService _ruleService;
        private IApplication _application;

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
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            _view.AddTrace(this, "OnViewInitialised_Start");
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) return;

            _view.OnTransitionHistoryClicked += new EventHandler(_view_OnTransitionHistoryClicked);

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
                _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;

                //set the application key
                if (_node != null)
                    switch (_node.GenericKeyTypeKey)
                    {
                        case (int)SAHL.Common.Globals.GenericKeyTypes.Offer:
                            _applicationKey = _node.GenericKey;
                            break;
                        case (int)SAHL.Common.Globals.GenericKeyTypes.Account:
                        case (int)SAHL.Common.Globals.GenericKeyTypes.FinancialService:
                            int accountKey;
                            if (_node.GenericKeyTypeKey == (int)SAHL.Common.Globals.GenericKeyTypes.Account)
                            {
                                accountKey = _node.GenericKey;
                            }
                            else
                            {
                                IFinancialService financialService = RepositoryFactory.GetRepository<IFinancialServiceRepository>().GetFinancialServiceByKey(_node.GenericKey);
                                accountKey = financialService.Account.Key;
                            }
                            // get the latest open application from the account
                            IAccount account = RepositoryFactory.GetRepository<IAccountRepository>().GetAccountByKey(accountKey);
                            if (account != null && account.Applications.Count > 0)
                            {
                                foreach (IApplication app in account.Applications)
                                {
                                    if (app.IsOpen)
                                    {
                                        _applicationKey = app.Key;
                                        if (app.ApplicationStatus.Key == (int)SAHL.Common.Globals.OfferStatuses.NTU || app.ApplicationStatus.Key == (int)SAHL.Common.Globals.OfferStatuses.Declined)
                                        {
                                            string msg = String.Format(@"{0} in progress.", app.ApplicationStatus.Description);
                                            _view.Messages.Add(new SAHL.Common.DomainMessages.Warning(msg, msg));
                                        }

                                        break;
                                    }
                                }
                            }
                            break;
                        default:
                            break;
                    }
            }
            _view.AddTrace(this, "OnViewInitialised_Start_A");
            IApplicationRepository appRep = RepositoryFactory.GetRepository<IApplicationRepository>();

            if (_applicationKey > 0)
            {
                //Populate the Private Cache for navigate to History if it is valid
                if (PrivateCacheData.ContainsKey(ViewConstants.ApplicationKey))
                    PrivateCacheData.Remove(ViewConstants.ApplicationKey);

                PrivateCacheData.Add(ViewConstants.ApplicationKey, _applicationKey);

                IApplication app = appRep.GetApplicationByKey(_applicationKey);
                _view.application = app;
                if (app != null && app.ApplicationType.Key != (int)OfferTypes.Unknown)
                {
                    if (!_view.IsPostBack)
                    {
                        bool recalc = false;
                        //we need to check if the base rate has changed and recalc the application if it has
                        //this only applies to open applications that have not yet gone through credit.
                        if (app.IsOpen && app.GetLatestApplicationInformation().ApplicationInformationType.Key != (int)SAHL.Common.Globals.OfferInformationTypes.AcceptedOffer)
                        {
                            ISupportsVariableLoanApplicationInformation vlInfo = app.CurrentProduct as ISupportsVariableLoanApplicationInformation;
                            if (vlInfo != null)
                            {
                                if ((int)app.CurrentProduct.ProductType == (int)SAHL.Common.Globals.Products.VariFixLoan)
                                    recalc = true; //always recalc

                                if (!recalc)
                                {
                                    double baseRate = vlInfo.VariableLoanInformation.MarketRate.HasValue ? vlInfo.VariableLoanInformation.MarketRate.Value : 0D;

                                    IMarketRate mR = LookupRepo.MarketRates.ObjectDictionary[((int)SAHL.Common.Globals.MarketRates.ThreeMonthJIBARRounded).ToString()];
                                    if (mR.Value != baseRate) //18th reset rate
                                        recalc = true;
                                }
                            }
                        }
                        _view.AddTrace(this, "OnViewInitialised_Start_B");
                        if (recalc) //recalc and save
                        {
                            app.CalculateApplicationDetail(false, false);
                            _view.AddTrace(this, "OnViewInitialised_Start_C");
                            if (!_view.Messages.HasErrorMessages)
                                AppRepo.SaveApplication(app);
                            _view.AddTrace(this, "OnViewInitialised_Start_D");
                        }

                        _view.ShowCapitecDetails = app.IsCapitec();

                        _view.ShowComcorpDetails = app.IsComcorp();

                        _view.ShowIsGEPFDetails = app.IsGEPF();

                        _view.ShowStopOrderDiscountEligibility = app.HasAttribute(OfferAttributeTypes.StopOrderDiscount);
                     }
                }
                //Rate Adjustment Check

                RuleService.ExecuteRule(_view.Messages, "AccountDebtCounseling", Application);
                RuleService.ExecuteRule(_view.Messages, "LegalEntitiesUnderDebtCounsellingForAccount", Application);
                RuleService.ExecuteRule(_view.Messages, "RateAdjustmentCounterRateStillValid", Application);
                RuleService.ExecuteRule(_view.Messages, Rules.ApplicationIsAlphaHousing, Application);
                RuleService.ExecuteRule(_view.Messages, Rules.ApplicationIsReturningClient, Application);
                RuleService.ExecuteRule(_view.Messages, Rules.ApplicationProductEdgeLTVWarning, Application);
            }
            _view.AddTrace(this, "OnViewInitialised_End");
        }

        private void _view_OnTransitionHistoryClicked(object sender, EventArgs e)
        {
            _view.ShouldRunPage = false;
            if (GlobalCacheData.ContainsKey(ViewConstants.ApplicationKey))
                GlobalCacheData.Remove(ViewConstants.ApplicationKey);

            IList<ICacheObjectLifeTime> LifeTimes = new List<ICacheObjectLifeTime>();
            GlobalCacheData.Add(ViewConstants.ApplicationKey, PrivateCacheData[ViewConstants.ApplicationKey], LifeTimes);

            Navigator.Navigate("TransitionHistory");
        }

        protected ILookupRepository LookupRepo
        {
            get
            {
                if (_lookupRepo == null)
                    _lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();

                return _lookupRepo;
            }
        }

        protected IApplicationRepository AppRepo
        {
            get
            {
                if (_appRepo == null)
                    _appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

                return _appRepo;
            }
        }

        protected IRuleService RuleService
        {
            get
            {
                if (_ruleService == null)
                    _ruleService = ServiceFactory.GetService<IRuleService>();

                return _ruleService;
            }
        }

        protected IApplication Application
        {
            get
            {
                if (_application == null)
                {
                    _application = AppRepo.GetApplicationByKey(_applicationKey);
                }
                return _application;
            }
        }

    }
}