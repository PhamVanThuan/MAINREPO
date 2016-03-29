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
    public class QuickCashDetailsAdd : QuickCashDetailsBase
    {
        IReadOnlyEventList<ILegalEntity> mainApplicants;
 
        public QuickCashDetailsAdd(IQuickCashDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!View.ShouldRunPage) return;

            _view.ShowApprovedPanel = true;
            _view.QuickCashInformation = quickCashApplicationInformation;
            _view.BindApprovedPanel();
            _view.BindQuickCashPaymentsGrid(false);
            _view.ShowBankAccountPanel = true;
            _view.ShowButtons = true;

            application = appInfoQuickCash.ApplicationInformation.Application;

            _view.BindQuickCashPaymentTypes(lookups.QuickCashPaymentTypes);
           
            _view.OnSubmitButtonClicked+=new EventHandler(_view_OnSubmitButtonClicked);
            _view.OnCancelButtonClicked+=new EventHandler(_view_OnCancelButtonClicked);

            OfferRoleTypes[] roleTypes = new OfferRoleTypes[1] { OfferRoleTypes.MainApplicant };
            mainApplicants = application.GetLegalEntitiesByRoleType(roleTypes);

            BindBankAccounts();
            
            _view.onPaymentTypeSelectedIndexChanged+=new KeyChangedEventHandler(_view_onPaymentTypeSelectedIndexChanged);
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
    
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);

            SelectedPaymentType();
        }

        private void SelectedPaymentType()
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

        protected void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("QuickCashDetails");
        }

        protected void _view_OnSubmitButtonClicked(object sender, EventArgs e)
        {
            // Create Application Information QuickCashdetail Record
            IApplicationInformationQuickCashDetail qcDetail = qcRepo.CreateEmptyApplicationInformationQuickCashDetail();
            qcDetail = _view.GetUpdatedQuickDetailRecord(qcDetail);

            IRateConfiguration rateConfiguration = GetRateConfigurationFromCache();
            
            if (rateConfiguration != null)
            {
                qcDetail.RateConfiguration = rateConfiguration;
                qcDetail.InterestRate = (((rateConfiguration.Margin.Value * 100) + rateConfiguration.MarketRate.Value*100))/100;
            }
            qcDetail.OfferInformationQuickCash = appInfoQuickCash;
            if (qcDetail.ValidateEntity())
            {
                // Create ApplicationExpenseRecord
                ILegalEntityBankAccount LegalEntityBankAcct = GetSelectedLegalEntityBankAccount(_view.BankAccountKey);
                IApplicationExpense appExpense = appRepo.GetEmptyApplicationExpense();
                appExpense.Application = application;
                appExpense.LegalEntity = LegalEntityBankAcct.LegalEntity;
                appExpense.ExpenseAccountName = LegalEntityBankAcct.BankAccount.AccountName;
                appExpense.ExpenseAccountNumber = LegalEntityBankAcct.BankAccount.AccountNumber;
                appExpense.ExpenseReference = application.ReservedAccount.Key.ToString() + " QuickCash";
                appExpense.TotalOutstandingAmount = Convert.ToDouble(qcDetail.RequestedAmount);
                appExpense.MonthlyPayment = 0;
                appExpense.ToBeSettled = true;
                appExpense.ExpenseType = lookups.ExpenseTypes.ObjectDictionary[((int)ExpenseTypes.QuickCash).ToString()];

                qcDetail.ApplicationExpenses.Add(_view.Messages, appExpense);

                // Create the ApplicationDebtSettlement Record
                IApplicationDebtSettlement appDebtSettlement = appRepo.GetEmptyApplicationDebtSettlement();
                appDebtSettlement.BankAccount = LegalEntityBankAcct.BankAccount;
                appDebtSettlement.SettlementAmount = Convert.ToDouble(qcDetail.RequestedAmount);
                appDebtSettlement.DisbursementType = lookups.DisbursementTypes.ObjectDictionary[((int)DisbursementTypes.QuickCashdisbursement).ToString()];
                appDebtSettlement.OfferExpense = appExpense;

                appExpense.ApplicationDebtSettlements.Add(_view.Messages, appDebtSettlement);

                appInfoQuickCash.ApplicationInformationQuickCashDetails.Add(_view.Messages, qcDetail);

                // Force rules to fire as we are not changing values on the parent object
                appInfoQuickCash.ValidateEntity();

                if (!_view.IsValid)
                    return;

                TransactionScope txn = new TransactionScope();

                try
                {
                    qcRepo.SaveApplicationInformationQuickCash(appInfoQuickCash);
                    appRepo.SaveApplicationDebtSettlement(appDebtSettlement);
                    appRepo.SaveApplicationExpense(appExpense);
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
            }
            if (_view.IsValid)
                _view.Navigator.Navigate(_view.ViewName);
        }

        private IRateConfiguration GetRateConfigurationFromCache()
        {
            IRateConfiguration rateConfiguration = null;
            if (PrivateCacheData.ContainsKey("rateconfiguration"))
                rateConfiguration = PrivateCacheData["rateconfiguration"] as IRateConfiguration;
            return rateConfiguration;
        }

        private ILegalEntityBankAccount GetSelectedLegalEntityBankAccount(int bankAccountKey)
        {
           foreach (ILegalEntity legalEntity in mainApplicants)
            {
                for (int a = 0; a < legalEntity.LegalEntityBankAccounts.Count; a++)
                {
                    if (legalEntity.LegalEntityBankAccounts[a].BankAccount.Key == bankAccountKey)
                    {
                        return legalEntity.LegalEntityBankAccounts[a];
                    }
                }
            }

            return null;
        }
    }
}
