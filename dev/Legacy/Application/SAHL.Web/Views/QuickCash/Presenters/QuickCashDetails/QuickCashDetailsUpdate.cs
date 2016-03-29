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
using SAHL.Web.Views.QuickCash.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using System.Collections;
using System.Collections.Generic;
using SAHL.Common.Web.UI.Events;
using Castle.ActiveRecord;
using SAHL.Common.Exceptions;

namespace SAHL.Web.Views.QuickCash.Presenters.QuickCashDetails
{
    public class QuickCashDetailsUpdate : QuickCashDetailsBase
    {
        IReadOnlyEventList<ILegalEntity> mainApplicants;
        IApplicationInformationQuickCashDetail appInformationQCDetail;

        public QuickCashDetailsUpdate(IQuickCashDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!View.ShouldRunPage) return;

            _view.SetSubmitButtonText = "Update";
            _view.ShowApprovedPanel = true;

            RemovedDisbursedQuickCashDetailRecords();
            
            _view.QuickCashInformation = quickCashApplicationInformation;
            _view.BindApprovedPanel();
            _view.BindQuickCashPaymentsGrid(true);

            _view.BindQuickCashPaymentTypes(lookups.QuickCashPaymentTypes);

            _view.OnSubmitButtonClicked += new EventHandler(_view_OnSubmitButtonClicked);
            _view.OnCancelButtonClicked += new EventHandler(_view_OnCancelButtonClicked);

            OfferRoleTypes[] roleTypes = new OfferRoleTypes[1] { OfferRoleTypes.MainApplicant };
            mainApplicants = application.GetLegalEntitiesByRoleType(roleTypes);

            BindBankAccounts();
        }

        private void RemovedDisbursedQuickCashDetailRecords()
        {
            for (int i = 0; i < quickCashApplicationInformation.ApplicationInformationQuickCash.ApplicationInformationQuickCashDetails.Count; i++)
            {
                if (Convert.ToBoolean(quickCashApplicationInformation.ApplicationInformationQuickCash.ApplicationInformationQuickCashDetails[i].Disbursed))
                    quickCashApplicationInformation.ApplicationInformationQuickCash.ApplicationInformationQuickCashDetails.Remove(_view.Messages, quickCashApplicationInformation.ApplicationInformationQuickCash.ApplicationInformationQuickCashDetails[i]);
            }
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);

            appInformationQCDetail = null;

            if (appInfoQuickCash.ApplicationInformationQuickCashDetails != null && appInfoQuickCash.ApplicationInformationQuickCashDetails.Count > 0)
            {
                appInformationQCDetail = appInfoQuickCash.ApplicationInformationQuickCashDetails[0];
                
                if (!_view.IsPostBack)
                {
                    selectedPaymentType = appInformationQCDetail.QuickCashPaymentType.Key;
                    _view.BindBankAccountPanel(appInformationQCDetail);
                }
                else
                if (appInformationQCDetail != null && _view.GetSetSelectedGridItem != appInformationQCDetail.Key)
                {
                    GetSelectedQuickCashDetailsRecord();
                    selectedPaymentType = appInformationQCDetail.QuickCashPaymentType.Key;
                    _view.BindBankAccountPanel(appInformationQCDetail);
                }

            SelectedPaymentTypeChange();
            _view.ShowBankAccountPanel = true;
            _view.ShowButtons = true;
            }
        }

        private void GetSelectedQuickCashDetailsRecord()
        {
            for (int i = 0; i < appInfoQuickCash.ApplicationInformationQuickCashDetails.Count; i++)
            {
                if (_view.GetSetSelectedGridItem == appInfoQuickCash.ApplicationInformationQuickCashDetails[i].Key)
                {
                    appInformationQCDetail = appInfoQuickCash.ApplicationInformationQuickCashDetails[i];
                    break;
                }
            }
        }

        private void SelectedPaymentTypeChange()
        {
            IRateConfiguration rateConfig = null;

            switch (selectedPaymentType)
            {
                case (int)QuickCashPaymentTypes.UpfrontPayment:
                    {
                        rateConfig = GetControlNumericForLinkRate("QuickCashCashUpfrontLinkRate");
                        break;
                    }
                case (int)QuickCashPaymentTypes.RegularPayment:
                    {
                        rateConfig = GetControlNumericForLinkRate("QuickCashRegularPaymentLinkRate");
                        break;
                    }
                default: break;
            }

            PrivateCacheData.Remove("rateconfiguration");
            PrivateCacheData.Add("rateconfiguration", rateConfig);

            _view.SetRatesForPaymentType(rateConfig);
        }

        private void BindBankAccounts()
        {
            IDictionary<string, string> bankAccounts = new Dictionary<string, string>();

            foreach (ILegalEntity legalEntity in mainApplicants)
            {
                for (int a = 0; a < legalEntity.LegalEntityBankAccounts.Count; a++)
                {
                    bankAccounts.Add(legalEntity.LegalEntityBankAccounts[a].BankAccount.Key.ToString(), legalEntity.LegalEntityBankAccounts[a].BankAccount.GetDisplayName(BankAccountNameFormat.Full));
                }
            }

            _view.BindBankAccount(bankAccounts);

            if (application.ApplicationDebitOrders != null && application.ApplicationDebitOrders.Count > 0)
                _view.BankAccountKey = application.ApplicationDebitOrders[0].BankAccount.Key;
        }      

        protected void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("QuickCashDetails");
        }

        protected void _view_OnSubmitButtonClicked(object sender, EventArgs e)
        {
            GetSelectedQuickCashDetailsRecord();

            appInformationQCDetail = _view.GetUpdatedQuickDetailRecord(appInformationQCDetail);

            double totalThirdPartyPayments = 0;
            
            for (int i = 0; i < appInformationQCDetail.ApplicationExpenses.Count; i++)
            {
                if (appInformationQCDetail.ApplicationExpenses[i].ExpenseType.PaymentType.Key == (int)PaymentTypes.CashPaymentnointerest)
                    totalThirdPartyPayments += appInformationQCDetail.ApplicationExpenses[i].TotalOutstandingAmount;

                if (appInformationQCDetail.ApplicationExpenses[i].ExpenseType.PaymentType.Key == (int)PaymentTypes.QuickCashPayment)
                {
                    appInformationQCDetail.ApplicationExpenses[i].TotalOutstandingAmount = Convert.ToDouble(appInformationQCDetail.RequestedAmount) - totalThirdPartyPayments;
                    appInformationQCDetail.ApplicationExpenses[i].ApplicationDebtSettlements[0].SettlementAmount = Convert.ToDouble(appInformationQCDetail.ApplicationExpenses[i].TotalOutstandingAmount);
                }
            }
            // Force rules to fire as we are not changing values on the parent object
            appInfoQuickCash.ValidateEntity();

            if (!_view.IsValid)
                return;

            TransactionScope txn = new TransactionScope();
            try
            {
                qcRepo.SaveApplicationInformationQuickCash(appInfoQuickCash);
                txn.VoteCommit();
                _view.Navigator.Navigate(_view.ViewName);
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

      

    }
}
