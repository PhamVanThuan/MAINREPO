using Castle.ActiveRecord.Queries;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.CacheData;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.DomainMessages;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Interfaces;
using SAHL.Common.Security;
using SAHL.Common.Service.Interfaces;
using System;
using System.Data;
using System.Linq;

namespace SAHL.Common.BusinessModel.Helpers
{
    public class ApplicationCalculateMortgageLoanHelper : AbstractRepositoryBase
    {
        private IDomainMessageCollection _dmc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent()).DomainMessages;

        private IApplicationMortgageLoan _appML;
        private OfferTypes _appType;
        private MortgageLoanPurposes _mlp;
        private IAccountRepository _accRepo;
        private ILookupRepository _lookupRepo;
        private IControlRepository _ctrlRepo;
        private IApplicationRepository _appRepo;
        private ICreditMatrixRepository _cmRepo;

        public ApplicationCalculateMortgageLoanHelper(IApplicationMortgageLoan appML, OfferTypes appType)
        {
            _appML = appML;
            _appType = appType; // (ApplicationTypes)Enum.ToObject(typeof(ApplicationTypes), Convert.ToInt16(_appML.ApplicationType.Key));

            switch (_appType)
            {
                case OfferTypes.FurtherAdvance:
                    _mlp = MortgageLoanPurposes.Switchloan;
                    break;

                case OfferTypes.FurtherLoan:
                    _mlp = MortgageLoanPurposes.Switchloan;
                    break;

                case OfferTypes.Life:
                    throw new NotSupportedException("This is not a mortgage loan.");
                case OfferTypes.NewPurchaseLoan:
                    _mlp = MortgageLoanPurposes.Newpurchase;
                    break;

                case OfferTypes.ReAdvance:
                    _mlp = MortgageLoanPurposes.Switchloan;
                    break;

                case OfferTypes.RefinanceLoan:
                    _mlp = MortgageLoanPurposes.Refinance;
                    break;

                case OfferTypes.SwitchLoan:
                    _mlp = MortgageLoanPurposes.Switchloan;
                    break;

                default:
                    throw new NotSupportedException("This application type is not supported.");
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public void CalculateApplicationDetail(bool IsBondExceptionAction, bool keepMarketRate)
        {
            #region outputs

            double initiationFee = 0;
            double? initiationFeeDiscount = null;
            double registrationFee = 0;
            double cancelFee = 0;
            double overrideCancelFee = 0;
            double interimInterest = 0;
            double bondToRegister = 0;

            #endregion outputs

            #region inputs

            double loanAgreeAmount = 0;
            double newLoanAgreeAmount = 0;
            double applicationBondToRegister = 0;
            double feesTotal = 0;
            double cashOut = 0;

            int productKey = 0;
            bool interestOnly = false;
            bool noInitFee = false;
            bool capitaliseFees = false;
            bool capitaliseInitiationFee = false;
            int newSPVKey = 0;

            #endregion inputs

            IApplicationProduct cp = _appML.CurrentProduct;
            ISupportsVariableLoanApplicationInformation suppvlai = cp as ISupportsVariableLoanApplicationInformation;

            //If the offer is not open (statuskey != 1 or
            //application information has been accepted then no calcs allowed
            IApplicationInformation ai = _appML.GetLatestApplicationInformation();
            if (!_appML.IsOpen
                || ai == null
                || ai.ApplicationInformationType.Key == (int)OfferInformationTypes.AcceptedOffer)
                return;

            //If it is further lending then recalc LTV only (for adding valuations to get correct spv)
            if (_appML.ApplicationType != null
                && (_appML.ApplicationType.Key == (int)OfferTypes.ReAdvance
                || _appML.ApplicationType.Key == (int)OfferTypes.FurtherAdvance
                || _appML.ApplicationType.Key == (int)OfferTypes.FurtherLoan))
            {
                FLAppCalculateHelper flH = new FLAppCalculateHelper(_appML.Account);
                flH.CalculateFurtherLending(false);

                return;
            }

            var applicationStartDate = _appML.ApplicationStartDate.HasValue ? _appML.ApplicationStartDate.Value : DateTime.Now;

            //A single value could have been persisted to the object, so we need to recalc the LoanAgreeAmount
            switch (_appType)
            {
                case OfferTypes.NewPurchaseLoan:
                    IApplicationMortgageLoanNewPurchase np = cp.Application as IApplicationMortgageLoanNewPurchase;
                    newLoanAgreeAmount = (np.PurchasePrice.HasValue ? np.PurchasePrice.Value : 0) - (np.CashDeposit.HasValue ? np.CashDeposit.Value : 0);
                    break;

                case OfferTypes.RefinanceLoan:
                    IApplicationMortgageLoanRefinance rf = cp.Application as IApplicationMortgageLoanRefinance;
                    newLoanAgreeAmount = rf.CashOut.HasValue ? rf.CashOut.Value : 0;
                    break;

                case OfferTypes.SwitchLoan:
                    IApplicationMortgageLoanSwitch sw = cp.Application as IApplicationMortgageLoanSwitch;

                    newLoanAgreeAmount = (sw.ExistingLoan.HasValue ? sw.ExistingLoan.Value : 0) + (sw.CashOut.HasValue ? sw.CashOut.Value : 0);// +(sw.InterimInterest.HasValue ? sw.InterimInterest.Value : 0);
                    cashOut = sw.CashOut.HasValue ? sw.CashOut.Value : 0;
                    foreach (IApplicationExpense ae in sw.ApplicationExpenses)
                    {
                        if (ae.ExpenseType.Key == (int)ExpenseTypes.CancellationFee && ae.OverRidden == true)
                        {
                            overrideCancelFee = ae.TotalOutstandingAmount;
                        }

                        //need to ensure that the Expense amount is the same as the Existing Loan Agreement Amount
                        if (ae.ExpenseType.Key == (int)ExpenseTypes.Existingmortgageamount)
                        {
                            double existingLoan = (sw.ExistingLoan.HasValue ? sw.ExistingLoan.Value : 0);
                            if (existingLoan != ae.TotalOutstandingAmount)
                                ae.TotalOutstandingAmount = existingLoan;
                        }
                    }
                    break;

                default:
                    break;
            }

            // New originated Staff loans do not pay an initiation fee
            if (_appType != OfferTypes.FurtherLoan)
            {
                if (cp.Application.HasAttribute(OfferAttributeTypes.StaffHomeLoan))
                    noInitFee = true;
            }

            //Attribute checks
            //Check for Interest Only
            if (cp.Application.HasFinancialAdjustment(FinancialAdjustmentTypeSources.InterestOnly))
                interestOnly = true;

            //Check for Capitalise Fees / Capitalise Initiation Fee
            if (cp.Application.HasAttribute(OfferAttributeTypes.CapitalizeFees))
                capitaliseFees = true;
            else if (cp.Application.HasAttribute(OfferAttributeTypes.CapitaliseInitiationFee))
                capitaliseInitiationFee = true;

            //Check for Returning Client
            bool isDiscountedInitiationFee = AppRepo.DetermineApplicationAttributeTypes(_appML).Exists(x => x.ApplicationAttributeTypeKey == (int)OfferAttributeTypes.DiscountedInitiationFeeReturningClient && x.ToBeRemoved == false);

            double householdIncome = 0;
            int employmentType = (int)EmploymentTypes.Salaried;
            double propertyValue = 0;
            GetInformationForFeesCalc(suppvlai, out householdIncome, out employmentType, out propertyValue);

            if (cp.ProductType == SAHL.Common.Globals.Products.VariableLoan)
            {
                IApplicationProductVariableLoan prod = cp as IApplicationProductVariableLoan;
                if (newLoanAgreeAmount != 0)
                    prod.LoanAmountNoFees = newLoanAgreeAmount;

                loanAgreeAmount = prod.LoanAmountNoFees.HasValue ? prod.LoanAmountNoFees.Value : 0;
                applicationBondToRegister = prod.VariableLoanInformation.BondToRegister.HasValue ? prod.VariableLoanInformation.BondToRegister.Value : 0;

                CalculateOriginationFees(loanAgreeAmount, applicationBondToRegister, _appType, cashOut, overrideCancelFee, capitaliseFees, false, IsBondExceptionAction, isDiscountedInitiationFee, out initiationFeeDiscount, out initiationFee, out registrationFee, out cancelFee, out interimInterest, out bondToRegister, _appML.HasAttribute(OfferAttributeTypes.QuickPayLoan), householdIncome, employmentType, propertyValue, 0, cp.Application.HasAttribute(OfferAttributeTypes.StaffHomeLoan), applicationStartDate, capitaliseInitiationFee, _appML.HasAttribute(OfferAttributeTypes.GovernmentEmployeePensionFund));

                if (noInitFee)
                    initiationFee = 0;

                SaveFees(initiationFee, registrationFee, cancelFee, (overrideCancelFee != 0), interimInterest);

                productKey = prod.Application.GetLatestApplicationInformation().Product.Key;

                feesTotal = initiationFee + registrationFee + cancelFee;

                CalculateCommonApplicationDetail(prod.VariableLoanInformation, productKey, interestOnly, loanAgreeAmount, feesTotal, interimInterest, bondToRegister, keepMarketRate, initiationFee);

                newSPVKey = prod.VariableLoanInformation.SPV.Key;
            }

            if (cp.ProductType == SAHL.Common.Globals.Products.VariFixLoan)
            {
                IApplicationProductVariFixLoan prod = cp as IApplicationProductVariFixLoan;
                if (newLoanAgreeAmount != 0)
                    prod.LoanAmountNoFees = newLoanAgreeAmount;

                loanAgreeAmount = prod.LoanAmountNoFees.HasValue ? prod.LoanAmountNoFees.Value : 0;
                applicationBondToRegister = prod.VariableLoanInformation.BondToRegister.HasValue ? prod.VariableLoanInformation.BondToRegister.Value : 0;

                CalculateOriginationFees(loanAgreeAmount, applicationBondToRegister, _appType, cashOut, overrideCancelFee, capitaliseFees, false, IsBondExceptionAction, isDiscountedInitiationFee, out initiationFeeDiscount, out initiationFee, out registrationFee, out cancelFee, out interimInterest, out bondToRegister, _appML.HasAttribute(OfferAttributeTypes.QuickPayLoan), householdIncome, employmentType, propertyValue, 0, cp.Application.HasAttribute(OfferAttributeTypes.StaffHomeLoan), applicationStartDate, capitaliseInitiationFee, _appML.HasAttribute(OfferAttributeTypes.GovernmentEmployeePensionFund));

                if (noInitFee)
                    initiationFee = 0;

                SaveFees(initiationFee, registrationFee, cancelFee, (overrideCancelFee != 0), interimInterest);

                productKey = prod.Application.GetLatestApplicationInformation().Product.Key;

                feesTotal = initiationFee + registrationFee + cancelFee;

                CalculateCommonApplicationDetail(prod.VariableLoanInformation, productKey, interestOnly, loanAgreeAmount, feesTotal, interimInterest, bondToRegister, keepMarketRate, initiationFee);

                newSPVKey = prod.VariableLoanInformation.SPV.Key;
            }

            if (cp.ProductType == SAHL.Common.Globals.Products.SuperLo)
            {
                IApplicationProductSuperLoLoan prod = cp as IApplicationProductSuperLoLoan;
                if (newLoanAgreeAmount != 0)
                    prod.LoanAmountNoFees = newLoanAgreeAmount;

                loanAgreeAmount = prod.LoanAmountNoFees.HasValue ? prod.LoanAmountNoFees.Value : 0;
                applicationBondToRegister = prod.VariableLoanInformation.BondToRegister.HasValue ? prod.VariableLoanInformation.BondToRegister.Value : 0;

                CalculateOriginationFees(loanAgreeAmount, applicationBondToRegister, _appType, cashOut, overrideCancelFee, capitaliseFees, false, IsBondExceptionAction, isDiscountedInitiationFee, out initiationFeeDiscount, out initiationFee, out registrationFee, out cancelFee, out interimInterest, out bondToRegister, _appML.HasAttribute(OfferAttributeTypes.QuickPayLoan), householdIncome, employmentType, propertyValue, 0, cp.Application.HasAttribute(OfferAttributeTypes.StaffHomeLoan), applicationStartDate, capitaliseInitiationFee, _appML.HasAttribute(OfferAttributeTypes.GovernmentEmployeePensionFund));

                if (noInitFee)
                    initiationFee = 0;

                SaveFees(initiationFee, registrationFee, cancelFee, (overrideCancelFee != 0), interimInterest);

                productKey = prod.Application.GetLatestApplicationInformation().Product.Key;

                feesTotal = initiationFee + registrationFee + cancelFee;

                CalculateCommonApplicationDetail(prod.VariableLoanInformation, productKey, interestOnly, loanAgreeAmount, feesTotal, interimInterest, bondToRegister, keepMarketRate, initiationFee);

                newSPVKey = prod.VariableLoanInformation.SPV.Key;

                // need to know what year period this sl account is in
                int thresholdPeriod = 0;
                if (_appType == OfferTypes.FurtherLoan)
                {
                    IAccount acc = AccRepo.GetAccountByKey(prod.Application.ReservedAccount.Key);
                    foreach (IFinancialAdjustment fa in acc.FinancialServices[0].FinancialAdjustments)
                    {
                        if (fa.FinancialAdjustmentType.Key == (int)FinancialAdjustmentTypeSources.SuperLo)
                        {
                            TimeSpan dateDiff = DateTime.Now.Subtract(fa.FromDate.HasValue ? fa.FromDate.Value : DateTime.Now);
                            thresholdPeriod = (int)(dateDiff.Days / 365);
                            break;
                        }
                    }
                }

                //Threshold recalc

                IControl ctrl = CtrlRepo.GetControlByDescription("SuperRate New Client Threshold");

                loanAgreeAmount += interimInterest;

                if (capitaliseFees)
                    loanAgreeAmount += prod.VariableLoanInformation.FeesTotal.HasValue ? prod.VariableLoanInformation.FeesTotal.Value : 0;

                double threshold = loanAgreeAmount * (ctrl.ControlNumeric.HasValue ? ctrl.ControlNumeric.Value : 0);
                if (thresholdPeriod < 1)
                    prod.SuperLoInformation.PPThresholdYr1 = threshold;
                if (thresholdPeriod < 2)
                    prod.SuperLoInformation.PPThresholdYr2 = threshold;
                if (thresholdPeriod < 3)
                    prod.SuperLoInformation.PPThresholdYr3 = threshold;
                if (thresholdPeriod < 4)
                    prod.SuperLoInformation.PPThresholdYr4 = threshold;
                if (thresholdPeriod < 5)
                    prod.SuperLoInformation.PPThresholdYr5 = threshold;
            }

            if (cp.ProductType == SAHL.Common.Globals.Products.DefendingDiscountRate)
            {
                IApplicationProductDefendingDiscountLoan prod = cp as IApplicationProductDefendingDiscountLoan;
                if (newLoanAgreeAmount != 0)
                    prod.LoanAmountNoFees = newLoanAgreeAmount;

                loanAgreeAmount = prod.LoanAmountNoFees.HasValue ? prod.LoanAmountNoFees.Value : 0;
                applicationBondToRegister = prod.VariableLoanInformation.BondToRegister.HasValue ? prod.VariableLoanInformation.BondToRegister.Value : 0;

                CalculateOriginationFees(loanAgreeAmount, applicationBondToRegister, _appType, cashOut, overrideCancelFee, capitaliseFees, false, IsBondExceptionAction, isDiscountedInitiationFee, out initiationFeeDiscount, out initiationFee, out registrationFee, out cancelFee, out interimInterest, out bondToRegister, _appML.HasAttribute(OfferAttributeTypes.QuickPayLoan), householdIncome, employmentType, propertyValue, 0, cp.Application.HasAttribute(OfferAttributeTypes.StaffHomeLoan), applicationStartDate, capitaliseInitiationFee, _appML.HasAttribute(OfferAttributeTypes.GovernmentEmployeePensionFund));

                if (noInitFee)
                    initiationFee = 0;

                SaveFees(initiationFee, registrationFee, cancelFee, (overrideCancelFee != 0), interimInterest);

                productKey = prod.Application.GetLatestApplicationInformation().Product.Key;

                feesTotal = initiationFee + registrationFee + cancelFee;

                CalculateCommonApplicationDetail(prod.VariableLoanInformation, productKey, interestOnly, loanAgreeAmount, feesTotal, interimInterest, bondToRegister, keepMarketRate, initiationFee);

                newSPVKey = prod.VariableLoanInformation.SPV.Key;
            }

            if (cp.ProductType == SAHL.Common.Globals.Products.NewVariableLoan)
            {
                IApplicationProductNewVariableLoan prod = cp as IApplicationProductNewVariableLoan;
                if (newLoanAgreeAmount != 0)
                    prod.LoanAmountNoFees = newLoanAgreeAmount;

                loanAgreeAmount = prod.LoanAmountNoFees.HasValue ? prod.LoanAmountNoFees.Value : 0;
                applicationBondToRegister = prod.VariableLoanInformation.BondToRegister.HasValue ? prod.VariableLoanInformation.BondToRegister.Value : 0;

                CalculateOriginationFees(loanAgreeAmount, applicationBondToRegister, _appType, cashOut, overrideCancelFee, capitaliseFees, false, IsBondExceptionAction, isDiscountedInitiationFee,
                    out initiationFeeDiscount, out initiationFee, out registrationFee, out cancelFee, out interimInterest, out bondToRegister, _appML.HasAttribute(OfferAttributeTypes.QuickPayLoan), householdIncome, employmentType, propertyValue, 0, cp.Application.HasAttribute(OfferAttributeTypes.StaffHomeLoan), applicationStartDate, capitaliseInitiationFee, _appML.HasAttribute(OfferAttributeTypes.GovernmentEmployeePensionFund));

                if (noInitFee)
                    initiationFee = 0;

                SaveFees(initiationFee, registrationFee, cancelFee, (overrideCancelFee != 0), interimInterest);

                productKey = prod.Application.GetLatestApplicationInformation().Product.Key;

                feesTotal = initiationFee + registrationFee + cancelFee;

                CalculateCommonApplicationDetail(prod.VariableLoanInformation, productKey, interestOnly, loanAgreeAmount, feesTotal, interimInterest, bondToRegister, keepMarketRate, initiationFee);

                newSPVKey = prod.VariableLoanInformation.SPV.Key;
            }

            if (cp.ProductType == SAHL.Common.Globals.Products.Edge)
            {
                IApplicationProductEdge prod = cp as IApplicationProductEdge;
                if (newLoanAgreeAmount != 0)
                    prod.LoanAmountNoFees = newLoanAgreeAmount;

                loanAgreeAmount = prod.LoanAmountNoFees.HasValue ? prod.LoanAmountNoFees.Value : 0;
                applicationBondToRegister = prod.VariableLoanInformation.BondToRegister.HasValue ? prod.VariableLoanInformation.BondToRegister.Value : 0;

                CalculateOriginationFees(loanAgreeAmount, applicationBondToRegister, _appType, cashOut, overrideCancelFee, capitaliseFees, false, IsBondExceptionAction, isDiscountedInitiationFee,
                    out initiationFeeDiscount, out initiationFee, out registrationFee, out cancelFee, out interimInterest, out bondToRegister, _appML.HasAttribute(OfferAttributeTypes.QuickPayLoan), householdIncome, employmentType, propertyValue, 0, cp.Application.HasAttribute(OfferAttributeTypes.StaffHomeLoan), applicationStartDate, capitaliseInitiationFee, _appML.HasAttribute(OfferAttributeTypes.GovernmentEmployeePensionFund));

                if (noInitFee)
                    initiationFee = 0;

                SaveFees(initiationFee, registrationFee, cancelFee, (overrideCancelFee != 0), interimInterest);

                productKey = prod.Application.GetLatestApplicationInformation().Product.Key;

                feesTotal = initiationFee + registrationFee + cancelFee;

                CalculateCommonApplicationDetail(prod.VariableLoanInformation, productKey, true, loanAgreeAmount, feesTotal, interimInterest, bondToRegister, keepMarketRate, initiationFee);

                newSPVKey = prod.VariableLoanInformation.SPV.Key;
            }

            _appML.ResetConfiguration = AppRepo.GetApplicationResetConfiguration(newSPVKey, productKey);

            suppvlai.VariableLoanInformation.AppliedInitiationFeeDiscount = initiationFeeDiscount;

            switch (_appML.ResetConfiguration.Key)
            {
                case (int)ResetConfigurations.Eighteenth:
                    suppvlai.VariableLoanInformation.RateConfiguration = CMRepo.GetRateConfigurationByMarginKeyAndMarketRateKey(suppvlai.VariableLoanInformation.RateConfiguration.Margin.Key, (int)MarketRates.ThreeMonthJIBARRounded);
                    break;

                case (int)ResetConfigurations.TwentyFirst:
                    suppvlai.VariableLoanInformation.RateConfiguration = CMRepo.GetRateConfigurationByMarginKeyAndMarketRateKey(suppvlai.VariableLoanInformation.RateConfiguration.Margin.Key, (int)MarketRates.ThreeMonthJIBARRounded);
                    break;

                case (int)ResetConfigurations.TwentySecond:
                    suppvlai.VariableLoanInformation.RateConfiguration = CMRepo.GetRateConfigurationByMarginKeyAndMarketRateKey(suppvlai.VariableLoanInformation.RateConfiguration.Margin.Key, (int)MarketRates.ThreeMonthJIBARRounded);
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="aivl"></param>
        /// <param name="productKey"></param>
        /// <param name="interestOnly"></param>
        /// <param name="loanAmount"></param>
        /// <param name="feesTotal"></param>
        /// <param name="InterimInterest"></param>
        /// <param name="bondToRegister"></param>
        /// <param name="keepMarketRate"></param>
        /// <param name="initiationFee"></param>
        private void CalculateCommonApplicationDetail(IApplicationInformationVariableLoan aivl, int productKey, bool interestOnly, double loanAmount, double feesTotal, double InterimInterest, double bondToRegister, bool keepMarketRate, double initiationFee)
        {
            IApplicationProductVariFixLoan appVF = _appML.CurrentProduct as IApplicationProductVariFixLoan;
            IApplicationProductEdge appEdge = _appML.CurrentProduct as IApplicationProductEdge;
            ISupportsInterestOnlyApplicationInformation ioAppInfo = _appML.CurrentProduct as ISupportsInterestOnlyApplicationInformation;

            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            IDomainMessageCollection messages = spc.DomainMessages;
            int osKey = _appML.OriginationSource.Key;
            int mortgageLoanPurposeKey = (int)_mlp;
            int employmentTypeKey = aivl.EmploymentType.Key;
            double term = aivl.Term.HasValue ? aivl.Term.Value : 240;
            double householdIncome = aivl.HouseholdIncome.HasValue ? aivl.HouseholdIncome.Value : 0;
            double interestRate;
            double interestRateFix;
            double baseRateFix = 0;

            //Can not determine a CM if the employment type is Unknown, assume Salaried as
            //business rules should take care of employment records elsewhere.
            if (employmentTypeKey == (int)EmploymentTypes.Unknown || employmentTypeKey == (int)EmploymentTypes.Unemployed)
                employmentTypeKey = (int)EmploymentTypes.Salaried;

            ILookupRepository lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();
            IEventList<IMarketRate> mRates = lookupRepo.MarketRates;

            IApplicationInformationVarifixLoan aivf;
            if (appVF != null) // Get the marketrate key from VF info
            {
                aivf = appVF.VariFixInformation;

                foreach (IMarketRate mR in mRates)
                {
                    if (mR.Key == aivf.MarketRate.Key) //VF rate
                    {
                        baseRateFix = mR.Value;
                        break;
                    }
                }
            }

            //Populate the new data
            aivl.InterimInterest = InterimInterest;
            if (_appType != OfferTypes.FurtherAdvance && _appType != OfferTypes.ReAdvance)
            {
                aivl.FeesTotal = feesTotal;
                aivl.BondToRegister = bondToRegister;
            }

            loanAmount += InterimInterest;

            // LAA excludes fees, even if they are capitalised
            aivl.LoanAmountNoFees = loanAmount;

            if (_appML.HasAttribute(OfferAttributeTypes.CapitalizeFees))
            {
                loanAmount += feesTotal;
            }
            else if (_appML.HasAttribute(OfferAttributeTypes.CapitaliseInitiationFee))
            {
                loanAmount += initiationFee;
            }

            //LTV
            double propertyValue = aivl.PropertyValuation.HasValue ? aivl.PropertyValuation.Value : _appML.ClientEstimatePropertyValuation.HasValue ? _appML.ClientEstimatePropertyValuation.Value : 0;
            aivl.LTV = LoanCalculator.CalculateLTV(loanAmount, propertyValue);

            //SPV
            //Always reset the SPV on a recalc
            // even if it overwrites a user selected value as per LaraB
            // any view setting the SPV will have to do so after the call to recalc, before the call to save
            ISPVService spvServ = ServiceFactory.GetService<ISPVService>();

            // DetermineSPV returns a child SPV, for new business origination we only want MS (17) or BB (16)
            //aivl.SPV = LookupRepo.SPVList.ObjectDictionary[spvServ.DetermineSPV(aivl.LoanAgreementAmount.Value, aivl.PropertyValuation.Value, resetConfigKey, productKey, originationSourceKey, 0, 0, 0).ToString()];

            //aivl.SPV = LookupRepo.SPVList.ObjectDictionary[spvServ.GetSPVByLTV(aivl.LTV.Value).ToString()];

            spvServ.DetermineSPVOnApplication(_appML);

            // Credit Matrix/Criteria
            IFinancialsService finsS = ServiceFactory.GetService<IFinancialsService>();
            ICreditCriteria cc = null;

            var creditCriteriaAttributeType = CreditCriteriaAttributeTypes.NewBusiness;
            if (_appML.IsGEPF())
            {
                creditCriteriaAttributeType = CreditCriteriaAttributeTypes.GovernmentEmployeePensionFund;
            }

            cc = finsS.GetCreditCriteriaByLTVAndIncome(messages, osKey, productKey, mortgageLoanPurposeKey, employmentTypeKey, loanAmount, propertyValue, householdIncome, creditCriteriaAttributeType);

            //Exceptions CM
            if (cc == null)
                cc = finsS.GetCreditCriteriaException(messages, osKey, productKey, mortgageLoanPurposeKey, employmentTypeKey, loanAmount);

            //Exceptions CM as a catch all, 0 prop val
            if (cc == null)
                cc = finsS.GetCreditCriteriaException(messages, osKey, productKey, mortgageLoanPurposeKey, employmentTypeKey, 0);

            if (cc == null)
            {
                _dmc.Add(new Error("This application does not meet the current SAHL lending criteria and can not continue to be processed.", "This application does not meet the current SAHL lending criteria and can not continue to be processed."));
                return;
            }

            aivl.Category = cc.Category;
            aivl.CreditMatrix = cc.CreditMatrix;
            aivl.CreditCriteria = cc;

            //Default Rate config Key to 18th
            int marketRateKey = (int)MarketRates.ThreeMonthJIBARRounded;
            if (aivl.RateConfiguration != null)
                marketRateKey = aivl.RateConfiguration.MarketRate.Key;

            IRateConfiguration rc = RateConfiguration.GetByMarginKeyAndMarketRateKey(cc.Margin.Key, marketRateKey);
            aivl.RateConfiguration = rc;

            //Need to set the MarketRate to the MarketRate from the Rateconfiguration
            //We maintain this value because the MarketRate value could change after the final calc on this application is done
            //and we need to be able to check historically how we got to our calculated values

            //fb4010 maintain the market rate value at the time of the credit decision
            if (keepMarketRate)
            {
                //Get the interest rate to calculate on
                interestRate = aivl.MarketRate.Value + rc.Margin.Value;
                interestRateFix = baseRateFix + rc.Margin.Value;
            }
            else
            {
                aivl.MarketRate = rc.MarketRate.Value;
                //Get the interest rate to calculate on
                interestRate = aivl.RateConfiguration.MarketRate.Value + rc.Margin.Value;
                interestRateFix = baseRateFix + rc.Margin.Value;
            }

            foreach (IApplicationInformationFinancialAdjustment fa in _appML.GetLatestApplicationInformation().ApplicationInformationFinancialAdjustments)
            {
                if (fa.FromDate.HasValue
                    && fa.FromDate <= DateTime.Now
                    && fa.Discount.HasValue)
                {
                    interestRate += fa.Discount.HasValue ? fa.Discount.Value : 0;
                    interestRateFix += fa.Discount.HasValue ? fa.Discount.Value : 0;
                }
            }

            if (appVF != null) // Need to calculate the instalments etc....
            {
                aivf = appVF.VariFixInformation;
                double varInstalment;
                if (aivf.FixedPercent == 1)
                    varInstalment = 0;
                else
                    varInstalment = LoanCalculator.CalculateInstallment(loanAmount * (1 - aivf.FixedPercent), interestRate, term, interestOnly);

                double fixInstalment = LoanCalculator.CalculateInstallment(loanAmount * aivf.FixedPercent, interestRateFix, term, interestOnly);

                aivf.FixedInstallment = fixInstalment;
                aivl.MonthlyInstalment = varInstalment + fixInstalment;

                //PTI always calc on Amortising instalment
                if (interestOnly)
                {
                    if (aivf.FixedPercent == 1)
                        varInstalment = 0;
                    else
                        varInstalment = LoanCalculator.CalculateInstallment(loanAmount * (1 - aivf.FixedPercent), interestRate, term, false);

                    fixInstalment = LoanCalculator.CalculateInstallment(loanAmount * aivf.FixedPercent, interestRateFix, term, false);
                }
                aivl.PTI = LoanCalculator.CalculatePTI((varInstalment + fixInstalment), householdIncome);
            }
            else
            {
                //Calculate Interest only instalment
                if (interestOnly && ioAppInfo != null && ioAppInfo.InterestOnlyInformation != null)
                    ioAppInfo.InterestOnlyInformation.Installment = LoanCalculator.CalculateInstallment(loanAmount, interestRate, term, interestOnly);

                //else
                //    if (ioAppInfo != null && ioAppInfo.InterestOnlyInformation != null)
                //    {
                //        // Domain setup always creates this data with nulls, regardless if it is used, so dont remove it, just null the values
                //        ioAppInfo.InterestOnlyInformation.Installment = null;
                //        ioAppInfo.InterestOnlyInformation.MaturityDate = null;
                //    }

                //Calculate Amortising instalment
                aivl.MonthlyInstalment = LoanCalculator.CalculateInstallment(loanAmount, interestRate, term, false);

                //Calculate PTI: always done against the full amortising instalment
                aivl.PTI = LoanCalculator.CalculatePTI(aivl.MonthlyInstalment.Value, householdIncome);
            }

            //And a little extra work for Edge
            if (appEdge != null)
            {
                appEdge.EdgeInformation.InterestOnlyTerm = 36;
                appEdge.EdgeInformation.FullTermInstalment = aivl.MonthlyInstalment.Value;
                appEdge.EdgeInformation.AmortisationTermInstalment = LoanCalculator.CalculateInstallment(loanAmount, interestRate, (term - appEdge.EdgeInformation.InterestOnlyTerm), false);
                appEdge.EdgeInformation.InterestOnlyInstalment = ioAppInfo.InterestOnlyInformation.Installment.Value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="LoanAmount"></param>
        /// <param name="BondRequired"></param>
        /// <param name="appType"></param>
        /// <param name="CashOut"></param>
        /// <param name="OverrideCancelFee"></param>
        /// <param name="capitaliseFees"></param>
        /// <param name="NCACompliant"></param>
        /// <param name="IsBondExceptionAction"></param>
        /// <param name="IsDiscountedInitiationFee"></param>
        /// <param name="InitiationFeeDiscount"></param>
        /// <param name="InitiationFee"></param>
        /// <param name="RegistrationFee"></param>
        /// <param name="CancelFee"></param>
        /// <param name="InterimInterest"></param>
        /// <param name="BondToRegister"></param>
        /// <param name="IsQuickPayLoan"></param>
        /// <param name="HouseholdIncome"></param>
        /// <param name="EmploymentTypeKey"></param>
        /// <param name="PropertyValue"></param>
        /// <param name="ApplicationParentAccountKey"></param>
        /// <param name="IsStaffLoan"></param>
        /// <param name="OfferStartDate"></param>
        /// <param name="capitaliseInitiationFee"></param>
        public static void CalculateOriginationFees(double LoanAmount, double BondRequired, OfferTypes appType, double CashOut, double OverrideCancelFee, bool capitaliseFees, bool NCACompliant, bool IsBondExceptionAction, bool IsDiscountedInitiationFee, out double? InitiationFeeDiscount, out double InitiationFee, out double RegistrationFee, out double CancelFee, out double InterimInterest, out double BondToRegister, bool IsQuickPayLoan, double HouseholdIncome, int EmploymentTypeKey, double PropertyValue, int ApplicationParentAccountKey, bool IsStaffLoan, DateTime OfferStartDate, bool capitaliseInitiationFee, bool isGEPF)
        {
            InterimInterest = 0;
            CancelFee = 0;
            RegistrationFee = 0;
            BondToRegister = 0;
            InitiationFee = 0;
            InitiationFeeDiscount = null;

            var LTV = LoanCalculator.CalculateLTV(LoanAmount, PropertyValue);

            ParameterCollection prms = new ParameterCollection();
            Helper.AddFloatParameter(prms, "@LoanAmount", LoanAmount);
            Helper.AddFloatParameter(prms, "@BondRequired", BondRequired);
            Helper.AddIntParameter(prms, "@LoanType", (int)appType);
            Helper.AddFloatParameter(prms, "@CashOut", CashOut);
            Helper.AddFloatParameter(prms, "@OverRideCancelFee", OverrideCancelFee);
            Helper.AddBitParameter(prms, "@CapitaliseFees", (bool)capitaliseFees);
            Helper.AddBitParameter(prms, "@NCACompliant", (bool)NCACompliant);
            Helper.AddBitParameter(prms, "@IsBondExceptionAction", (bool)IsBondExceptionAction);
            Helper.AddBitParameter(prms, "@QuickPay", IsQuickPayLoan);
            Helper.AddFloatParameter(prms, "@HouseholdIncome", HouseholdIncome);
            Helper.AddIntParameter(prms, "@EmploymentTypeKey", EmploymentTypeKey);
            Helper.AddFloatParameter(prms, "@LTV", LTV);
            Helper.AddIntParameter(prms, "@ApplicationParentAccountKey", ApplicationParentAccountKey);
            Helper.AddBitParameter(prms, "@IsStaffLoan", IsStaffLoan);
            Helper.AddBitParameter(prms, "@IsDiscountedInitiationFee", IsDiscountedInitiationFee);
            Helper.AddDateParameter(prms, "@OfferStartDate", OfferStartDate);
            Helper.AddBitParameter(prms, "@CapitaliseInitiationFee", (bool)capitaliseInitiationFee);
            Helper.AddBitParameter(prms, "@isGEPF", isGEPF);

            string sqlQuery = UIStatementRepository.GetStatement("COMMON", "GetFees");

            DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sqlQuery, typeof(Fees_DAO), prms);

            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    InterimInterest = Convert.ToDouble(dr[0]);
                    CancelFee = Convert.ToDouble(dr[1]);
                    InitiationFee = Convert.ToDouble(dr[2]);
                    BondToRegister = Convert.ToDouble(dr[3]);
                    RegistrationFee = Convert.ToDouble(dr[4]);
                    InitiationFeeDiscount = Convert.ToDouble(dr[5]);
                }
            }
        }

        private void SaveFees(double initiationFee, double registrationFee, double cancelFee, bool overrideCancelFee, double interimInterest)
        {
            SetCancellationFee(cancelFee, overrideCancelFee);
            SetInitiationFee(initiationFee, false);
            SetInterimInterest(interimInterest);
            SetRegistrationFee(registrationFee, false);
        }

        private void GetInformationForFeesCalc(ISupportsVariableLoanApplicationInformation suppvlai, out double HouseholdIncome, out int EmploymentTypeKey, out double PropertyValue)
        {
            HouseholdIncome = suppvlai.VariableLoanInformation.HouseholdIncome.GetValueOrDefault(0);
            EmploymentTypeKey = suppvlai.VariableLoanInformation.EmploymentType.Key;
            PropertyValue = suppvlai.VariableLoanInformation.PropertyValuation.GetValueOrDefault(0);
        }

        public double MinBondRequired
        {
            get
            {
                // do any capacity calcs for the bond amount i.e.: further lending capacity
                double minBond = LoanCalculator.CalculateBondAmount(_appML.LoanAgreementAmount.HasValue ? _appML.LoanAgreementAmount.Value : 0);

                // get the next bond from the fee range
                string HQL = "from Fees_DAO f where f.Key >= ? order by f.Key";
                SimpleQuery<Fees_DAO> q = new SimpleQuery<Fees_DAO>(HQL, minBond);
                Fees_DAO[] res = q.Execute();

                if (res.Length == 0)//if we get nothing, return the maximum
                {
                    HQL = "from Fees_DAO f order by f.Key desc";
                    q = new SimpleQuery<Fees_DAO>(HQL);
                    res = q.Execute();
                }

                Fees_DAO fees = res[0];
                minBond = fees.Key;

                return minBond;
            }
        }

        public double? GetCashOut()
        {
            ISupportsVariableLoanApplicationInformation VLI = _appML.CurrentProduct as ISupportsVariableLoanApplicationInformation;

            if (VLI != null)
            {
                IApplicationInformationVariableLoanForSwitchAndRefinance VL = VLI.VariableLoanInformation as IApplicationInformationVariableLoanForSwitchAndRefinance;
                if (VL != null)
                {
                    return VL.RequestedCashAmount;
                }
            }

            throw new NullReferenceException();
        }

        public void SetCashOut(double? CashOut)
        {
            ISupportsVariableLoanApplicationInformation VLI = _appML.CurrentProduct as ISupportsVariableLoanApplicationInformation;
            if (VLI != null)
            {
                IApplicationInformationVariableLoanForSwitchAndRefinance VL = VLI.VariableLoanInformation as IApplicationInformationVariableLoanForSwitchAndRefinance;
                VL.RequestedCashAmount = CashOut;
            }
        }

        public double? GetCashDeposit()
        {
            ISupportsVariableLoanApplicationInformation supportsVariableLoanApplicationInformation = _appML.CurrentProduct as ISupportsVariableLoanApplicationInformation;

            if (supportsVariableLoanApplicationInformation != null)
                return supportsVariableLoanApplicationInformation.VariableLoanInformation.CashDeposit;
            else
                return null;
        }

        public void SetCashDeposit(double? CashDeposit)
        {
            ISupportsVariableLoanApplicationInformation supportsVariableLoanApplicationInformation = _appML.CurrentProduct as ISupportsVariableLoanApplicationInformation;

            if (supportsVariableLoanApplicationInformation != null)
                supportsVariableLoanApplicationInformation.VariableLoanInformation.CashDeposit = CashDeposit;
        }

        public double? GetInitiationFee()
        {
            double? initiationFee = null;

            foreach (IApplicationExpense applicationExpense in _appML.ApplicationExpenses)
            {
                if (applicationExpense.ExpenseType.Key == (int)ExpenseTypes.InitiationFeeBondPreparationFee)
                {
                    initiationFee = applicationExpense.TotalOutstandingAmount;
                    break;
                }
            }

            return initiationFee;
        }

        public void SetInitiationFee(double? InitiationFee, bool Override)
        {
            IApplicationExpense ae = null;

            foreach (IApplicationExpense appExpense in _appML.ApplicationExpenses)
            {
                if (appExpense.ExpenseType.Key == (int)ExpenseTypes.InitiationFeeBondPreparationFee)
                {
                    ae = appExpense;
                    break;
                }
            }

            if (ae != null)
            {
                if (!ae.OverRidden)
                {
                    ae.TotalOutstandingAmount = InitiationFee.Value;
                    ae.OverRidden = Override;
                }
                else if (Override)
                {
                    ae.TotalOutstandingAmount = InitiationFee.Value;

                    //ae.OverRidden = Override; //these will always both be true;
                }
            }
            else  //need to create the expense
            {
                if (InitiationFee.HasValue)
                {
                    ae = new ApplicationExpense(new ApplicationExpense_DAO());
                    ae.ExpenseType = AccRepo.GetExpenseTypeByKey((int)ExpenseTypes.InitiationFeeBondPreparationFee);
                    ae.TotalOutstandingAmount = InitiationFee.Value;
                    ae.OverRidden = Override;
                    ae.Application = _appML as IApplication;

                    _appML.ApplicationExpenses.Add(_dmc, ae);
                }
            }
        }

        public double? GetRegistrationFee()
        {
            double? registrationFee = null;

            foreach (IApplicationExpense applicationExpense in _appML.ApplicationExpenses)
            {
                if (applicationExpense.ExpenseType.Key == (int)ExpenseTypes.RegistrationFee)
                {
                    registrationFee = applicationExpense.TotalOutstandingAmount;
                    break;
                }
            }

            return registrationFee;
        }

        public void SetRegistrationFee(double? RegistrationFee, bool Override)
        {
            IApplicationExpense ae = null;

            foreach (IApplicationExpense appExpense in _appML.ApplicationExpenses)
            {
                if (appExpense.ExpenseType.Key == (int)ExpenseTypes.RegistrationFee)
                {
                    ae = appExpense;
                    break;
                }
            }

            if (ae != null)
            {
                if (!ae.OverRidden)
                {
                    ae.TotalOutstandingAmount = RegistrationFee.Value;
                    ae.OverRidden = Override;
                }
                else if (Override)
                {
                    ae.TotalOutstandingAmount = RegistrationFee.Value;

                    //ae.OverRidden = Override; //these will always both be true;
                }
            }
            else  //need to create the expense
            {
                if (RegistrationFee.HasValue)
                {
                    ae = new ApplicationExpense(new ApplicationExpense_DAO());
                    ae.ExpenseType = AccRepo.GetExpenseTypeByKey((int)ExpenseTypes.RegistrationFee);
                    ae.TotalOutstandingAmount = RegistrationFee.Value;
                    ae.OverRidden = Override;
                    ae.Application = _appML as IApplication;

                    _appML.ApplicationExpenses.Add(_dmc, ae);
                }
            }
        }

        public bool CapitaliseFees
        {
            get
            {
                foreach (IApplicationAttribute appAtr in _appML.CurrentProduct.Application.ApplicationAttributes)
                {
                    if (appAtr.ApplicationAttributeType.Key == (int)OfferAttributeTypes.CapitalizeFees)
                    {
                        return true;
                    }
                }

                return false;
            }
            set
            {
                bool capFees = false;
                IApplicationAttribute capFeesAttr = null;

                foreach (IApplicationAttribute appAtr in _appML.CurrentProduct.Application.ApplicationAttributes)
                {
                    if (appAtr.ApplicationAttributeType.Key == (int)OfferAttributeTypes.CapitalizeFees)
                    {
                        capFeesAttr = appAtr;
                        capFees = true;
                    }
                }

                if (value != capFees)//not the same, needs to be set
                {
                    IApplicationAttribute applicationAttribute = new ApplicationAttribute(new ApplicationAttribute_DAO());
                    applicationAttribute.ApplicationAttributeType = LookupRepo.ApplicationAttributesTypes.ObjectDictionary[Convert.ToString((int)OfferAttributeTypes.CapitalizeFees)];
                    applicationAttribute.Application = _appML.CurrentProduct.Application;

                    if (value)
                        _appML.CurrentProduct.Application.ApplicationAttributes.Add(_dmc, applicationAttribute);
                    else
                        _appML.CurrentProduct.Application.ApplicationAttributes.Remove(_dmc, capFeesAttr);
                }
            }
        }

        public double GetCancellationFee()
        {
            double cancellationFee = 0;

            foreach (IApplicationExpense applicationExpense in _appML.ApplicationExpenses)
            {
                if (applicationExpense.ExpenseType.Key == (int)ExpenseTypes.CancellationFee)
                {
                    cancellationFee = applicationExpense.TotalOutstandingAmount;
                    break;
                }
            }

            return cancellationFee;
        }

        public void SetCancellationFee(double CancellationFee, bool Override)
        {
            IApplicationExpense ae = null;

            foreach (IApplicationExpense appExpense in _appML.ApplicationExpenses)
            {
                if (appExpense.ExpenseType.Key == (int)ExpenseTypes.CancellationFee)
                {
                    ae = appExpense;
                    break;
                }
            }

            if (ae != null)
            {
                ae.TotalOutstandingAmount = CancellationFee;
                ae.OverRidden = Override;
            }
            else  //need to create the expense
            {
                ae = new ApplicationExpense(new ApplicationExpense_DAO());
                ae.ExpenseType = AccRepo.GetExpenseTypeByKey((int)ExpenseTypes.CancellationFee);
                ae.TotalOutstandingAmount = CancellationFee;
                ae.OverRidden = Override;
                ae.Application = _appML as IApplication;

                _appML.ApplicationExpenses.Add(_dmc, ae);
            }
        }

        public double? GetExistingLoan()
        {
            ISupportsVariableLoanApplicationInformation supportsVariableLoanApplicationInformation = _appML.CurrentProduct as ISupportsVariableLoanApplicationInformation;

            if (supportsVariableLoanApplicationInformation != null)
                return supportsVariableLoanApplicationInformation.VariableLoanInformation.ExistingLoan;
            else
                return null;
        }

        public void SetExistingLoan(double? ExistingLoan)
        {
            ISupportsVariableLoanApplicationInformation supportsVariableLoanApplicationInformation = _appML.CurrentProduct as ISupportsVariableLoanApplicationInformation;

            if (supportsVariableLoanApplicationInformation != null)
                supportsVariableLoanApplicationInformation.VariableLoanInformation.ExistingLoan = ExistingLoan;
        }

        public double? GetInterimInterest()
        {
            ISupportsVariableLoanApplicationInformation supportsVariableLoanApplicationInformation = _appML.CurrentProduct as ISupportsVariableLoanApplicationInformation;

            if (supportsVariableLoanApplicationInformation != null)
                return supportsVariableLoanApplicationInformation.VariableLoanInformation.InterimInterest;
            else
                return null;
        }

        public void SetInterimInterest(double? InterimInterest)
        {
            ISupportsVariableLoanApplicationInformation supportsVariableLoanApplicationInformation = _appML.CurrentProduct as ISupportsVariableLoanApplicationInformation;

            if (supportsVariableLoanApplicationInformation != null)
                supportsVariableLoanApplicationInformation.VariableLoanInformation.InterimInterest = InterimInterest;
        }

        public double? GetLoanAgreementAmount()
        {
            ISupportsVariableLoanApplicationInformation supportsVariableLoanApplicationInformation = _appML.CurrentProduct as ISupportsVariableLoanApplicationInformation;

            if (supportsVariableLoanApplicationInformation != null)
            {
                return (supportsVariableLoanApplicationInformation.VariableLoanInformation.LoanAgreementAmount ?? 0.0);
            }
            else
                return null;
        }

        public double EstimatedDisbursedLTV()
        {
            double _currBalance = 0D;

            ISupportsVariableLoanApplicationInformation suppvlai = _appML.CurrentProduct as ISupportsVariableLoanApplicationInformation;

            IMortgageLoanAccount mla = _appML.Account as IMortgageLoanAccount;
            IMortgageLoan vML = mla.SecuredMortgageLoan;
            IMortgageLoan fML = null;
            IAccountVariFixLoan _fAccount = _appML.Account as IAccountVariFixLoan;

            if (_fAccount != null)
                fML = _fAccount.FixedSecuredMortgageLoan;

            //get account loan amount plus accrued interest
            if (fML != null)
                _currBalance = Convert.ToDouble(vML.CurrentBalance + (vML.AccruedInterestMTD) + fML.CurrentBalance + (fML.AccruedInterestMTD));
            else
                _currBalance = Convert.ToDouble(vML.CurrentBalance + (vML.AccruedInterestMTD));

            //get application loan amount
            double LoanAmount = _currBalance + (suppvlai.VariableLoanInformation.LoanAgreementAmount.HasValue ? suppvlai.VariableLoanInformation.LoanAgreementAmount.Value : 0D);

            //get property value
            double propertyValue = vML.GetActiveValuationAmount();

            //calc LTV
            return LoanCalculator.CalculateLTV(LoanAmount, propertyValue);
        }

        private IAccountRepository AccRepo
        {
            get
            {
                if (_accRepo == null)
                    _accRepo = RepositoryFactory.GetRepository<IAccountRepository>();

                return _accRepo;
            }
        }

        private ILookupRepository LookupRepo
        {
            get
            {
                if (_lookupRepo == null)
                    _lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();

                return _lookupRepo;
            }
        }

        private IControlRepository CtrlRepo
        {
            get
            {
                if (_ctrlRepo == null)
                    _ctrlRepo = RepositoryFactory.GetRepository<IControlRepository>();

                return _ctrlRepo;
            }
        }

        private IApplicationRepository AppRepo
        {
            get
            {
                if (_appRepo == null)
                    _appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

                return _appRepo;
            }
        }

        private ICreditMatrixRepository CMRepo
        {
            get
            {
                if (_cmRepo == null)
                    _cmRepo = RepositoryFactory.GetRepository<ICreditMatrixRepository>();

                return _cmRepo;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="discount"></param>
        /// <returns></returns>
        private double CalculateDiscount(double amount, double discount)
        {
            double amountWithDiscount = amount;

            if (discount >= 0 && discount <= 1)
            {
                amountWithDiscount = amount * (1 - discount);
            }

            return amountWithDiscount;
        }

        public bool CapitaliseInitiationFees
        {
            set
            {
                if (value != _appML.HasAttribute(OfferAttributeTypes.CapitaliseInitiationFee))
                {
                    if (value)
                    {
                        IApplicationAttribute applicationAttribute = new ApplicationAttribute(new ApplicationAttribute_DAO());
                        applicationAttribute.ApplicationAttributeType = LookupRepo.ApplicationAttributesTypes.ObjectDictionary[Convert.ToString((int)OfferAttributeTypes.CapitaliseInitiationFee)];
                        applicationAttribute.Application = _appML.CurrentProduct.Application;
                        _appML.CurrentProduct.Application.ApplicationAttributes.Add(_dmc, applicationAttribute);
                    }
                    else
                    {
                        IApplicationAttribute capitaliseInitiationFeeAttribute = _appML.CurrentProduct.Application.ApplicationAttributes.FirstOrDefault(x => x.ApplicationAttributeType.Key == (int)OfferAttributeTypes.CapitaliseInitiationFee);
                        _appML.CurrentProduct.Application.ApplicationAttributes.Remove(_dmc, capitaliseInitiationFeeAttribute);
                    }
                }
            }
        }
    }
}