using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Exceptions;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Web.Views.Common.Presenters;

namespace SAHL.Web.Views.Common.Presenters
{
    /// <summary>
    ///
    /// </summary>
    public class HOCFSSummaryUpdate : HOCFSSummaryBase
    {
        private IHOCRepository hocRepo;
        private ILookupRepository lookups;
        private IHOC _HOC;
        private IAccountRepository _accRepository;
        private CBOMenuNode _node;
        private int _genericKey;
        private int _DefSelectedHOCInsurerValue;
        private IEventList<IAddress> _addressLst;
        private IValuation _valuation;

        /// <summary>
        /// Used by Test
        /// </summary>
        public IHOC HOCAccount
        {
            set { _HOC = value; }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public HOCFSSummaryUpdate(IHOCFSSummary view, SAHLCommonBaseController controller)
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

            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            _genericKey = _node.GenericKey;

            _accRepository = RepositoryFactory.GetRepository<IAccountRepository>();
            hocRepo = RepositoryFactory.GetRepository<IHOCRepository>();

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
                case (int)SAHL.Common.Globals.GenericKeyTypes.Offer:
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

            lookups = RepositoryFactory.GetRepository<ILookupRepository>();
            IEventList<IHOCInsurer> hocInsurerLst = new EventList<IHOCInsurer>();

            // Filter Only Active HOC Insurers
            for (int i = 0; i < lookups.HOCInsurers.Count; i++)
            {
                if (lookups.HOCInsurers[i].HOCInsurerStatus == 1)
                    hocInsurerLst.Add(_view.Messages, lookups.HOCInsurers[i]);
            }

            hocInsurerLst.Sort(
                delegate(IHOCInsurer h1, IHOCInsurer h2)
                {
                    return h1.Description.CompareTo(h2.Description);
                });

            _view.BindHOCLookUpControls(hocInsurerLst, lookups.HOCStatus.BindableDictionary, lookups.HOCSubsidence.BindableDictionary, lookups.HOCConstruction.BindableDictionary);

            if (_HOC != null)
            {
                _view.valuation = _valuation;
                IHOCHistoryDetail hocHistoryDetail = hocRepo.GetLastestHOCHistoryDetail(_HOC.Key);
                if (hocHistoryDetail != null && (_valuation.ValuationDate < hocHistoryDetail.ChangeDate))
                {
                    _view.UseHOCHistoryDetail = true;
                    _view.HOCHistoryDetail = hocHistoryDetail;
                }

                if (!_view.IsPostBack)
                {
                    _DefSelectedHOCInsurerValue = _HOC.HOCInsurer.Key;

                    if (_HOC.HOCProrataPremium > 0)
                        _view.ShowProRataPremium = true;
                    else
                        _view.ShowProRataPremium = false;
                }
                else
                {
                    _DefSelectedHOCInsurerValue = Convert.ToInt32(_view.SelectedHOCInsurerValue);

                    if (Convert.ToInt32(_view.SelectedHOCInsurerValue) == (int)HOCInsurers.SAHLHOC && _HOC.HOCInsurer.Key != (int)HOCInsurers.SAHLHOC)
                    {
                        _view.ShowProRataPremium = true;
                    }
                    else if (Convert.ToInt32(_view.SelectedHOCInsurerValue) == (int)HOCInsurers.SAHLHOC && _HOC.HOCInsurer.Key == (int)HOCInsurers.SAHLHOC)
                    {
                        if ((Convert.ToInt32(_view.SelectedHOCStatusValue) != -1 && Convert.ToInt32(_view.SelectedHOCStatusValue) == (int)HocStatuses.PaidUpwithHOC)
                            && _HOC.HOCStatus.Key == (int)HocStatuses.PaidUpwithnoHOC)
                            _view.ShowProRataPremium = true;
                        else
                            _view.ShowProRataPremium = false;
                    }
                    else
                        _view.ShowProRataPremium = false;
                }

                _view.SelectedHOCInsurerValue = _DefSelectedHOCInsurerValue.ToString();
                _view.BindPropertiesGrid(_addressLst);
                RetrieveHOCInsurer();
                _view.BindHOCSummaryData(_HOC);
            }
            _view.onCancelButtonClicked += new EventHandler(_view_onCancelButtonClicked);
            _view.onUpdateHOCButtonClicked += new EventHandler(_view_onUpdateHOCButtonClicked);
        }

        /// <summary>
        /// OnViewPreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!View.ShouldRunPage) return;

            _view.HOCDetailsDisplay = false;
            _view.HOCDetailsUpdate = true;
            _view.HOCCancelButtonVisible = true;
            _view.HOCUpdateButtonVisible = true;

            _view.HOCDetailsUpdateDisplay = true;

            if (_HOC.HOCInsurer.Key == (int)HOCInsurers.SAHLHOC)
                _view.HOCPremiumPanelVisible = true;
            else
                _view.HOCPremiumPanelVisible = false;

            if (Convert.ToInt32(_view.SelectedHOCInsurerValue) == (int)HOCInsurers.SAHLHOC)
                _view.HOCPremiumPanelVisible = true;
            else
                _view.HOCPremiumPanelVisible = false;

            if (Convert.ToInt32(_view.SelectedHOCStatusValue) == (int)HocStatuses.PaidUpwithnoHOC)
                _view.HOCInsurerValueChange = (int)HOCInsurers.PaidupwithnoHOC;

            if (Convert.ToInt32(_view.SelectedHOCStatusValue) == (int)HocStatuses.Closed)
                _view.HOCInsurerValueChange = (int)HOCInsurers.LoanCancelled_Closed;

            if (Convert.ToInt32(_view.SelectedHOCInsurerValue) != _HOC.HOCInsurer.Key)
                _view.RemoveHOCPolicyNumber();

            if (Convert.ToInt32(_view.SelectedHOCInsurerValue) == (int)HOCInsurers.SAHLHOC)
                _view.ShowCalculatedHOCPremiums("Update");
        }

        private void _view_onCancelButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("Cancel");
        }

        private void _view_onUpdateHOCButtonClicked(object sender, EventArgs e)
        {
            if (!Validate())
                return;

            int prevHocInsurerKey = _HOC.HOCInsurer.Key;
            hocRepo = RepositoryFactory.GetRepository<IHOCRepository>();
            TransactionScope txn = new TransactionScope();
            try
            {
                IHOC hoc = _view.GetCapturedHOC(_HOC);

                if (Convert.ToInt32(_view.SelectedHOCInsurerValue) == (int)HOCInsurers.SAHLHOC)
                {
                    hoc.CalculatePremiumForUpdate();
                }

                hocRepo.SaveHOC(hoc);
                hocRepo.UpdateHOCPremium(hoc.Key);
                hoc.Refresh();
                hocRepo.UpdateHOCWithHistory(_view.Messages, prevHocInsurerKey, hoc, 'U');

                if (!_view.IsValid)
                    throw new Exception();

                txn.VoteCommit();
            }

            catch (Exception)
            {
                txn.VoteRollBack();
                if (_view.IsValid)
                    throw;
            }

            finally
            {
                txn.Dispose();
            }

            if (_view.IsValid)
                _view.Navigator.Navigate("Cancel");
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

            foreach (IAccount account in app.RelatedAccounts)
            {
                if (account.Product.Key == (int)Products.HomeOwnersCover)
                {
                    _hocAcct = account as IAccountHOC;
                    _HOC = _hocAcct.HOC;
                    break;
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
    }
}