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
    /// HOC - Add Presenter
    /// </summary>
    public class HOCFSSummaryAddApplication : HOCFSSummaryBase
    {
        IEventList<IAddress> propertyAddressLst;
        IValuation valuation;
        ILookupRepository lookups;
        IApplicationRepository appRepo;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "Waiting on Backend Changes trac 19349")] // -- Incorrect ticket number?
        IHOCRepository hocRepo;
        IApplicationMortgageLoan _applicationMortgageLoan;
        ISupportsVariableLoanApplicationInformation _vlai;
        IApplicationInformationVariableLoan _vli;
        int _DefSelectedHOCInsurerValue;

        /// <summary>
        /// Constructor for HOC Add
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public HOCFSSummaryAddApplication(IHOCFSSummary view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            {
            }
        }

        /// <summary>
        /// OnView Initialised event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!View.ShouldRunPage) return;

            propertyAddressLst = new EventList<IAddress>();

            lookups = RepositoryFactory.GetRepository<ILookupRepository>();
            appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            hocRepo = RepositoryFactory.GetRepository<IHOCRepository>();

            if (GlobalCacheData.ContainsKey(ViewConstants.Application))
                _applicationMortgageLoan = GlobalCacheData[ViewConstants.Application] as IApplicationMortgageLoan;

            _applicationMortgageLoan = appRepo.GetApplicationByKey(_applicationMortgageLoan.Key) as IApplicationMortgageLoan;
            if (_applicationMortgageLoan.Property != null)
            {
                if (_applicationMortgageLoan.Property.Address != null)
                    propertyAddressLst.Add(_view.Messages, _applicationMortgageLoan.Property.Address);

                if (_applicationMortgageLoan.Property.LatestCompleteValuation != null &&
                    _applicationMortgageLoan.Property.LatestCompleteValuation.IsActive)
                {
                    valuation = _applicationMortgageLoan.Property.LatestCompleteValuation;
                    _view.valuation = valuation;
                }
            }

            _view.BindPropertiesGrid(propertyAddressLst);
            _view.SetControlsForAdd();

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

            _view.BindHOCInsurer(hocInsurerLst);
            _view.onUpdateHOCButtonClicked += new EventHandler(_view_onUpdateHOCButtonClicked);
            _view.onCancelButtonClicked += new EventHandler(_view_onCancelButtonClicked);
            _view.SetUpdateButtonText = "Add";

            _applicationMortgageLoan = appRepo.GetApplicationByKey(_applicationMortgageLoan.Key) as IApplicationMortgageLoan;

            if (!_view.IsPostBack)
                _DefSelectedHOCInsurerValue = -1;
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
        }

        /// <summary>
        /// OnView PreRender Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!View.ShouldRunPage) return;

            _view.SetDefaultValuesForAdd(lookups.HOCStatus, lookups.HOCSubsidence, lookups.HOCRoof, lookups.HOCConstruction);
            _view.SetBindValue = true;

            if (Convert.ToInt32(_view.SelectedHOCInsurerValue) == (int)HOCInsurers.SAHLHOC) // SAHLHOC
            {
                _view.HOCPremiumPanelVisible = true;
                _view.ShowCalculatedHOCPremiums("Add");
            }
            else
                _view.HOCPremiumPanelVisible = false;
        }

        /// <summary>
        /// Cancel Button Clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void _view_onCancelButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("Cancel");
        }

        /// <summary>
        /// Update Button click Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///
        protected void _view_onUpdateHOCButtonClicked(object sender, EventArgs e)
        {
            appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

            if (!Validate())
                return;

            TransactionScope txn = new TransactionScope();
            try
            {
                // Create HOC FinancialService/Account structure via Back-End API
                IHOC hoc = hocRepo.CreateHOC(_applicationMortgageLoan.Key);

                IApplicationMortgageLoan appML = appRepo.GetApplicationByKey(_applicationMortgageLoan.Key) as IApplicationMortgageLoan;
                appML.RelatedAccounts.Add(_view.Messages, hoc.FinancialService.Account);
                appRepo.SaveApplication(appML);

                hoc = _view.GetCapturedHOCRecordForAdd(valuation, hoc);

                hocRepo.SaveHOC(hoc);
                hocRepo.UpdateHOCPremium(hoc.Key);
                hoc.Refresh();

                hoc.HOCHistory = hocRepo.CreateHOCHistoryRecord(hoc);
                IHOCHistoryDetail hocHistoryDetail = hocRepo.CreateHOCHistoryDetailRecord(hoc);
                hocHistoryDetail.HOCHistory = hoc.HOCHistory;
                hoc.HOCHistory.HOCHistoryDetails.Add(_view.Messages, hocHistoryDetail);

                hocRepo.SaveHOCHistory(hoc.HOCHistory);
                hocRepo.SaveHOC(hoc);
                hocRepo.SaveHOCHistoryDetail(hocHistoryDetail);
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
                _view.SetBindValue = false;
            }
        }
    }
}