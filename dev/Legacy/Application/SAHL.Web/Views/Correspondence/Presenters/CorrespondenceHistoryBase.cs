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
using SAHL.Web.Views.Correspondence.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Globals;
using SAHL.Common.CacheData;
using SAHL.Common.Web.UI.Events;


namespace SAHL.Web.Views.Correspondence.Presenters.Correspondence
{
    /// <summary>
    /// 
    /// </summary>
    public class CorrespondenceHistoryBase : SAHLCommonBasePresenter<ICorrespondenceHistory>
    {
        private int _accountKey;
        private IEventList<ICorrespondence> _lstCorrespondence;
        private ICorrespondenceRepository _correspondenceRepo;
        private IAccountRepository _accountRepo;
        private IApplicationRepository _applicationRepo;
        private ICapRepository _capRepo;
        private IDebtCounsellingRepository _debtCounsellingRepo;
        private CBOMenuNode _node;
        private int _genericKeyValue;
        private int _genericKeyTypeKey;

        /// <summary>
        /// 
        /// </summary>
        public int AccountKey
        {
            get { return _accountKey; }
            set { _accountKey = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public CorrespondenceHistoryBase(ICorrespondenceHistory view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            // Get the CBO Node   
            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            if (_node == null)
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);

            _genericKeyValue = _node.GenericKey;
            _genericKeyTypeKey = _node.GenericKeyTypeKey;

            // if this is a static node : ie _node.GenericKey == -1 
            // then we must use the values off the parent node
            if (_node.GenericKey == -1)
            {
                if (_node.ParentNode != null)
                {
                    _genericKeyValue = _node.ParentNode.GenericKey;
                    _genericKeyTypeKey = _node.ParentNode.GenericKeyTypeKey;
                }
            }
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

            _correspondenceRepo = RepositoryFactory.GetRepository<ICorrespondenceRepository>();
            _accountRepo = RepositoryFactory.GetRepository<IAccountRepository>();
            _applicationRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            _capRepo = RepositoryFactory.GetRepository<ICapRepository>();
            _debtCounsellingRepo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();

            _view.onCancelButtonClicked += new EventHandler(onCancelButtonClicked);
            _view.onCallback += new KeyChangedEventHandler(onCallback);

            // Get the Correspondece entries
            // IList<int> genericKeys = new List<int>();
            // genericKeys.Add(_genericKeyValue);

            Dictionary<int, int> genericKeys = new Dictionary<int, int>();
            genericKeys.Add(_genericKeyValue, _genericKeyTypeKey);

            switch (_genericKeyTypeKey)
            {
                case (int)SAHL.Common.Globals.GenericKeyTypes.ParentAccount:
                case (int)SAHL.Common.Globals.GenericKeyTypes.Account:
                    // if the genrickeytype is account then we must look for an application aswell
                    IAccount account = _accountRepo.GetAccountByKey(_genericKeyValue);
                    if (account.Applications != null)
                    {
                        foreach (IApplication app in account.Applications)
                        {
                            if (!genericKeys.ContainsKey(app.Key))
                                genericKeys.Add(app.Key, (int)GenericKeyTypes.Offer);
                        }
                    } 

                    // get related child accounts aswel (if required)
                    if (_genericKeyTypeKey == (int)SAHL.Common.Globals.GenericKeyTypes.ParentAccount)
                    {
                        if (account.RelatedChildAccounts != null)
                        {
                            foreach (IAccount relatedAccount in account.RelatedChildAccounts)
                            {
                                if (!genericKeys.ContainsKey(relatedAccount.Key))
                                {
                                    genericKeys.Add(relatedAccount.Key, (int)GenericKeyTypes.Account);

                                    // look for applications on all related accounts aswell
                                    foreach (IApplication app in relatedAccount.Applications)
                                    {
                                        if (!genericKeys.ContainsKey(app.Key))
                                            genericKeys.Add(app.Key, (int)GenericKeyTypes.Offer);
                                    }
                                }
                                
                            }
                        }
                    }

                    if (account.UnderDebtCounselling)
                    {
                        List<IDebtCounselling> debtCounselling = _debtCounsellingRepo.GetDebtCounsellingByAccountKey(account.Key);
                        if (debtCounselling != null && debtCounselling.Count > 0)
                        {
                            foreach (var dc in debtCounselling)
                            {
                                genericKeys.Add(dc.Key, (int)GenericKeyTypes.DebtCounselling2AM);
                            }
                        }
                    }

                    break;

                case (int)SAHL.Common.Globals.GenericKeyTypes.FinancialService:
                    // if the genrickeytype is financialservice then we must look for an application aswell
                    IFinancialService financialService = RepositoryFactory.GetRepository<IFinancialServiceRepository>().GetFinancialServiceByKey(_genericKeyValue);
                    // if this a financialservice, add the accountkey to the generickey list
                    genericKeys.Add(financialService.Account.Key, (int)GenericKeyTypes.Account);

                    if (financialService.Account.Applications != null)
                    {
                        foreach (IApplication app in financialService.Account.Applications)
                        {
                            genericKeys.Add(app.Key, (int)GenericKeyTypes.Offer);
                        }
                    }
                    break;

                case (int)SAHL.Common.Globals.GenericKeyTypes.Offer:
                    IApplication application = _applicationRepo.GetApplicationByKey(_genericKeyValue);
                    if (application.ReservedAccount != null)
                    {
                        genericKeys.Add(application.ReservedAccount.Key, (int)GenericKeyTypes.Account);
                    }
                    break;
                case (int)SAHL.Common.Globals.GenericKeyTypes.CapOffer:
                    ICapApplication capApplication = _capRepo.GetCapOfferByKey(_genericKeyValue);
                    genericKeys.Add(capApplication.Account.Key, (int)GenericKeyTypes.Account);
                    break;
                default:
                    break;
            }

            // Get the Correspondece entries
            _lstCorrespondence = _correspondenceRepo.GetCorrespondenceByGenericKeys(genericKeys,false );

            // bind the data
            _view.BindHistoryGrid(_lstCorrespondence);

        }

        protected void onCallback(object sender, KeyChangedEventArgs e)
        {
            //Get the correspondence object to retrieve the correspondence detail
            int CorrespondenceKey = Convert.ToInt32(sender);
            ICorrespondenceRepository _correspondenceRepo = RepositoryFactory.GetRepository<ICorrespondenceRepository>();
            ICorrespondence correspondence = _correspondenceRepo.GetCorrespondenceByKey(CorrespondenceKey);

            _view.CorrespondenceDetailHTML = correspondence.CorrespondenceDetail.CorrespondenceText;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void onCancelButtonClicked(object sender, EventArgs e)
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

