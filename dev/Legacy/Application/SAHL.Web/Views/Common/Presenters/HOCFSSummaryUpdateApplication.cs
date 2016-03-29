using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Castle.ActiveRecord;
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

namespace SAHL.Web.Views.Common.Presenters
{
    /// <summary>
    /// HOCFSSummary Update application presenter - applies to HOC's of accounts not yet openeded
    /// </summary>
    public class HOCFSSummaryUpdateApplication : HOCFSSummaryBase
    {
        IEventList<IAddress> propertyAddressLst;
        IValuation valuation;
        IHOC HOC;
        bool PostBackCausedByGridClick;
        IApplicationMortgageLoan _applicationMortgageLoan;
        ILookupRepository lookups;
        IApplicationRepository appRepo;
        IHOCRepository hocRepo;
        ISupportsVariableLoanApplicationInformation _vlai;
        IApplicationInformationVariableLoan _vli;
        int _DefSelectedHOCInsurerValue;

        /// <summary>
        /// Constructor for HOC Application Update
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public HOCFSSummaryUpdateApplication(IHOCFSSummary view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            {
            }
        }

        /// <summary>
        /// OnViewInitialised event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!View.ShouldRunPage) return;

            BindValues();
        }

        /// <summary>
        /// View PreRender event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!View.ShouldRunPage) return;

            if (_applicationMortgageLoan.Property.LatestCompleteValuation != null &&
                _applicationMortgageLoan.Property.LatestCompleteValuation.IsActive)
            {
                valuation = _applicationMortgageLoan.Property.LatestCompleteValuation;
            }

            PrivateCacheData.Remove("LatestValuation");
            PrivateCacheData.Add("LatestValuation", valuation);

            if (PostBackCausedByGridClick) // Only Rebind Values if Postback was caused by GridClick
            {
                //BindValues();
                RetrieveHOCInsurer();
                _view.BindHOCSummaryData(HOC);
                PostBackCausedByGridClick = false;
                _view.SetBindValue = true;
            }

            if (_DefSelectedHOCInsurerValue == (int)HOCInsurers.SAHLHOC) // SAHLHOC
                _view.HOCPremiumPanelVisible = true;
            else
                _view.HOCPremiumPanelVisible = false;

            //--- Code Block removed as per ticket #9219 ----
            //if (Convert.ToInt32(_view.SelectedHOCInsurerValue) != HOC.HOCInsurer.Key)
            //    _view.RemoveHOCPolicyNumber();

            _view.ShowCalculatedHOCPremiums("UpdateApplication");
        }

        /// <summary>
        /// Gets the property selected on the Property Grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void _view_OnPropertiesGridSelectedIndexChanged(object sender, KeyChangedEventArgs e)
        {
            PostBackCausedByGridClick = true;
        }

        /// <summary>
        ///  Cancel Button Clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void _view_onCancelButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("Cancel");
        }

        /// <summary>
        /// Update event of HOC
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void _view_onUpdateHOCButtonClicked(object sender, EventArgs e)
        {
            if (!Validate())
                return;

            int hocInsurerKey = HOC.HOCInsurer.Key;

            if (PrivateCacheData.ContainsKey("LatestValuation"))
                valuation = PrivateCacheData["LatestValuation"] as IValuation;

            //Read HOC back off the DB to prevent getting seesion errors
            IHOC hoc = hocRepo.GetHOCByKey(HOC.Key);

            hoc = _view.GetCapturedHOCRecordForAdd(valuation, hoc);

            TransactionScope txn = new TransactionScope();
            try
            {
                hocRepo.SaveHOC(hoc);
                hocRepo.UpdateHOCPremium(hoc.Key);
                hoc.Refresh();
                hocRepo.UpdateHOCWithHistory(_view.Messages, hocInsurerKey, hoc, 'U');
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
            {
                GlobalCacheData.Remove(ViewConstants.Application);
                _view.Navigator.Navigate("Cancel");
            }
            else
            {
                PostBackCausedByGridClick = true;
                _view.SetBindValue = false;
            }
        }

        protected void _view_OnddlHOCInsurerSelectedIndexChanged(object sender, KeyChangedEventArgs e)
        {
            if (!(this.PrivateCacheData.ContainsKey("HOCSummarySelectedHOCInsurer")))
            {
                this.PrivateCacheData.Add("HOCSummarySelectedHOCInsurer", Convert.ToInt32(e.Key));
                if (Convert.ToInt32(_view.SelectedHOCInsurerValue) != HOC.HOCInsurer.Key)
                {
                    _view.RemoveHOCPolicyNumber();
                }

                if (Convert.ToInt32(_view.SelectedHOCInsurerValue) == (int)HOCInsurers.SAHLHOC) // SAHLHOC
                    _view.HOCPremiumPanelVisible = true;
                else
                    _view.HOCPremiumPanelVisible = false;
            }
            else
            {
                if (!(Convert.ToInt32(this.PrivateCacheData["HOCSummarySelectedHOCInsurer"]) == Convert.ToInt32(_view.SelectedHOCInsurerValue)))
                {
                    this.PrivateCacheData.Remove("HOCSummarySelectedHOCInsurer");
                    this.PrivateCacheData.Add("HOCSummarySelectedHOCInsurer", Convert.ToInt32(e.Key));
                    _view.RemoveHOCPolicyNumber();
                }

                if (Convert.ToInt32(_view.SelectedHOCInsurerValue) == (int)HOCInsurers.SAHLHOC) // SAHLHOC
                    _view.HOCPremiumPanelVisible = true;
                else
                    _view.HOCPremiumPanelVisible = false;
            }
        }

        private void BindValues()
        {
            _view.OnPropertiesGridSelectedIndexChanged += new KeyChangedEventHandler(_view_OnPropertiesGridSelectedIndexChanged);
            _view.OnddlHOCInsurerSelectedIndexChanged += new KeyChangedEventHandler(_view_OnddlHOCInsurerSelectedIndexChanged);
            _view.onUpdateHOCButtonClicked += new EventHandler(_view_onUpdateHOCButtonClicked);
            _view.onCancelButtonClicked += new EventHandler(_view_onCancelButtonClicked);

            propertyAddressLst = new EventList<IAddress>();
            appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            lookups = RepositoryFactory.GetRepository<ILookupRepository>();
            hocRepo = RepositoryFactory.GetRepository<IHOCRepository>();

            if (GlobalCacheData.ContainsKey(ViewConstants.Application))
            {
                _applicationMortgageLoan = GlobalCacheData[ViewConstants.Application] as IApplicationMortgageLoan;
                _applicationMortgageLoan = appRepo.GetApplicationByKey(_applicationMortgageLoan.Key) as IApplicationMortgageLoan;
            }

            _applicationMortgageLoan = appRepo.GetApplicationByKey(_applicationMortgageLoan.Key) as IApplicationMortgageLoan;
            if (_applicationMortgageLoan.Property != null)
            {
                if (_applicationMortgageLoan.Property.Address != null)
                    propertyAddressLst.Add(_view.Messages, _applicationMortgageLoan.Property.Address);

                if (_applicationMortgageLoan.Property.LatestCompleteValuation != null &&
                    _applicationMortgageLoan.Property.LatestCompleteValuation.IsActive)
                {
                    _view.valuation = _applicationMortgageLoan.Property.LatestCompleteValuation;
                }
            }

            _view.BindPropertiesGrid(propertyAddressLst);
            _view.SetControlsForAdd();

            IAccount hocAccount = _applicationMortgageLoan.RelatedAccounts.Where(x => x.Product.Key == (int)Products.HomeOwnersCover).SingleOrDefault();
            if (hocAccount != null)
            {
                IFinancialService hocFinancialService = hocAccount.FinancialServices.Where(y => y.FinancialServiceType.Key == (int)SAHL.Common.Globals.FinancialServiceTypes.HomeOwnersCover).SingleOrDefault();
                if (hocAccount != null && hocFinancialService != null)
                {
                    HOC = hocRepo.GetHOCByKey(hocFinancialService.Key);
                }
            }
            IEventList<IHOCInsurer> hocInsurerLst = new EventList<IHOCInsurer>();
            // Filter Only Active HOC Insurers
            for (int i = 0; i < lookups.HOCInsurers.Count; i++)
            {
                if (lookups.HOCInsurers[i].HOCInsurerStatus == (int)GeneralStatuses.Active)
                    hocInsurerLst.Add(_view.Messages, lookups.HOCInsurers[i]);
            }

            hocInsurerLst.Sort(
                delegate(IHOCInsurer h1, IHOCInsurer h2)
                {
                    return h1.Description.CompareTo(h2.Description);
                });

            if (!_view.IsPostBack)
                _DefSelectedHOCInsurerValue = HOC.HOCInsurer.Key;
            else
                _DefSelectedHOCInsurerValue = Convert.ToInt32(_view.SelectedHOCInsurerValue);

            _view.SelectedHOCInsurerValue = _DefSelectedHOCInsurerValue.ToString();

            // Determines the Total Sum Insured Amount
            if (_applicationMortgageLoan.ApplicationType.Key != (int)OfferTypes.FurtherAdvance &&
                 _applicationMortgageLoan.ApplicationType.Key != (int)OfferTypes.FurtherLoan &&
                 _applicationMortgageLoan.ApplicationType.Key != (int)OfferTypes.ReAdvance)
            {
                _vlai = _applicationMortgageLoan.CurrentProduct as ISupportsVariableLoanApplicationInformation;
                _vli = _vlai.VariableLoanInformation;

                if ((_vlai != null && _vli != null) && (_applicationMortgageLoan.ApplicationType.Key == (int)OfferTypes.NewPurchaseLoan))
                    _view.TotalHOCSumInsured = (_vli.LoanAgreementAmount.HasValue ? _vli.LoanAgreementAmount.Value : 0D);
                else if ((_vlai != null && _vli != null) && (_applicationMortgageLoan.ApplicationType.Key == (int)OfferTypes.SwitchLoan || _applicationMortgageLoan.ApplicationType.Key == (int)OfferTypes.RefinanceLoan))
                    _view.TotalHOCSumInsured = (_applicationMortgageLoan.ClientEstimatePropertyValuation.HasValue ? _applicationMortgageLoan.ClientEstimatePropertyValuation.Value : 0D);
                else
                    _view.TotalHOCSumInsured = 0;

                _view.UseDefaultValue = true;
            }
            else
                _view.UseDefaultValue = false;

            // Retrieve original HOC Insurer
            RetrieveHOCInsurer();

            _view.BindHOCInsurer(hocInsurerLst);
            _view.BindHOCSummaryData(HOC);
        }

        private void RetrieveHOCInsurer()
        {
            IHOC _hocRec = hocRepo.GetHOCByKey(HOC.Key);
            _view.SetHOCInsurerKey = _hocRec.HOCInsurer.Key;
        }
    }
}