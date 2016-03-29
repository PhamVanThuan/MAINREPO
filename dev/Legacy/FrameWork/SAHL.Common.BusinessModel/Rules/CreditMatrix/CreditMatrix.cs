using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Helpers;

namespace SAHL.Common.BusinessModel.Rules.CreditMatrix
{
    [RuleDBTag("CreditMatrixInvestmentPropertySecondary",
    "Whenever the property Occupancy Type is not equal to ‘Owner Occupied’, the Max LTV is 75%.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.CreditMatrix.CreditMatrixInvestmentPropertySecondary")]
    [RuleParameterTag(new string[] { "@SecondaryPropertyMaxLTV,0.75,7" })]
    [RuleInfo]
    public class CreditMatrixInvestmentPropertySecondary : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #   region Check for allowed object type(s)
            if (Parameters.Length == 0)
                throw new ArgumentException("The CreditMatrixInvestmentPropertySecondary  rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IApplicationMortgageLoan))
                throw new ArgumentException("The CreditMatrixInvestmentPropertySecondary  rule expects the following objects to be passed: IApplicationMortgageLoan.");

            if (RuleItem.RuleParameters.Count < 1)
                throw new Exception(String.Format("Missing rule parameter configuration for the rule {0}.", RuleItem.Name));
            #endregion

            # region Rule Check

            IApplicationMortgageLoan application = Parameters[0] as IApplicationMortgageLoan;
            IApplicationProduct appP = application.CurrentProduct;
            ISupportsVariableLoanApplicationInformation suppvlai = appP as ISupportsVariableLoanApplicationInformation;

            //seperate rules check that a property exists, so ignore this here
            if (application != null && application.Property != null && application.Property.OccupancyType != null)
            {
                if (application.Property.OccupancyType.Key != (int)OccupancyTypes.OwnerOccupied)
                {
                    double maxLTV = Convert.ToDouble(RuleItem.RuleParameters[0].Value);
                    double appLTV = (suppvlai.VariableLoanInformation.LTV.HasValue ? suppvlai.VariableLoanInformation.LTV.Value : 1);

                    if (appLTV > maxLTV)
                    {
                        string err = String.Format("Properties that are not Owner Occupied have a max LTV of " + maxLTV.ToString(SAHL.Common.Constants.RateFormat) + ".");
                        AddMessage(err, err, Messages);
                    }
                }
            }
            
            #endregion

            return 0;
        }
    }


    [RuleDBTag("CreditMatrixRefinanceLoans",
   "Where purpose is switch, and Cashout > 50% of the loan amount, then warn the user:",
    "SAHL.Rules.DLL",
   "SAHL.Common.BusinessModel.Rules.CreditMatrix.CreditMatrixRefinanceLoans")]
    [RuleInfo]
    public class CreditMatrixRefinanceLoans : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {

            #   region Check for allowed object type(s)
            if (Parameters.Length == 0)
                throw new ArgumentException("The CreditMatrixRefinanceLoans  rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IApplication))
                throw new ArgumentException("The CreditMatrixRefinanceLoans  rule expects the following objects to be passed: IApplication.");

            #endregion

            # region Rule Check

            IApplication app = Parameters[0] as IApplication;

            IApplicationMortgageLoanWithCashOut mlco = app as IApplicationMortgageLoanWithCashOut;
            ISupportsVariableLoanApplicationInformation vlai = app.CurrentProduct as ISupportsVariableLoanApplicationInformation;

            if (app.ApplicationType.Key == (int)OfferTypes.SwitchLoan)
                if ((mlco.RequestedCashAmount * 2) >= vlai.VariableLoanInformation.PropertyValuation)
                    AddMessage(String.Format("Loan Purpose should be of type Refinance for loans where the Cash Out is > 50% of the Property Valuation."), "", Messages);

            #endregion

            return 0;
        }
    }


    [RuleDBTag("CreditMatrixRiskCategory5",
    "Max 80% LTV and Max 25% PTI for property risk category 5.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.CreditMatrix.CreditMatrixRiskCategory5")]
    [RuleParameterTag(new string[] { "@PropertyRiskCategory5MaxLTV,0.80,7", "@PropertyRiskCategory5MaxPTI,0.25,7" })]
    [RuleInfo]
    public class CreditMatrixRiskCategory5 : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {

            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The CreditMatrixRiskCategory5  rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IApplicationMortgageLoan))
                throw new ArgumentException("The CreditMatrixRiskCategory5  rule expects the following objects to be passed: IApplicationMortgageLoan.");

            #endregion

            # region Rule Check

            IApplicationMortgageLoan appML = Parameters[0] as IApplicationMortgageLoan;

            if (appML.Property != null)
            {
                double maxLTV = Convert.ToDouble(RuleItem.RuleParameters[0].Value);
                double maxPTI = Convert.ToDouble(RuleItem.RuleParameters[1].Value);

                ISupportsVariableLoanApplicationInformation vlai = appML.CurrentProduct as ISupportsVariableLoanApplicationInformation;
                IApplicationInformationVariableLoan vli = vlai.VariableLoanInformation;

                if (appML.Property.AreaClassification.Key >= (int)AreaClassifications.Class5)
                {
                    // for fl we need to add pti and ltv from the application to the 
                    // existing values for the Account
                    // FL stores atomic PTI and LTV per application type
                    // We are not concerned about other applications, as only one application can be processed at a time.
                    if (appML.ApplicationType.Key == (int)OfferTypes.ReAdvance ||
                        appML.ApplicationType.Key == (int)OfferTypes.FurtherAdvance ||
                        appML.ApplicationType.Key == (int)OfferTypes.FurtherLoan)
                    {
                        //Get Account LTV & PTI
                        IAccountRepository accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
                        int accountKey = appML.ReservedAccount.Key;
                        IAccount acc = accRepo.GetAccountByKey(accountKey);

                        double fixLAA = 0;
                        double fixInstal = 0;
                        double fixLTV = 0;
                        double fixPTI = 0;
                        double varLTV = 0;
                        double varPTI = 0;

                        //get mulit-use values
                        double margin = vli.RateConfiguration.Margin.Value;
                        double valuation = vli.PropertyValuation.HasValue ? vli.PropertyValuation.Value : 0D;
                        double income = vli.HouseholdIncome.HasValue ? vli.HouseholdIncome.Value : 0D;
                        int term = vli.Term.HasValue ? vlai.VariableLoanInformation.Term.Value : 1;
                        bool interestOnly = appML.HasFinancialAdjustment(FinancialAdjustmentTypeSources.InterestOnly);

                        IMortgageLoanAccount mla = acc as IMortgageLoanAccount;
                        IMortgageLoan vml = mla.SecuredMortgageLoan;
                        
                        //Get any varifix details, need to recalc using any application update values e.g.: Margin (LinkRate)
                        if (acc.Product.Key == (int)SAHL.Common.Globals.Products.VariFixLoan)
                        {
                            IAccountVariFixLoan fAccount = acc as IAccountVariFixLoan;
                            IMortgageLoan fml;
                            fml = fAccount.FixedSecuredMortgageLoan;
                            fixLAA = fml.CurrentBalance;
                            
                            if (fixLAA > 0) // use 0 values if there is no outstanding amount and dont bother recalc, will generate errors
                            {
								double fixRate = ((fml.ActiveMarketRate) + (fml.RateAdjustment) + margin);
                                //recalc instalment with new rate, it could have changed
                                fixInstal = LoanCalculator.CalculateInstallment(fixLAA, fixRate, term, interestOnly);
                                //calc LTV
                                fixLTV = LoanCalculator.CalculateLTV(fixLAA, valuation);
                                //calc PTI
                                fixPTI = LoanCalculator.CalculatePTI(fixInstal, income);
                            }
                        }

                        //recalc the variable portion using any updated application values
						//There used to be logic that stipulated the ActiveMarketRate could be null and that the BaseRate should be used in those cases, in the new revamp project, we have been made a promise by the database
						//that the Active Market Rate should never be null
						double varRate = ((vml.ActiveMarketRate) + (vml.RateAdjustment) + margin);
                        double varLAA = vml.CurrentBalance + (vli.LoanAgreementAmount.HasValue ? vli.LoanAgreementAmount.Value : 0D);
                        double varInstal = 0D;

                        // PTI for Edge should be calculated on the Amortising Term
                        if (acc.Product.Key == (int)SAHL.Common.Globals.Products.Edge)
                        {
                            int termEdge = vml.RemainingInstallments;
                            IFinancialAdjustment intOnlyFinancialAdjustment = null;
                            int intOnlyRemainderMonths = 0;

                            foreach(IFinancialAdjustment financialAdjustment in vml.FinancialAdjustments)
                            {
                                if (financialAdjustment.FinancialAdjustmentTypeSource.Key == (int)FinancialAdjustmentTypeSources.InterestOnly
                                    && financialAdjustment.FinancialAdjustmentStatus.Key == (int)FinancialAdjustmentStatuses.Active)
                                {
                                    intOnlyFinancialAdjustment = financialAdjustment;
                                    break;
                                }
                            }

                            if (intOnlyFinancialAdjustment != null)
                            {
                                System.DateTime dtStart = DateTime.Now;
                                System.DateTime dtEndRateOverride = intOnlyFinancialAdjustment.EndDate.HasValue ? intOnlyFinancialAdjustment.EndDate.Value : dtStart;
                                int intOnlyRemainderYears = (dtEndRateOverride.Year - dtStart.Year) * 12;
                                intOnlyRemainderMonths = Math.Abs(dtStart.Month - dtEndRateOverride.Month);
                                intOnlyRemainderMonths += intOnlyRemainderYears;
                            }
                            termEdge -= intOnlyRemainderMonths;
                            varInstal = LoanCalculator.CalculateInstallment(varLAA, varRate, termEdge, false);
                        }
                        else
                        {
                            //recalc instalment with new rate, it could have changed
                            varInstal = LoanCalculator.CalculateInstallment(varLAA, varRate, term, interestOnly);
                        }
                        //calc LTV
                        varLTV = LoanCalculator.CalculateLTV(varLAA, valuation);
                        //calc PTI
                        varPTI = LoanCalculator.CalculatePTI(varInstal, income);
                        
                        //sum variable and fixed amounts
                        double LTV = varLTV + fixLTV;
                        double PTI = varPTI + fixPTI;

                        if (LTV > maxLTV || PTI > maxPTI)
                        {
                            string err = String.Format("A maximum LTV of " + maxLTV.ToString(SAHL.Common.Constants.RateFormat) + " (current: " + LTV.ToString(SAHL.Common.Constants.RateFormat)
                                + ")and maximum PTI of " + maxPTI.ToString(SAHL.Common.Constants.RateFormat) + " (current: " + PTI.ToString(SAHL.Common.Constants.RateFormat)
                                + ") is allowed for property risk " + appML.Property.AreaClassification.Description + ".");
                            AddMessage(err, err, Messages);
                        }
                    }
                    else
                    {
                        //For new business we can just check the LTV and PTI stored against the application
                        if ((vli.LTV.HasValue ? vli.LTV.Value : 0D) > maxLTV || (vli.PTI.HasValue ? vli.PTI.Value : 0D) > maxPTI)
                        {
                            string err = String.Format("A maximum LTV of " + maxLTV.ToString(SAHL.Common.Constants.RateFormat) + " (current: " + vlai.VariableLoanInformation.LTV.Value.ToString(SAHL.Common.Constants.RateFormat) 
                                + ")and maximum PTI of " + maxPTI.ToString(SAHL.Common.Constants.RateFormat) + " (current: " + vlai.VariableLoanInformation.PTI.Value.ToString(SAHL.Common.Constants.RateFormat) 
                                + ") is allowed for property risk " + appML.Property.AreaClassification.Description + ".");
                            AddMessage(err, err, Messages);
                        }
                    }

                }
            }

            #endregion
            return 0;
        }
    }
}
