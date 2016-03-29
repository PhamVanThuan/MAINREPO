using System;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using SAHL.Common.UI;
using SAHL.Common.Globals;

using SAHL.Common.CacheData;

namespace SAHL.Web.Views.Common.Presenters.Memo
{
    /// <summary>
    /// Memo base - used by all memo presenters
    /// </summary>
    public class MemoBase : SAHLCommonBasePresenter<Interfaces.IMemo>
    {
        /// <summary>
        /// Memo Repository 
        /// </summary>
        protected IMemoRepository _memoRepository;
        /// <summary>
        /// Memo interface
        /// </summary>
        protected IEventList<IMemo> _memo;

        /// <summary>
        /// Selected Index on Grid
        /// </summary>
        protected int _memoIndexSelected;
        /// <summary>
        /// Generic Key
        /// </summary>
        protected int _genericKey;
        /// <summary>
        /// Generic Key Type
        /// </summary>
        protected int _genericKeyType;
        /// <summary>
        /// CBO Menu Node
        /// </summary>
        protected CBOMenuNode _node;
        /// <summary>
        /// Look Ups Repository
        /// </summary>
        protected ILookupRepository lookups;
        /// <summary>
        /// GenericTypeDescription
        /// </summary>
        protected string _genericCBONodeDescription;
        /// <summary>
        /// View to return to
        /// </summary>
        protected string _genericMemoSummary;
        /// <summary>
        /// Add View to return to
        /// </summary>
        protected string _genericMemoAdd;
        /// <summary>
        /// Update View to Return to
        /// </summary>
        protected string _genericMemoUpdate;
        /// <summary>
        /// Used by Test
        /// </summary>
        public IEventList<IMemo> memo
        {
            set
            {
                _memo = value;
            }
        }
        /// <summary>
        /// used on page load
        /// </summary>
        protected int _statusSelectedIndex; // = 0;
              
        /// <summary>
        /// Contructor for memo base
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public MemoBase(Interfaces.IMemo view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            {
            }
        }

        /// <summary>
        /// ON View Initialised event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!View.ShouldRunPage) return;

            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            if (_node != null)
            {
                _genericKey = Convert.ToInt32(_node.GenericKey);
                _genericKeyType = Convert.ToInt32(_node.GenericKeyTypeKey);
            }

            _view.CancelButtonClicked += (_view_CancelButtonClicked);

            switch (_genericKeyType)
            {
                case (int)GenericKeyTypes.Account:
                    _genericCBONodeDescription = GenericKeyTypes.Account.ToString();
                    _genericMemoSummary = "AccountMemoSummary";
                    _genericMemoAdd = "AccountMemoAdd";
                    _genericMemoUpdate = "AccountMemoUpdate";
                    break;
                case (int)GenericKeyTypes.ParentAccount:
                    _genericCBONodeDescription = GenericKeyTypes.Account.ToString();
                    _genericMemoSummary = "AccountMemoSummary";
                    _genericMemoAdd = "AccountMemoAdd";
                    _genericMemoUpdate = "AccountMemoUpdate";
                    _genericKeyType = (int)GenericKeyTypes.Account;
                    break;
                case (int)GenericKeyTypes.LegalEntity :
                    _genericCBONodeDescription = GenericKeyTypes.LegalEntity.ToString();
                    _genericMemoSummary = "LegalEntityMemoSummary";
                    _genericMemoAdd = "LegalEntityMemoAdd";
                    _genericMemoUpdate = "LegalEntityMemoUpdate";
                    break;
                case (int)GenericKeyTypes.RelatedLegalEntity:
                    _genericCBONodeDescription = GenericKeyTypes.RelatedLegalEntity.ToString();
                    _genericMemoSummary = "LegalEntityMemoSummary";
                    _genericMemoAdd = "LegalEntityMemoAdd";
                    _genericMemoUpdate = "LegalEntityMemoUpdate";
                    break;
                case (int)GenericKeyTypes.Valuation:
                    _genericCBONodeDescription = GenericKeyTypes.Valuation.ToString();
                    _genericMemoSummary = "MemoValuationDisplay";
                    _genericMemoAdd = "MemoValuationAdd";
                    _genericMemoUpdate = "MemoValuationUpdate";
                    break;
                case (int)GenericKeyTypes.Offer:
                    // check if this is a life offer and if it is then get the parent loan account
                    IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
                    IApplication application = appRepo.GetApplicationByKey(_genericKey);
                    if (application != null && application.ApplicationType.Key == (int)OfferTypes.Life)
                    {
                        //_genericKey = application.Account.GetRelatedAccountByType(AccountTypes.MortgageLoan, application.Account.RelatedParentAccounts).Key
                        _genericKey = application.Account.ParentAccount.Key;
                        _genericKeyType = (int)GenericKeyTypes.Account; 

                        _genericCBONodeDescription = GenericKeyTypes.Account.ToString();
                        _genericMemoSummary = "AccountMemoSummary";
                        _genericMemoAdd = "AccountMemoAdd";
                        _genericMemoUpdate = "AccountMemoUpdate";
                    }
                    else
                    {
                        _genericCBONodeDescription = "Application"; // have to hardcode this value as the enum returns "Offer"
                        _genericMemoSummary = "ApplicationMemoSummary";
                        _genericMemoAdd = "ApplicationMemoAdd";
                        _genericMemoUpdate = "ApplicationMemoUpdate";
                    }
                    break;
                case (int)GenericKeyTypes.CapOffer:
                    InstanceNode instanceNode = _node as InstanceNode;
                    if (instanceNode != null)
                    {
                        Dictionary<string, object> _x2Data = instanceNode.X2Data as Dictionary<string, object>;
                        if (_x2Data != null) _genericKey = Convert.ToInt32(_x2Data["AccountKey"]);
                    }
                    _genericKeyType = (int)GenericKeyTypes.Account;
                    _genericCBONodeDescription = GenericKeyTypes.Account.ToString();
                    _genericMemoSummary = "AccountMemoSummary";
                    _genericMemoAdd = "AccountMemoAdd";
                    _genericMemoUpdate = "AccountMemoUpdate";
                    break;
           }

            lookups = RepositoryFactory.GetRepository<ILookupRepository>();

            _memoRepository = RepositoryFactory.GetRepository<IMemoRepository>();
            _view.PopulateStatusDropDown();
            _view.PopulateStatusUpdateDropDown();

        }

        void _view_CancelButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate(_genericMemoSummary);
        }
    }
}
