using System;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel.Helpers
{
    internal static class ApplicationProductMortgageLoanHelper
    {
        public static double? GetManualDiscount(IApplicationProductMortgageLoan ApplicationProduct)
        {
            IApplicationInformation applicationInformation = ApplicationProduct.Application.GetLatestApplicationInformation();
            foreach (IApplicationInformationFinancialAdjustment applicationInformationFinancialAdjustment in applicationInformation.ApplicationInformationFinancialAdjustments)
            {
                if (applicationInformationFinancialAdjustment.FinancialAdjustmentTypeSource.Key == (int)FinancialAdjustmentTypeSources.DiscountedLinkrate)
                {
                    return applicationInformationFinancialAdjustment.Discount;
                }
            }

            return null;
        }

        public static void SetManualDiscount(IApplicationProductMortgageLoan ApplicationProduct, double? Discount, FinancialAdjustmentTypeSources fats, SAHL.Common.Collections.Interfaces.IDomainMessageCollection Messages)
        {
            bool discountDone = false;
            IApplicationInformation ai = ApplicationProduct.Application.GetLatestApplicationInformation();
            
            foreach (IApplicationInformationFinancialAdjustment airo in ai.ApplicationInformationFinancialAdjustments)
            {
                if (airo.FinancialAdjustmentTypeSource.Key == (int)fats)
                {
                    if (Discount == null || Discount == 0) //Delete
                        ai.ApplicationInformationFinancialAdjustments.Remove(Messages, airo);
                    else //Update
                        airo.Discount = Discount;

                    discountDone = true;
                    break;
                }
            }

            //Create if it does not exist
            if (!discountDone && Discount != null && Discount != 0)
            {
                IApplicationInformationFinancialAdjustment airo = SetupNewRateOverride(Discount, fats);
                airo.ApplicationInformation = ai;
                ai.ApplicationInformationFinancialAdjustments.Add(Messages, airo);
            }

        }

        public static void SetLoanAgreementAmount(IApplicationProductMortgageLoan ApplicationProduct)
        {
            ISupportsVariableLoanApplicationInformation svlai = ApplicationProduct.Application.CurrentProduct as ISupportsVariableLoanApplicationInformation;

            if (svlai == null)
                return;

            ApplicationInformationVariableLoan aivl = svlai.VariableLoanInformation as ApplicationInformationVariableLoan;

            if (aivl == null)
                return;
            
            ApplicationInformationVariableLoan_DAO aivlDAO = aivl.GetDAOObject() as ApplicationInformationVariableLoan_DAO;

            if (aivlDAO == null)
                return;

            if (ApplicationProduct.Application.HasAttribute(OfferAttributeTypes.CapitalizeFees))
            {
                aivlDAO.LoanAgreementAmount = (ApplicationProduct.LoanAmountNoFees ?? 0D) + (aivl.FeesTotal ?? 0D);
            }
            else if (ApplicationProduct.Application.HasAttribute(OfferAttributeTypes.CapitaliseInitiationFee))
            {
                IApplicationMortgageLoanNewPurchase applicationMortgageLoanNewPurchase = ApplicationProduct.Application as IApplicationMortgageLoanNewPurchase;
                if (applicationMortgageLoanNewPurchase == null)
                {
                    aivlDAO.LoanAgreementAmount = (ApplicationProduct.LoanAmountNoFees ?? 0D);
                }
                else
                {
                    aivlDAO.LoanAgreementAmount = (ApplicationProduct.LoanAmountNoFees ?? 0D) + (applicationMortgageLoanNewPurchase.InitiationFee ?? 0D);
                }
            }
            else
            {
                aivlDAO.LoanAgreementAmount = (ApplicationProduct.LoanAmountNoFees ?? 0D);
            }
        }

        private static IApplicationInformationFinancialAdjustment SetupNewRateOverride(double? Discount, FinancialAdjustmentTypeSources fats)
        {
            IApplicationInformationFinancialAdjustment aro = new ApplicationInformationFinancialAdjustment(new ApplicationInformationFinancialAdjustment_DAO());
            aro.Discount = Discount;
            aro.FromDate = DateTime.Now;
            aro.FinancialAdjustmentTypeSource = FinancialAdjustmentRepo.GetFinancialAdjustmentTypeSourceByKey((int)fats);
            aro.Term = -1;

            return aro;
        }

        public static double? GetDiscountedLinkRate(IApplicationProductMortgageLoan ApplicationProduct)
        {
            double? discountedLinkRate = null;

            // Get the Variableinfo
            ISupportsVariableLoanApplicationInformation supportsVariableLoanApplicationInformation = ApplicationProduct as ISupportsVariableLoanApplicationInformation;
            if (supportsVariableLoanApplicationInformation == null)
                return null;

            discountedLinkRate = supportsVariableLoanApplicationInformation.VariableLoanInformation.RateConfiguration.Margin.Value;
            discountedLinkRate += ApplicationProductMortgageLoanHelper.GetManualDiscount(ApplicationProduct) ?? 0.0;

            return discountedLinkRate;
        }

        public static double? GetEffectiveRate(IApplicationProductMortgageLoan ApplicationProduct)
        {
            // Get the Effective rate
            if (ApplicationProduct.MarketRate == null || ApplicationProduct.DiscountedLinkRate == null)
                return null;

            return (ApplicationProduct.MarketRate + ApplicationProduct.DiscountedLinkRate);
        }

        public static double? GetMarketRate(IApplicationProductMortgageLoan ApplicationProduct)
        {
            // Get the market rate
            ISupportsVariableLoanApplicationInformation supportsVariableLoanApplicationInformation = ApplicationProduct as ISupportsVariableLoanApplicationInformation;
            if (supportsVariableLoanApplicationInformation == null)
                return null;

            return supportsVariableLoanApplicationInformation.VariableLoanInformation.RateConfiguration.MarketRate.Value;

        }

        public static double? GetFixedMarketRate(int MarketRateKey)
        {
            ILookupRepository lookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();

            return lookupRepository.MarketRates.ObjectDictionary[Convert.ToString(MarketRateKey)].Value;
        }

        public static double? GetLinkRate(IApplicationProductMortgageLoan ApplicationProduct)
        {
            // Get the market rate
            ISupportsVariableLoanApplicationInformation supportsVariableLoanApplicationInformation = ApplicationProduct as ISupportsVariableLoanApplicationInformation;
            if (supportsVariableLoanApplicationInformation == null)
                return null;

            return supportsVariableLoanApplicationInformation.VariableLoanInformation.RateConfiguration.Margin.Value;

        }

        public static double? GetLoanAgreementAmount(IApplicationProductMortgageLoan ApplicationProduct)
        {
            // Get the market rate
            ISupportsVariableLoanApplicationInformation supVLAI = ApplicationProduct as ISupportsVariableLoanApplicationInformation;
            if (supVLAI == null)
                return null;

            //double loanAmount = supVLAI.VariableLoanInformation.LoanAgreementAmount ?? 0.0;

            //if (ApplicationProduct.Application.HasAttribute(OfferAttributeTypes.CapitalizeFees))
            //    loanAmount += (supVLAI.VariableLoanInformation.FeesTotal ?? 0.0);

            //return loanAmount;
            return supVLAI.VariableLoanInformation.LoanAgreementAmount ?? 0.0;
        }
        
        #region fxcop
        //public static double? GetInstalment(IApplicationProductMortgageLoan ApplicationProduct)
        //{
        //    // Get the market rate
        //    ISupportsVariableLoanApplicationInformation supportsVariableLoanApplicationInformation = ApplicationProduct as ISupportsVariableLoanApplicationInformation;
        //    if (supportsVariableLoanApplicationInformation == null)
        //        return null;

        //    return supportsVariableLoanApplicationInformation.VariableLoanInformation.MonthlyInstalment;

        //}
        #endregion
        

        public static double CalculateLoyaltyBenefit(IApplicationProductMortgageLoan ApplicationProduct, int numberOfPeriods)
        {
            //Direct copy of SQL Function
            // SAHLDB..CalcLoyaltyBonus c/o Rodders 'this is why I am leaving for SBIC' AKA 'the Joller' Majola
            ISupportsSuperLoApplicationInformation sl = ApplicationProduct as ISupportsSuperLoApplicationInformation;
            if (sl == null)
                return 0;
            
            double tempBonus = 0;
            int maxPeriod = 0;
            int period = 0;
            double accumulatedBonus = 0;
            int termsProcessed = 0;
            //int periodZeroBased = 0;     // Cater for terms starting at (one)
            double installment = 0;
            double loanBalanceNotional = 0;

            double loanBalance = ApplicationProduct.LoanAgreementAmount.HasValue ? ApplicationProduct.LoanAgreementAmount.Value : 0;
            double interest = (ApplicationProduct.EffectiveRate.HasValue ? ApplicationProduct.EffectiveRate.Value : 0)/12;
            int loanTerm = ApplicationProduct.Term.HasValue ? ApplicationProduct.Term.Value : 0;


            IControlRepository ctrlRepo = RepositoryFactory.GetRepository<IControlRepository>();
            IControl ctrl = ctrlRepo.GetControlByDescription("SuperRate Discount");
            
            if (ctrl == null || !ctrl.ControlNumeric.HasValue)
                throw new ArgumentNullException("There is no value for 'SuperRate Discount' in the control table");
            
            double loyaltyRate = ctrl.ControlNumeric.Value / 12;
            
            for (int i = 1; i < numberOfPeriods; i++)
            {
                if (accumulatedBonus == 0)
                    maxPeriod = 24;
                else
                    maxPeriod = 12;
        	
                if (numberOfPeriods < maxPeriod)
                    period = numberOfPeriods;
                else
                    period = maxPeriod;


                loanBalanceNotional = loanBalance;
                installment = (interest * (loanBalance * Math.Pow(1 + interest, (loanTerm - termsProcessed))) / ((1 + interest * 0) * (1 - Math.Pow(1 + interest, (loanTerm - termsProcessed)))) * -1);
                tempBonus = (Math.Pow(interest, 2) + period * installment * interest + (interest * loanBalance - installment) * (Math.Pow(1 + interest, period) - 1)) / interest;
                tempBonus = tempBonus - (Math.Pow((interest - loyaltyRate), 2) + period * installment * (interest - loyaltyRate) + ((interest - loyaltyRate) * loanBalanceNotional - installment) * (Math.Pow(1 + (interest - loyaltyRate), period) - 1)) / (interest - loyaltyRate);

                termsProcessed = termsProcessed + period;

                loanBalance = (loanBalance * Math.Pow((1 + interest), period) - installment * (Math.Pow((1 + interest), period) - 1) / interest) - tempBonus;

                numberOfPeriods = numberOfPeriods - period;
                accumulatedBonus = accumulatedBonus + tempBonus;
            } 

            return accumulatedBonus;
        }

        /// <summary>
        /// When changing the Product or creating a revision the previous OfferInformation SL or VF 
        /// Conversion Status must be set to 3.
        /// </summary>
        public static void SetConversionStatus(IApplication application, int NewProduct, bool resetStatus)
        {
           if (application.ProductHistory.Length > 0)
            {
                int oldProductType = (int)application.CurrentProduct.ProductType;
                int newProductType = NewProduct;

                if ((oldProductType != newProductType || resetStatus) && oldProductType == (int)Products.VariFixLoan)
                {
                    IApplicationProductVariFixLoan apvfl = application.CurrentProduct as IApplicationProductVariFixLoan;
                    if (apvfl != null)
                    {
                        apvfl.VariFixInformation.ConversionStatus = 3;
                    }
                }

                if ((oldProductType != newProductType || resetStatus) && oldProductType == (int)Products.SuperLo)
                {
                    IApplicationProductSuperLoLoan apsl = application.CurrentProduct as IApplicationProductSuperLoLoan;
                    if (apsl != null)
                    {
                        apsl.SuperLoInformation.Status = 3;
                    }
                }
            }
        }

        private static ILookupRepository _lookupRepo;
        private static ILookupRepository LookupRepo
        {
            get
            {
                if (_lookupRepo == null)
                    _lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();

                return _lookupRepo;
            }
        }

        private static IFinancialAdjustmentRepository _financialAdjustmentRepo;
        private static IFinancialAdjustmentRepository FinancialAdjustmentRepo
        {
            get
            {
                if (_financialAdjustmentRepo == null)
                    _financialAdjustmentRepo = RepositoryFactory.GetRepository<IFinancialAdjustmentRepository>();

                return _financialAdjustmentRepo;
            }
        }
    }
}
