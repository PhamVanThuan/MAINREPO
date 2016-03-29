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
using SAHL.Common;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.CacheData;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DomainMessages;
using SAHL.Common.Exceptions;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Cap.Interfaces;

namespace SAHL.Web.Views.Cap.Presenters
{
    /// <summary>
    ///
    /// </summary>
    public class CapOfferSalesBase : SAHLCommonBasePresenter<ICapOfferSales>
    {
        #region Protected Members

        protected InstanceNode _node;
        protected Dictionary<string, object> _x2Data;
        protected int _accountKey;
        protected int _capOfferKey;
        protected ICapRepository _capRepo;
        protected ICapApplication _capOffer;
        protected IAccount _account;

        IList<ICapNTUReason> _capNTUReasons;
        IList<ICapNTUReason> _capDeclineReasons;
        IList<ICapPaymentOption> _capPaymentOptions;

        protected double _LoanAgreementAmount;
        protected double _TotalBondAmount;
        // protected double _PTI;
        protected double _LoanCurrentBalance;
        protected double _TotalInstalment;
        // protected double _LTV;
        protected double _LinkRate;
        protected double _BalanceToCap;
        protected double _VariableLoanInstallment;
        protected double _InterestRate;
        protected double _HouseholdIncome;
        protected double _LatestValuation;
        protected int _ResetConfigKey;
        protected int _RemainingInstallments;
        protected string _LegalEntityName;

        #endregion Protected Members

        #region Constructor

        /// <summary>
        ///
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public CapOfferSalesBase(ICapOfferSales view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        #endregion Constructor

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage) return;

            _capRepo = RepositoryFactory.GetRepository<ICapRepository>();
        }

        /// <summary>
        ///
        /// </summary>
        protected void LoadCapOffer()
        {
            //TODO:
            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal, CBONodeSetType.X2) as InstanceNode;
            if (_node == null)
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);
            _x2Data = _node.X2Data as Dictionary<string, object>;
            _accountKey = Convert.ToInt32(_x2Data["AccountKey"]);
            _capOfferKey = Convert.ToInt32(_x2Data["CapOfferKey"]);

            _capOffer = _capRepo.GetCapOfferByKey(_capOfferKey);
            _account = _capOffer.Account;

            //TODO:
            _view.LegalEntityNameText = _x2Data["LegalEntityName"].ToString();
            _view.AccountNumberText = _x2Data["AccountKey"].ToString();
            IOrganisationStructureRepository OSR = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
            IADUser adUser = OSR.GetAdUserForAdUserName(_x2Data["CapBroker"].ToString());
            _view.SalesConsultantText = adUser.LegalEntity.GetLegalName(LegalNameFormat.Full);
            if (Convert.ToInt32(_x2Data["Promotion"]) == 1)
                _view.PromotionCheckBoxChecked = true;
            else
                _view.PromotionCheckBoxChecked = false;

            if ((_capOffer != null && _capOffer.CapApplicationDetails != null) &&
                (_capOffer.CapApplicationDetails.Count > 0 && _capOffer.CapApplicationDetails[0].CapNTUReason != null))
                _view.NTUReasonLabelText = _capOffer.CapApplicationDetails[0].CapNTUReason.Description;
            else
                _view.NTUReasonLabelText = "-";

            if (_capOffer.CAPPaymentOption != null)
            {
                if (_view.PaymentOptionDropDownVisible)
                    _view.CapPaymentOptionSelectedValue = _capOffer.CAPPaymentOption.Key;
                else
                    _view.PaymentOptionText = _capOffer.CAPPaymentOption.Description;
            }
            else
                _view.PaymentOptionText = "-";

            _view.NextResetDateText = _capOffer.CapTypeConfiguration.ResetDate.ToString(SAHL.Common.Constants.DateFormat);
            _view.CapEffectiveDateText = _capOffer.CapTypeConfiguration.CapEffectiveDate.ToString(SAHL.Common.Constants.DateFormat);
            _view.OfferStartDateText = _capOffer.CapTypeConfiguration.ApplicationStartDate.ToString(SAHL.Common.Constants.DateFormat);
            _view.OfferEndDateText = _capOffer.CapTypeConfiguration.ApplicationEndDate.ToString(SAHL.Common.Constants.DateFormat);
            _view.OfferStatusText = _capOffer.CapStatus.Description;
            _view.BalanceToCap = _capOffer.CurrentBalance.ToString(SAHL.Common.Constants.CurrencyFormat);

            //run the cap application rules
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
            IRuleService svc = ServiceFactory.GetService<IRuleService>();
            svc.ExecuteRule(spc.DomainMessages, "ApplicationCap2QualifyDebtCounselling", _capOffer);
        }

        /// <summary>
        ///
        /// </summary>
        protected void LoadCapOfferFromCBO()
        {
            CBOMenuNode cboNode = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;

            _capOfferKey = cboNode.GenericKey;
            _capOffer = _capRepo.GetCapOfferByKey(_capOfferKey);

            if (_capOffer != null)
            {
                _account = _capOffer.Account;

                _view.NextResetDateText = _capOffer.CapTypeConfiguration.ResetDate.ToString(SAHL.Common.Constants.DateFormat);
                _view.CapEffectiveDateText = _capOffer.CapTypeConfiguration.CapEffectiveDate.ToString(SAHL.Common.Constants.DateFormat);
                _view.OfferStartDateText = _capOffer.CapTypeConfiguration.ApplicationStartDate.ToString(SAHL.Common.Constants.DateFormat);
                _view.OfferEndDateText = _capOffer.CapTypeConfiguration.ApplicationEndDate.ToString(SAHL.Common.Constants.DateFormat);
                _view.OfferStatusText = _capOffer.CapStatus.Description;
                _view.BalanceToCap = _capOffer.CurrentBalance.ToString(SAHL.Common.Constants.CurrencyFormat);
                _view.NTUReasonLabelText = "-";

                if (_capOffer.CAPPaymentOption != null)
                {
                    if (_view.PaymentOptionDropDownVisible)
                        _view.CapPaymentOptionSelectedValue = _capOffer.CAPPaymentOption.Key;
                    else
                        _view.PaymentOptionText = _capOffer.CAPPaymentOption.Description;
                }
                else
                    _view.PaymentOptionText = "-";

                _LegalEntityName = _account.GetLegalName(LegalNameFormat.Full);
                _view.LegalEntityNameText = _LegalEntityName;
                _view.AccountNumberText = _capOffer.Account.Key.ToString();
                _view.SalesConsultantText = _capOffer.Broker.ADUser != null ? _capOffer.Broker.ADUser.LegalEntity.GetLegalName(LegalNameFormat.Full) : "-";
                if (_capOffer.Promotion.HasValue && _capOffer.Promotion.Value == true)
                    _view.PromotionCheckBoxChecked = true;
                else
                    _view.PromotionCheckBoxChecked = false;
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected void CreateCapOffer(bool recalculatingOffer)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
            IDomainMessageCollection dmc = spc.DomainMessages;
            ICapApplication capOffer = null;

            int accountKey = -1;
            if (recalculatingOffer)
            {
                _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal, CBONodeSetType.X2) as InstanceNode;
                if (_node == null)
                    throw new NullReferenceException(StaticMessages.NullCurrentCBONode);
                _x2Data = _node.X2Data as Dictionary<string, object>;
                accountKey = Convert.ToInt32(_x2Data["AccountKey"]);
                capOffer = GlobalCacheData["RecalculatedCapOffer"] as ICapApplication;
                capOffer = _capRepo.GetCapOfferByKey(capOffer.Key);
            }
            else
            {
                if (GlobalCacheData.ContainsKey("CapAccountKey"))
                    accountKey = Convert.ToInt32(GlobalCacheData["CapAccountKey"]);

                capOffer = _capRepo.CreateCapApplication();
            }

            if (accountKey != -1)
            {
                IAccountRepository accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
                _account = accRepo.GetAccountByKey(accountKey);
                ILookupRepository lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();
                ISecurityRepository secRepo = RepositoryFactory.GetRepository<ISecurityRepository>();
                IADUser adUser = secRepo.GetADUserByPrincipal(_view.CurrentPrincipal);
                IRuleService svc = ServiceFactory.GetService<IRuleService>();

                BindInstalmentValues("Real-time Instalment");

                ICapTypeConfiguration currentConfig = _capRepo.GetCurrentCapTypeConfigByResetConfigKey(_ResetConfigKey);
                if (currentConfig != null)
                {
                    capOffer.Account = _account;
                    capOffer.RemainingInstallments = _RemainingInstallments;
                    capOffer.CurrentBalance = _BalanceToCap;
                    capOffer.CurrentInstallment = _VariableLoanInstallment;
                    capOffer.LinkRate = _LinkRate;

                    if (recalculatingOffer)
                        capOffer.CapStatus = lookupRepo.CapStatuses.ObjectDictionary[Convert.ToInt32(CapStatuses.Recalculated).ToString()];
                    else
                        capOffer.CapStatus = lookupRepo.CapStatuses.ObjectDictionary[Convert.ToInt32(CapStatuses.Open).ToString()];

                    capOffer.ApplicationDate = DateTime.Now;
                    capOffer.Promotion = false;
                    capOffer.CapTypeConfiguration = currentConfig;
                    capOffer.Broker = _capRepo.GetBrokerByADUserKey(adUser.Key);
                    capOffer.ChangeDate = DateTime.Now;
                    capOffer.UserID = adUser.ADUserName;

                    for (int i = 0; i < currentConfig.CapTypeConfigurationDetails.Count; i++)
                    {
                        ICapApplicationDetail newCapApplicationDetail = null;
                        bool addToCap = false;

                        if (recalculatingOffer)
                        {
                            foreach (ICapApplicationDetail _capAppDet in capOffer.CapApplicationDetails)
                            {
                                if (_capAppDet.CapTypeConfigurationDetail.Key == currentConfig.CapTypeConfigurationDetails[i].Key)
                                {
                                    newCapApplicationDetail = _capAppDet;
                                    break;
                                }
                            }

                            // if the record has not been found, it should be recreated
                            if (newCapApplicationDetail == null)
                            {
                                newCapApplicationDetail = _capRepo.CreateCapApplicationDetail();
                                addToCap = true;
                            }
                        }
                        else
                        {
                            newCapApplicationDetail = _capRepo.CreateCapApplicationDetail();
                        }

                        newCapApplicationDetail.CapApplication = capOffer;
                        newCapApplicationDetail.CapTypeConfigurationDetail = currentConfig.CapTypeConfigurationDetails[i];
                        newCapApplicationDetail.EffectiveRate = _LinkRate + currentConfig.CapTypeConfigurationDetails[i].Rate;

                        double CapFee = _BalanceToCap * currentConfig.CapTypeConfigurationDetails[i].Premium;
                        double LV = _BalanceToCap + CapFee;
                        double IntRate = ((_InterestRate + _LinkRate) / 12);
                        //double IntRate = (_InterestRate / 12);
                        double CapPayment = LV * (Math.Pow(1 + IntRate, _RemainingInstallments)) * (IntRate / (Math.Pow(1 + IntRate, _RemainingInstallments) - 1));

                        newCapApplicationDetail.Payment = CapPayment;
                        newCapApplicationDetail.Fee = CapFee;

                        if (recalculatingOffer)
                            newCapApplicationDetail.CapStatus = lookupRepo.CapStatuses.ObjectDictionary[Convert.ToInt32(CapStatuses.Recalculated).ToString()];
                        else
                            newCapApplicationDetail.CapStatus = lookupRepo.CapStatuses.ObjectDictionary[Convert.ToInt32(CapStatuses.Open).ToString()];

                        newCapApplicationDetail.UserID = adUser.ADUserName;
                        newCapApplicationDetail.ChangeDate = DateTime.Now;
                        int addCAD = svc.ExecuteRule(dmc, "ApplicationCAP2QualifyCAPOfferDetail", newCapApplicationDetail);

                        if (!recalculatingOffer && addCAD == 0)
                        {
                            capOffer.CapApplicationDetails.Add(_view.Messages, newCapApplicationDetail);
                        }
                        else if (recalculatingOffer && addToCap && addCAD == 0)
                        {
                            capOffer.CapApplicationDetails.Add(_view.Messages, newCapApplicationDetail);
                        }
                        else if (addCAD == 1)
                        {
                            for (int cadI = 0; cadI < capOffer.CapApplicationDetails.Count; cadI++)
                            {
                                if (capOffer.CapApplicationDetails[cadI].Key == newCapApplicationDetail.Key)
                                {
                                    capOffer.CapApplicationDetails.RemoveAt(_view.Messages, cadI);
                                    break;
                                }
                            }
                        }
                    }

                    _capOffer = capOffer;
                }

                if (_capOffer != null)
                {
                    _view.NextResetDateText = _capOffer.CapTypeConfiguration.ResetDate.ToString(SAHL.Common.Constants.DateFormat);
                    _view.CapEffectiveDateText = _capOffer.CapTypeConfiguration.CapEffectiveDate.ToString(SAHL.Common.Constants.DateFormat);
                    _view.OfferStartDateText = _capOffer.CapTypeConfiguration.ApplicationStartDate.ToString(SAHL.Common.Constants.DateFormat);
                    _view.OfferEndDateText = _capOffer.CapTypeConfiguration.ApplicationEndDate.ToString(SAHL.Common.Constants.DateFormat);
                    _view.OfferStatusText = _capOffer.CapStatus.Description;
                    _view.BalanceToCap = _capOffer.CurrentBalance.ToString(SAHL.Common.Constants.CurrencyFormat);
                    _view.NTUReasonLabelText = "-";
                    _view.PaymentOptionText = "-";

                    _LegalEntityName = _account.GetLegalName(LegalNameFormat.Full);
                    _view.LegalEntityNameText = _LegalEntityName;

                    _view.AccountNumberText = _capOffer.Account.Key.ToString();

                    if (_capOffer.Broker != null)
                        _view.SalesConsultantText = _capOffer.Broker.ADUser != null ? _capOffer.Broker.ADUser.LegalEntity.GetLegalName(LegalNameFormat.Full) : "-";
                    else
                        _view.SalesConsultantText = "-";

                    if (_capOffer.Promotion.HasValue && _capOffer.Promotion.Value == true)
                        _view.PromotionCheckBoxChecked = true;
                    else
                        _view.PromotionCheckBoxChecked = false;
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="installmentHeaderValue"></param>
        protected void BindInstalmentValues(string installmentHeaderValue)
        {
            IAccount account = _account;
            if (account != null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("ServiceType", typeof(String)));
                dt.Columns.Add(new DataColumn("LoanInstallment", typeof(double)));
                dt.Columns.Add(new DataColumn("Premiums", typeof(double)));
                dt.Columns.Add(new DataColumn("ShortInstallments", typeof(double)));
                dt.Columns.Add(new DataColumn("TotalInstallments", typeof(double)));

                IMortgageLoanAccount mortgageLoanAccount = account as IMortgageLoanAccount;
                double shortTermLoanInstalment = 0;
                double variableInstallmentVal = 0;
                double premium = 0;
                double fixedPayment = 0;
                double currentBalance = 0;
                double latestValuation = 0;
                double accruedInterestMTD = 0D;
                double committedLoanValue = 0D;

                if (mortgageLoanAccount != null)
                {
                    accruedInterestMTD = (mortgageLoanAccount.SecuredMortgageLoan.AccruedInterestMTD.HasValue ? mortgageLoanAccount.SecuredMortgageLoan.AccruedInterestMTD.Value : 0D);
                    variableInstallmentVal = mortgageLoanAccount.SecuredMortgageLoan.Payment;
                    _VariableLoanInstallment = variableInstallmentVal;
                    premium = 0;

                    premium = account.InstallmentSummary.TotalPremium;

                    committedLoanValue = account.CommittedLoanValue;

                    /*if (mortgageLoanAccount.HOCAccount != null)
                        premium = mortgageLoanAccount.HOCAccount.FixedPayment;
                    if (mortgageLoanAccount.LifePolicyAccount != null)
                        premium = mortgageLoanAccount.LifePolicyAccount.FixedPayment;
                    */

                    currentBalance += mortgageLoanAccount.SecuredMortgageLoan.CurrentBalance;
                    _BalanceToCap = currentBalance;

                    #region New Installment Calc

                    IMortgageLoan _ml = mortgageLoanAccount.SecuredMortgageLoan;
                    double _capBalance = 0D;
                    double _marketRate = _ml.ActiveMarketRate;
                    //double _lifeBalance = mortgageLoanAccount.CapitalisedLife;

                    if (_marketRate <= 0D)
                        _marketRate = _ml.ActiveMarketRate;

                    int _interestPeriods = 12;

                    variableInstallmentVal = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateNewProductInstallmentAtEndOfPeriod(_ml.InterestRate, _marketRate, _ml.ActiveMarketRate, _ml, _capBalance, Convert.ToInt32(_ml.RemainingInstallments), _interestPeriods);
                    //CAP or CAP1 - is no longer active and something we dont need to care about
                    //if (_ml.FinancialServiceType.Key == 1) // Variable
                    //variableInstallmentVal = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateNewProductInstallmentAtEndOfPeriod(_ml.InterestRate, _marketRate, _ml.ActiveMarketRate, _ml as IMortgageLoanAccount, _capBalance, Convert.ToInt32(_ml.RemainingInstallments), _interestPeriods);
                    //else
                    //variableInstallmentVal = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateNewProductInstallmentAtEndOfPeriod(_ml.InterestRate, _marketRate, _ml.ActiveMarketRate, _ml.CurrentBalance, _capBalance, Convert.ToInt32(_ml.RemainingInstallments), _interestPeriods);

                    #endregion New Installment Calc

                    shortTermLoanInstalment = account.InstallmentSummary.TotalShortTermLoanInstallment;
                    /*
                    for (int i = 0; i < mortgageLoanAccount.UnsecuredMortgageLoans.Count; i++)
                    {
                        shortTermLoanInstalment += mortgageLoanAccount.UnsecuredMortgageLoans[i].Payment;
                    }
                     */

                    latestValuation = mortgageLoanAccount.SecuredMortgageLoan.GetActiveValuationAmount();
                    _LatestValuation = latestValuation;

                    // The Total of the Bonds should be displayed
                    _TotalBondAmount = 0.0;
                    _LoanAgreementAmount = 0.0;
                    foreach (IBond _bond in mortgageLoanAccount.SecuredMortgageLoan.Bonds)
                    {
                        _TotalBondAmount += _bond.BondRegistrationAmount;
                        _LoanAgreementAmount += _bond.BondLoanAgreementAmount;
                    }

                    double linkrate = mortgageLoanAccount.SecuredMortgageLoan.RateConfiguration.Margin.Value;
                    linkrate += mortgageLoanAccount.SecuredMortgageLoan.RateAdjustment;
                    _LinkRate = linkrate;

                    _RemainingInstallments = mortgageLoanAccount.SecuredMortgageLoan.RemainingInstallments;
                    _ResetConfigKey = mortgageLoanAccount.SecuredMortgageLoan.ResetConfiguration.Key;
                    _InterestRate = mortgageLoanAccount.SecuredMortgageLoan.ActiveMarketRate;
                }

                IAccountVariFixLoan varifixLoanAccount = account as IAccountVariFixLoan;
                if (varifixLoanAccount != null)
                {
                    IMortgageLoan fixedmortgageLoan = varifixLoanAccount.FixedSecuredMortgageLoan;
                    if (fixedmortgageLoan != null)
                    {
                        fixedPayment = fixedmortgageLoan.Payment;
                        currentBalance += fixedmortgageLoan.CurrentBalance;
                    }
                }

                dt.Rows.Add(
                CreateInstallmentDataRow(
                    "Variable",
                    variableInstallmentVal,
                    premium,
                    shortTermLoanInstalment,
                    variableInstallmentVal + premium + shortTermLoanInstalment,
                    dt));

                if (fixedPayment > 0.0d)
                {
                    dt.Rows.Add(
                        CreateInstallmentDataRow(
                            "Fixed",
                            fixedPayment,
                            -1,
                            -1,
                            fixedPayment,
                            dt));
                }

                dt.Rows.Add(
                    CreateInstallmentDataRow(
                        "Total",
                        fixedPayment + variableInstallmentVal,
                        premium,
                        shortTermLoanInstalment,
                        fixedPayment + variableInstallmentVal + premium + shortTermLoanInstalment,
                        dt));

                double householdIncome = account.GetHouseholdIncome();
                _HouseholdIncome = householdIncome;
                //if (householdIncome != 0)
                //    _PTI = (fixedPayment + variableInstallmentVal + premium + shortTermLoanInstalment) / householdIncome;

                _LoanCurrentBalance = currentBalance;
                _TotalInstalment = (fixedPayment + variableInstallmentVal + premium + shortTermLoanInstalment);

                //if (latestValuation != 0)
                //    _LTV = currentBalance / latestValuation;

                _view.LoanAgreementAmount = _LoanAgreementAmount;
                _view.TotalBondAmount = _TotalBondAmount;
                // _view.PTI = _PTI;
                //_view.LoanCurrentBalance = _LoanCurrentBalance;
                _view.VariableInstallment = variableInstallmentVal;
                //_view.LTV = _LTV;
                _view.LinkRate = _LinkRate;
                _view.HouseholdIncome = _HouseholdIncome;
                _view.LatestValuation = _LatestValuation;
                _view.BindInstalmentGrid(dt, installmentHeaderValue);
                _view.AccruedInterestMTD = accruedInterestMTD;

                _view.CommittedLoanValue = committedLoanValue;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="loanInstallment"></param>
        /// <param name="premiums"></param>
        /// <param name="shortInstallments"></param>
        /// <param name="totalInstallments"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private static DataRow CreateInstallmentDataRow(string serviceType,
                                                double loanInstallment,
                                                double premiums,
                                                double shortInstallments,
                                                double totalInstallments,
                                                DataTable data)
        {
            DataRow dr = data.NewRow();
            dr["ServiceType"] = serviceType;
            dr["LoanInstallment"] = loanInstallment;
            if (premiums == -1)
                dr["Premiums"] = DBNull.Value;
            else
                dr["Premiums"] = premiums;
            if (shortInstallments == -1)
                dr["ShortInstallments"] = DBNull.Value;
            else
                dr["ShortInstallments"] = shortInstallments;
            dr["TotalInstallments"] = totalInstallments;
            return dr;
        }

        /// <summary>
        ///
        /// </summary>
        protected void BindCapNTUReasons()
        {
            _capNTUReasons = _capRepo.GetCapNTUReasons();
            _view.BindReasonDropdown(_capNTUReasons);
        }

        /// <summary>
        ///
        /// </summary>
        protected void BindCapDeclineReasons()
        {
            _capDeclineReasons = _capRepo.GetCapDeclineReasons();
            _view.BindReasonDropdown(_capDeclineReasons);
        }

        protected void BindCapPaymentOptions()
        {
            _capPaymentOptions = _capRepo.GetCapPaymentOptions();
            _view.BindPaymentOptionDropDown(_capPaymentOptions);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="reasonKey"></param>
        /// <returns></returns>
        protected ICapNTUReason GetSelectedDeclineReason(int reasonKey)
        {
            for (int i = 0; i < _capDeclineReasons.Count; i++)
            {
                if (_capDeclineReasons[i].Key == reasonKey)
                    return _capDeclineReasons[i];
            }
            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="paymentOptionKey"></param>
        /// <returns></returns>
        protected ICapPaymentOption GetSelectedCapPaymentOption(int paymentOptionKey)
        {
            for (int i = 0; i < _capPaymentOptions.Count; i++)
            {
                if (_capPaymentOptions[i].Key == paymentOptionKey)
                    return _capPaymentOptions[i];
            }
            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="reasonKey"></param>
        /// <returns></returns>
        protected ICapNTUReason GetSelectedNTUReason(int reasonKey)
        {
            for (int i = 0; i < _capNTUReasons.Count; i++)
            {
                if (_capNTUReasons[i].Key == reasonKey)
                    return _capNTUReasons[i];
            }
            return null;
        }

        /// <summary>
        ///
        /// </summary>
        protected void ValidateOnUpdate()
        {
            string errorMessage = "Please select a Payment Option.";
            if (_view.CapPaymentOptionSelectedValue == -1)
            {
                _view.Messages.Add(new DomainMessage(errorMessage, errorMessage));
                throw new DomainValidationException();
            }
        }
    }
}

//COMPLETED   Display = 1
//COMPLETED   Summary
//COMPLETED   Accept
//COMPLETED   Reverse
//COMPLETED   Recalculate   *** creating an offer
//COMPLETED   Add           *** creating an offer
//COMPLETED   Update, NTU   *** Renamed to NTU screen
//COMPLETED   PrintLetter   *** need to revisit when the rsviewer stuff is completed
//COMPLETED   Decline       *** similar to the NTU screen
//COMPLETED   ReadvanceDone
//COMPLETED   Rework
//REMOVED     CashPayment   *** No longer using the cash payment route in the workflow
//REMOVED     CashPaymentVerify *** No longer using the cash payment route in the workflow
//COMPLETED   PrepForCredit
//COMPLETED   CreditApproval
//COMPLETED   GrantOffer
//COMPLETED   ReadyForReadvance
//COMPLETED   FormsSent
//COMPLETED   LASent
//COMPLETED   ConfirmCancel
//COMPLETED   Promotion