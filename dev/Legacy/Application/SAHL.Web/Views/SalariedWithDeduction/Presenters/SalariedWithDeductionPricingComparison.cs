using SAHL.Common;
using SAHL.Common.BusinessModel.Helpers;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.SalariedWithDeduction.Interfaces;
using SAHL.Web.Views.SalariedWithDeduction.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAHL.Web.Views.SalariedWithDeduction.Presenters
{
    public class SalariedWithDeductionPricingComparison : SAHLCommonBasePresenter<ISalariedWithDeductionPricingComparison>
    {
        private IApplicationRepository applicationRepo;
        private IControlRepository controlRepo;

        public SalariedWithDeductionPricingComparison(ISalariedWithDeductionPricingComparison view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            applicationRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            controlRepo = RepositoryFactory.GetRepository<IControlRepository>();
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;

            InstanceNode node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal, CBONodeSetType.X2) as InstanceNode;
            if (node == null)
            {
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);
            }
            int applicationKey = node.GenericKey;
            IApplication application = applicationRepo.GetApplicationByKey(applicationKey);

            double discountedInterestRate = GetDiscountedInterestRateForSWD();

            ApplicationPricingDetailModel currentPricing = GetCurrentPricingDetail(application);
            ApplicationPricingDetailModel newPricing = GetNewPricingDetail(application, currentPricing, discountedInterestRate);

            View.OnCancelButtonClicked += View_OnCancelButtonClicked;
            View.DisplayCurrentApplicationPricingDetails(currentPricing);
            View.DisplayNewApplicationPricingDetails(newPricing);
        }

        private void View_OnCancelButtonClicked(object sender, EventArgs e)
        {
            X2Service.CancelActivity(_view.CurrentPrincipal);
            this.X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
        }

        private double GetDiscountedInterestRateForSWD()
        {
            return controlRepo.GetControlByDescription(SAHL.Common.Constants.ControlTable.StopOrderDiscountPercentage).ControlNumeric.Value;
        }

        private ApplicationPricingDetailModel GetCurrentPricingDetail(IApplication application)
        {
            ApplicationPricingDetailModel pricingModel = new ApplicationPricingDetailModel();

            ISupportsVariableLoanApplicationInformation vlInfo = application.CurrentProduct as ISupportsVariableLoanApplicationInformation;
            IApplicationInformationVariableLoan variableLoanInformation = vlInfo.VariableLoanInformation;

            pricingModel.Term = variableLoanInformation.Term;
            pricingModel.TotalLoanRequirement = variableLoanInformation.LoanAgreementAmount.Value;
            pricingModel.MarketRate = variableLoanInformation.MarketRate.HasValue ? variableLoanInformation.MarketRate.Value : 0;
            pricingModel.LinkRate = variableLoanInformation.RateConfiguration.Margin.Value;
            pricingModel.PricingAdjustment = application.GetLatestApplicationInformation().ApplicationInformationFinancialAdjustments.Where(x => x.FromDate < DateTime.Now && x.Discount.HasValue).Sum(v => v.Discount.HasValue ? v.Discount.Value : 0);
            pricingModel.EffectiveRate = pricingModel.MarketRate + pricingModel.LinkRate + pricingModel.PricingAdjustment;
            pricingModel.LTV = variableLoanInformation.LTV.HasValue? variableLoanInformation.LTV.Value : 0;
            pricingModel.PTI = variableLoanInformation.PTI.HasValue? variableLoanInformation.PTI.Value : 0;
            pricingModel.Instalment = variableLoanInformation.MonthlyInstalment.HasValue? variableLoanInformation.MonthlyInstalment.Value : 0;

            pricingModel.Interest = LoanCalculator.CalculateInterestOverTerm(pricingModel.TotalLoanRequirement.Value, pricingModel.EffectiveRate.Value, pricingModel.Term.Value, false);

            double registrationFee = 0;
            double initiationFee = 0;
             
            foreach (IApplicationExpense ae in application.ApplicationExpenses)
            {
                if (ae.ExpenseType.Key == (int)ExpenseTypes.RegistrationFee)
                    registrationFee = ae.TotalOutstandingAmount;

                if (ae.ExpenseType.Key == (int)ExpenseTypes.InitiationFeeBondPreparationFee)
                    initiationFee = ae.TotalOutstandingAmount;
            }

            pricingModel.RegistrationFee = registrationFee;
            pricingModel.InitiationFee = initiationFee;
            pricingModel.TotalFees = initiationFee + registrationFee;

            return pricingModel;
        }

        private ApplicationPricingDetailModel GetNewPricingDetail(IApplication application, ApplicationPricingDetailModel currentPricing, double discountedInterestRate)
        {
            ISupportsVariableLoanApplicationInformation vlInfo = application.CurrentProduct as ISupportsVariableLoanApplicationInformation;
            IApplicationInformationVariableLoan variableLoanInformation = vlInfo.VariableLoanInformation;

            ApplicationPricingDetailModel newPricing = currentPricing.Clone();
            newPricing.DiscountOnRate = discountedInterestRate;
            newPricing.EffectiveRate += discountedInterestRate;

            double householdIncome = variableLoanInformation.HouseholdIncome.Value;
            double newInstalment = LoanCalculator.CalculateInstallment(newPricing.TotalLoanRequirement.Value, newPricing.EffectiveRate.Value, newPricing.Term.Value, false);
            double pti = LoanCalculator.CalculatePTI(newInstalment, householdIncome);
            double interestOverTerm = LoanCalculator.CalculateInterestOverTerm(newPricing.TotalLoanRequirement.Value, newPricing.EffectiveRate.Value, newPricing.Term.Value, false);

            newPricing.Instalment = newInstalment;
            newPricing.PTI = pti;
            newPricing.Interest = interestOverTerm;

            return newPricing;
        }

    }
}