using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Cap.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.UI;
using System.Collections.Generic;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections;
using Castle.ActiveRecord;
using SAHL.Common.Exceptions;
using SAHL.Common.Collections.Interfaces;

using SAHL.Common.CacheData;
using SAHL.Common.DomainMessages;

namespace SAHL.Web.Views.Cap.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class CAPAcceptedHistoryCancel : SAHLCommonBasePresenter<ICAPAcceptedHistory>
    {
        #region Private Variables

        IList<ICapApplicationDetail> _capOfferDetailList;
        ICapRepository _capRepo;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "Waiting on Backend Changes trac 19227")]
        ILookupRepository _lookUpRepo;
        IList<ICancellationReason> _reasonList;
        IDictionary<int, IFinancialAdjustment> _finAdjustmentDict;

        #endregion


        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public CAPAcceptedHistoryCancel(ICAPAcceptedHistory view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!View.ShouldRunPage) return;

            _view.CancelCapRowVisible = true;
            _view.ButtonRowVisible = true;

            _view.OnCancelButtonClicked += new EventHandler(_view_OnCancelButtonClicked);
            _view.OnSubmitButtonClicked += new EventHandler(_view_OnSubmitButtonClicked);

            _lookUpRepo = RepositoryFactory.GetRepository<ILookupRepository>();

            //CBOMenuNode cboNode = new CBOMenuNode(null, 1441209, null, "", "");
            //CBOManager.AddCBOMenuNode(_view.Messages, _view.CurrentPrincipal, null, cboNode, CBONodeSetType.CBO);
            //CBOManager.SetCurrentCBONode(_view.CurrentPrincipal, cboNode, CBONodeSetType.CBO);
            CBOMenuNode cboNode = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;

            if (cboNode != null)
            {
                _capRepo = RepositoryFactory.GetRepository<ICapRepository>();
                IList<ICapApplication> capOfferList = _capRepo.GetAcceptedHistoryForCancel(cboNode.GenericKey);
                _capOfferDetailList = new List<ICapApplicationDetail>();
                _finAdjustmentDict = new Dictionary<int, IFinancialAdjustment>();

                foreach (ICapApplication _capApplication in capOfferList)
                {
                    foreach (ICapApplicationDetail _capApplicationDetail in _capApplication.CapApplicationDetails)
                    {
                        if (CanAdd(_capApplicationDetail))
                            _capOfferDetailList.Add(_capApplicationDetail);
                    }
                }

                _view.FinancialAdjustmentDict = _finAdjustmentDict;
                _view.BindGrid(_capOfferDetailList);
                _reasonList = _capRepo.GetCapCancellationReasons();
                _view.BindReasons(_reasonList);

                /*for (int i = 0; i < capOfferList.Count; i++)
                {
                    for (int j = 0; j < capOfferList[0].CapApplicationDetails.Count; j++)
                    {
                        if (capOfferList[i].CapStatus.Key == capOfferList[0].CapApplicationDetails[j].CapStatus.Key)
                            _capOfferDetailList.Add(capOfferList[0].CapApplicationDetails[j]);
                    }
                }*/
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("LoanSummary");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", Justification = "Waiting on Backend Changes trac 19227")]
        void _view_OnSubmitButtonClicked(object sender, EventArgs e)
        {
            if (!ValidationCheck)
                return;

            if (_capOfferDetailList != null && _capOfferDetailList.Count > 0)
            {

                var trans = new TransactionScope();
                try
                {
                    int accountkey = _capOfferDetailList[0].CapApplication.Account.Key;
                    _capRepo.OptOutCAP(accountkey, _view.SelectedCancellationReason, _view.CurrentPrincipal.Identity.Name);
                    trans.VoteCommit();
                }
                catch (Exception)
                {
                    trans.VoteRollBack();
                    if (_view.IsValid)
                        throw;
                }
                finally
                {
                    trans.Dispose();
                }


                if (_view.Messages.Count == 0)
                    _view.Navigator.Navigate("CapAcceptedHistory");
            }
        }

        #region Helper Methods

        public bool CanAdd(ICapApplicationDetail capAppDetail)
        {
            IMortgageLoanAccount mortgageLoanAcc = capAppDetail.CapApplication.Account as IMortgageLoanAccount;
            IMortgageLoan mortgageLoan = mortgageLoanAcc.SecuredMortgageLoan;

            foreach (IFinancialAdjustment _fa in mortgageLoan.FinancialAdjustments)
            {
                if (capAppDetail.CapTypeConfigurationDetail.CapTypeConfiguration.CapEffectiveDate == _fa.FromDate
                    && System.Math.Round(capAppDetail.CapTypeConfigurationDetail.Rate,5) == System.Math.Round(_fa.FixedRateAdjustment.Rate,5)
                    )
                {
                    _finAdjustmentDict.Add(capAppDetail.Key, _fa);
                    return true;
                }
            }

            return false;
        }

        private bool ValidationCheck
        {
            get 
            {
                string errorMessage = string.Empty;
                if (_view.SelectedCancellationReason == -1)
                {
                    errorMessage = "Please select a Cancellation Reason";
                    _view.Messages.Add(new Error(errorMessage, errorMessage));
                }
                return string.IsNullOrEmpty(errorMessage);
            }
        }

        #endregion

    }
}
