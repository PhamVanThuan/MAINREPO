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
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Collections;



namespace SAHL.Web.Views.Common.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class HOCFSSummary : HOCFSSummaryBase
    { 
        private IHOC _HOC;
        private IAccountRepository _accRepository;
        private CBOMenuNode _node;
        private IHOCInsurer _hocInsurer;
        private int _genericKey;
        private IEventList<IAddress> _addressLst;
        private IValuation _valuation;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public HOCFSSummary(IHOCFSSummary view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            {

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
            if (!View.ShouldRunPage) return;

            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            _genericKey = _node.GenericKey;
            _accRepository = RepositoryFactory.GetRepository<IAccountRepository>();
            _view.OnPropertiesGridSelectedIndexChanged += new KeyChangedEventHandler(_view_OnPropertiesGridSelectedIndexChanged);

            switch (_node.GenericKeyTypeKey)
            {
                case (int)SAHL.Common.Globals.GenericKeyTypes.Account:
                        RetrieveHOCByAccountKey(_genericKey);
                    break;
                case (int)SAHL.Common.Globals.GenericKeyTypes.Property:
                    _genericKey = _node.ParentNode.GenericKey;
                    if (_node.ParentNode.GenericKeyTypeKey == (int)SAHL.Common.Globals.GenericKeyTypes.Account)
                        RetrieveHOCByAccountKey(_genericKey);
                    else if (_node.ParentNode.GenericKeyTypeKey == (int)SAHL.Common.Globals.GenericKeyTypes.Offer)
                        RetrieveHOCByOfferKey(_genericKey);
                    else 
                        throw new Exception("Not supported");
                    break;
                case (int)SAHL.Common.Globals.GenericKeyTypes.Offer :
                        RetrieveHOCByOfferKey(_genericKey);
                    break;
                case (int)SAHL.Common.Globals.GenericKeyTypes.FinancialService:
                        _genericKey = _node.ParentNode.ParentNode.GenericKey;
                        RetrieveHOCByAccountKey(_genericKey);
                    break;
                default:
                        throw new Exception("Not supported");
                    break;
            }
        
            if (_HOC != null)
            {
                _hocInsurer = _HOC.HOCInsurer;
                _view.valuation = _valuation;
                _view.SelectedHOCInsurerValue = _HOC.HOCInsurer.Key.ToString();
                _view.BindPropertiesGrid(_addressLst);
                RetrieveHOCInsurer();
                _view.IsHOCFSSummary = true;
                _view.BindHOCSummaryData(_HOC);
            }
          }

        /// <summary>
        /// OnView PreRender event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!View.ShouldRunPage) return;

            if (_HOC != null)
            {
                _view.HOCDetailsDisplay = true;
                _view.HOCDetailsUpdate = false;
                _view.HOCCancelButtonVisible = false;
                _view.HOCUpdateButtonVisible = false;
                _view.HOCDetailsUpdateDisplay = true;

                _view.ShowCalculatedHOCPremiums("HOCFSSummary");

                if (_hocInsurer.Key == (int)HOCInsurers.SAHLHOC)
                    _view.HOCPremiumPanelVisible = true;
                else
                    _view.HOCPremiumPanelVisible = false;
            }
            else
                _view.ShowDefaultView("No HOC Record");
        }

        private void RetrieveHOCInsurer()
        {
            IHOCRepository _hocRepo = RepositoryFactory.GetRepository<IHOCRepository>();
            IHOC _hocRec = _hocRepo.GetHOCByKey(_HOC.Key);
            _view.SetHOCInsurerKey = _hocRec.HOCInsurer.Key;
        }

        private void RetrieveHOCByAccountKey(int AccountKey)
        {
            _addressLst = new EventList<IAddress>();
            IAccount account = _accRepository.GetAccountByKey(AccountKey);
            IMortgageLoanAccount mortgageLoanAccount = account as IMortgageLoanAccount;
            IAccountHOC _hocAcct = null;

            foreach (IAccount acc in account.RelatedChildAccounts)
            {
                if (acc.Product.Key == (int)Products.HomeOwnersCover)
                {
                    _hocAcct = _accRepository.GetAccountByKey(acc.Key) as IAccountHOC;
                    break;
                }
            }

            if (_hocAcct != null)
                _HOC = _hocAcct.HOC;

            if (mortgageLoanAccount != null && mortgageLoanAccount.SecuredMortgageLoan.Property != null)
            {
                if (mortgageLoanAccount.SecuredMortgageLoan.Property.LatestCompleteValuation != null
                && mortgageLoanAccount.SecuredMortgageLoan.Property.LatestCompleteValuation.IsActive)
                    _valuation = mortgageLoanAccount.SecuredMortgageLoan.Property.LatestCompleteValuation;

                if (mortgageLoanAccount.SecuredMortgageLoan.Property.Address != null)
                    _addressLst.Add(_view.Messages, mortgageLoanAccount.SecuredMortgageLoan.Property.Address);
            }
        }

        private void RetrieveHOCByOfferKey(int ApplicationKey)
        {
            _addressLst = new EventList<IAddress>();
            IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            IApplicationMortgageLoan applicationMortgageLoan = appRepo.GetApplicationByKey(ApplicationKey) as IApplicationMortgageLoan;
            IApplication app = appRepo.GetApplicationByKey(ApplicationKey);
            IAccountHOC _hocAcct = null;

            if (app.ApplicationType.Key == (int)OfferTypes.ReAdvance
                || app.ApplicationType.Key == (int)OfferTypes.FurtherAdvance
                || app.ApplicationType.Key == (int)OfferTypes.FurtherLoan)
            {
                foreach (IAccount account in app.Account.RelatedChildAccounts)
                {
                    if (account.Product.Key == (int)Products.HomeOwnersCover)
                    {
                        _hocAcct = account as IAccountHOC;
                        _HOC = _hocAcct.HOC;
                        break;
                    }
                }
            }
            else
            {
                foreach (IAccount account in app.RelatedAccounts)
                {
                    if (account.Product.Key == (int)Products.HomeOwnersCover)
                    {
                        _hocAcct = account as IAccountHOC;
                        _HOC = _hocAcct.HOC;
                        break;
                    }
                }
            }

            if (applicationMortgageLoan.Property != null)
            {
                if (applicationMortgageLoan.Property.LatestCompleteValuation != null
                && applicationMortgageLoan.Property.LatestCompleteValuation.IsActive)
                    _valuation = applicationMortgageLoan.Property.LatestCompleteValuation;

                if (applicationMortgageLoan.Property.Address != null)
                    _addressLst.Add(_view.Messages, applicationMortgageLoan.Property.Address);
            }
        }

        protected void _view_OnPropertiesGridSelectedIndexChanged(object sender, KeyChangedEventArgs e)
        {
        }
     }
}
