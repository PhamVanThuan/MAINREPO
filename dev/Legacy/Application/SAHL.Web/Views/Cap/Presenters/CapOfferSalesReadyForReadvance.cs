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
using SAHL.Common.Web.UI.Controls;
using System.Collections;
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Factories;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Service;
using SAHL.Common.Exceptions;
using SAHL.Common.Globals;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.CacheData;

namespace SAHL.Web.Views.Cap.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class CapOfferSalesReadyForReadvance : CapOfferSalesBase
    {
        #region Constructor

        public CapOfferSalesReadyForReadvance(ICapOfferSales view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        #endregion


        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage) return;

            _view.PaymentOptionDropDownVisible = false;
            LoadCapOffer();

            BindInstalmentValues("Instalment on Quote");
            _view.ReasonDropDownVisible = false;
            _view.PromotionCheckBoxEnabled = false;
            _view.ButtonRowVisible = true;
            _view.CapGridPostBackType = GridPostBackType.NoneWithClientSelect;
            _view.SubmitButtonText = "Documents Received";
            _view.SubmitButtonEnabled = false;
            _view.ConfirmMessageText = "Are you sure you that you have received all signed legal documents?";
            _view.CancelButtonVisible = false;

            _view.OnCancelButtonClicked += new EventHandler(_view_OnCancelButtonClicked);
            _view.OnSubmitButtonClicked += new EventHandler(_view_OnSubmitButtonClicked);

            IList<ICapApplicationDetail> detailList = new List<ICapApplicationDetail>();
            for (int i = 0; i < _capOffer.CapApplicationDetails.Count; i++)
            {
                if (_capOffer.CapApplicationDetails[i].CapStatus.Key == Convert.ToInt32(SAHL.Common.Globals.CapStatuses.AwaitingLA))
                {
                    detailList.Add(_capOffer.CapApplicationDetails[i]);
                    _view.SubmitButtonEnabled = true;
                }
            }
            _view.BindCapGrid(detailList);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("Cancel");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _view_OnSubmitButtonClicked(object sender, EventArgs e)
        {
            X2Service svc = ServiceFactory.GetService<IX2Service>() as X2Service;
            TransactionScope txn = new TransactionScope();
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
            IRuleService ruleSVC = ServiceFactory.GetService<IRuleService>();

            try
            {
                ILookupRepository lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();
                for (int i = 0; i < _capOffer.CapApplicationDetails.Count; i++)
                {
                    if (_capOffer.CapApplicationDetails[i].CapStatus.Key == Convert.ToInt32(CapStatuses.AwaitingLA))
                    {
                        _capOffer.CapApplicationDetails[i].CapStatus = lookupRepo.CapStatuses.ObjectDictionary[Convert.ToInt32(CapStatuses.ReadvanceRequired).ToString()];
                    }
                }
                _capOffer.Promotion = false;
                _capOffer.CapStatus = lookupRepo.CapStatuses.ObjectDictionary[Convert.ToInt32(CapStatuses.ReadvanceRequired).ToString()];

                // Rule ApplicationCap2CheckReadvanceRequired check if it's a Further Loan or Readvance
                if (_capRepo.IsReAdvanceLAA(_capOffer))
                    ruleSVC.ExecuteRule(spc.DomainMessages, "ApplicationCap2AllowFurtherLendingSPV", _capOffer.Account.Key, (int)DisbursementTransactionTypes.CAP2ReAdvance, (int)OfferTypes.ReAdvance);
                else
                    ruleSVC.ExecuteRule(spc.DomainMessages, "ApplicationCap2AllowFurtherLendingSPV", _capOffer.Account.Key, (int)DisbursementTransactionTypes.CAP2ReAdvance, (int)OfferTypes.FurtherAdvance);

                _capRepo.SaveCapApplication(_capOffer);

                Dictionary<string, string> inputFields = new Dictionary<string, string>();
                inputFields.Add("CapOfferKey", _capOffer.Key.ToString());
                inputFields.Add("CapStatusKey", _capOffer.CapStatus.Key.ToString());
                inputFields.Add("Promotion", Convert.ToInt32(_capOffer.Promotion.Value).ToString());

                if (_view.Messages.ErrorMessages.Count == 0)
                {
                    //svc.CompleteActivity(_view.CurrentPrincipal, null, false);
                    svc.CompleteActivity(_view.CurrentPrincipal, inputFields, false);
                    svc.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
                }

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
        }

        #region Code moved to Cap Repo
        //private bool IsReAdvance()
        //{
        //    for (int i = 0; i < _capOffer.CapApplicationDetails.Count; i++)
        //    {
        //        if (_capOffer.CapApplicationDetails[i].CapStatus.Key == (int)CapStatuses.ReadvanceRequired)
        //        {
        //            double currentBalance = 0;
        //            double loanAgreementAmount = 0.0;
        //            double incInLAA = 0.0;

        //            IMortgageLoanAccount mortgageLoanAccount = _capOffer.Account as IMortgageLoanAccount;

        //            if (mortgageLoanAccount != null)
        //            {
        //                currentBalance += mortgageLoanAccount.SecuredMortgageLoan.CurrentBalance;

        //                foreach (IBond _bond in mortgageLoanAccount.SecuredMortgageLoan.Bonds)
        //                {
        //                    loanAgreementAmount += _bond.BondLoanAgreementAmount;
        //                }
        //            }

        //            IAccountVariFixLoan varifixLoanAccount = _capOffer.Account as IAccountVariFixLoan;
        //            if (varifixLoanAccount != null)
        //            {
        //                IMortgageLoan fixedmortgageLoan = varifixLoanAccount.FixedSecuredMortgageLoan;
        //                if (fixedmortgageLoan != null)
        //                    currentBalance += fixedmortgageLoan.CurrentBalance;
        //            }


        //            incInLAA = (Math.Round(Convert.ToDouble(_capOffer.CapApplicationDetails[i].Fee + currentBalance), 2) - Math.Round(loanAgreementAmount, 2));
        //            if (incInLAA > 0D)
        //            {
        //                return false;
        //            }
        //        }
        //    }
        //    return true;
        //}

        #endregion
    }
}