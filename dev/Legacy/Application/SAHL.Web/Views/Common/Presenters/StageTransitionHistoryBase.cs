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
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Globals;
using SAHL.Common.CacheData;


namespace SAHL.Web.Views.Common.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class StageTransitionHistoryBase : SAHLCommonBasePresenter<IStageTransitionHistory>
    {
        private IStageDefinitionRepository _stageDefinitionRepo;
        private IAccountRepository _accountRepo;
        private CBONode _node;
        private int _genericKeyValue;
        private int _genericKeyTypeKey;
        private DataTable _dtStageTransitions;

		private const string StageTransitions = "DTstageTransitions";

		private List<ICacheObjectLifeTime> _lifeTimes;
		protected List<ICacheObjectLifeTime> LifeTimes
		{
			get
			{
				if (_lifeTimes == null)
				{
					List<string> views = new List<string>();
					views.Add("StageTransitionHistoryBase");
					views.Add("StageTransitionHistory");
					views.Add("StageTransitionHistoryLifeOrigination");
					views.Add("WF_StageTransitionHist");
					views.Add("Life_StageTransitionHistoryOrigination");
					views.Add("Life_StageTransitionHistoryAdmin");
					_lifeTimes = new List<ICacheObjectLifeTime>();
					_lifeTimes.Add(new SimplePageCacheObjectLifeTime(views));
				}
				return _lifeTimes;
			}
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public StageTransitionHistoryBase(IStageTransitionHistory view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            // Get the CBO Node   
            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal);
            if (_node == null)
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);

            _genericKeyValue = _node.GenericKey;
            _genericKeyTypeKey = _node.GenericKeyTypeKey;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) return;

            _stageDefinitionRepo = RepositoryFactory.GetRepository<IStageDefinitionRepository>();
            _accountRepo = RepositoryFactory.GetRepository<IAccountRepository>();
            _view.onCancelButtonClicked += new EventHandler(_view_onCancelButtonClicked);

            IDictionary<GenericKeyTypes, List<int>> dicGenricKeyTypeAndKeys = new Dictionary<GenericKeyTypes, List<int>>();
            List<int> genericKeys = new List<int>();

            // add the generickeytype and generickey to our dictionary
            genericKeys.Add(_genericKeyValue);
            dicGenricKeyTypeAndKeys.Add((GenericKeyTypes)_genericKeyTypeKey, genericKeys);

            // logic for specific generic key types
            switch (_genericKeyTypeKey)
            {
                case (int)SAHL.Common.Globals.GenericKeyTypes.ParentAccount:
                case (int)SAHL.Common.Globals.GenericKeyTypes.Account:
                    //genericKeys.Add(_genericKeyValue);
                    //dicGenricKeyTypeAndKeys.Add(GenericKeyTypes.Account, genericKeys);

                    // Get linked Applications
                    IAccount account = _accountRepo.GetAccountByKey(_genericKeyValue);
                    if (account.Applications != null)
                    {
                        genericKeys = new List<int>();
                        foreach (IApplication application in account.Applications)
                        {
                            genericKeys.Add(application.Key);
                        }

                        dicGenricKeyTypeAndKeys.Add(GenericKeyTypes.Offer, genericKeys);
                    }

                    // Get Linked Cap Applications
                    ICapRepository capRepo = RepositoryFactory.GetRepository<ICapRepository>();
                    IList<ICapApplication> capApplications = capRepo.GetCapOfferByAccountKey(account.Key);
                    if (capApplications.Count > 0)
                    {
                        genericKeys = new List<int>();
                        foreach (ICapApplication capApp in capApplications)
                        {
                            genericKeys.Add(capApp.Key);
                        }
                        dicGenricKeyTypeAndKeys.Add(GenericKeyTypes.CapOffer, genericKeys);
                    }

                    // Get Linked Debt Counselling Applications
                    IDebtCounsellingRepository dcRepo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();
                    IList<IDebtCounselling> debtCounselling = dcRepo.GetDebtCounsellingByAccountKey(account.Key);
                    if (debtCounselling.Count > 0)
                    {
                        genericKeys = new List<int>();
                        foreach (IDebtCounselling dc in debtCounselling)
                        {
                            genericKeys.Add(dc.Key);
                        }
                        dicGenricKeyTypeAndKeys.Add(GenericKeyTypes.DebtCounselling2AM, genericKeys);
                    }
                    _view.GenericKeyColumnVisible = true;
                    break;
                case (int)SAHL.Common.Globals.GenericKeyTypes.Offer:
                    _view.GenericKeyColumnVisible = true;
                    break;
                default:
                    break;
            }

            //if postback use the cache object
            if (_view.IsPostBack)
                _dtStageTransitions = GlobalCacheData[StageTransitionHistoryBase.StageTransitions] as DataTable;

            //TRAC 16405: sometimes the private cache does not get recognised when coming back the first time
            //this then needs to be reset. This code is here on purpose 
            
            //private data cache issue hack bug fix 16045
            //the first use of the private cache sometimes does not work, but does after the first load
            //this will resolve the issue and does not affect the original implementation
            if (_dtStageTransitions == null)
            {
                _dtStageTransitions = _stageDefinitionRepo.GetStageTransitionDTByGenericKeyTypeAndKeys(dicGenricKeyTypeAndKeys);
				GlobalCacheData.Add(StageTransitionHistoryBase.StageTransitions, _dtStageTransitions, LifeTimes);
            }

            _view.SetUpGrid();
        }

        protected override void  OnViewLoaded(object sender, EventArgs e)
        {
 	         base.OnViewLoaded(sender, e);
             if (!_view.ShouldRunPage) 
                 return;

             if (_dtStageTransitions.Rows.Count > 0)
             {
                 _view.BindHistoryGrid(_dtStageTransitions);
             }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void _view_onCancelButtonClicked(object sender, EventArgs e)
        {
            if (GlobalCacheData.ContainsKey(ViewConstants.NavigateTo))
            {
                string navigateTo = GlobalCacheData[ViewConstants.NavigateTo].ToString();
                GlobalCacheData.Remove(ViewConstants.NavigateTo);
                _view.Navigator.Navigate(navigateTo);
            }
            else
                _view.Navigator.Navigate("Cancel");
        }
    }
}
