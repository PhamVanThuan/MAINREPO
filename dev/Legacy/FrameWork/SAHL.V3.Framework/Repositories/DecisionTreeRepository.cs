using SAHL.Common.BusinessModel.Helpers;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.V3.Framework.Model;
using SAHL.V3.Framework.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.V3.Framework.Repositories
{
    public class DecisionTreeRepository : IDecisionTreeRepository
    {
        private IDecisionTreeService decisionTreeService;
        private ILegalEntityRepository legalEntityRepo;
        private ICreditScoringRepository creditScoringRepo;
        private IEmploymentRepository employmentRepo;
        private IApplicationRepository applicationRepo;

        public DecisionTreeRepository(IDecisionTreeService decsionTreeService, IApplicationRepository applicationRepo, ILegalEntityRepository legalEntityRepo, ICreditScoringRepository creditScoringRepo, IEmploymentRepository employmentRepo)
        {
            this.decisionTreeService = decsionTreeService;
            this.applicationRepo = applicationRepo;
            this.legalEntityRepo = legalEntityRepo;
            this.creditScoringRepo = creditScoringRepo;
            this.employmentRepo = employmentRepo;
        }

        public QualifyApplicationFor30YearLoanTermResult QualifyApplication(int applicationKey)
        {
            double marketRate = 0D;
            double linkRate = 0D;
            double pricingFoRiskAdj = 0D;
            double effectiveRate = 0D;

            var application = applicationRepo.GetApplicationByKey(applicationKey);
            ISupportsVariableLoanApplicationInformation vlInfo = application.CurrentProduct as ISupportsVariableLoanApplicationInformation;
            IApplicationInformationVariableLoan variableLoanInformation = vlInfo.VariableLoanInformation;

            marketRate = variableLoanInformation.MarketRate.HasValue ? variableLoanInformation.MarketRate.Value : 0;
            linkRate = variableLoanInformation.RateConfiguration.Margin.Value;
            pricingFoRiskAdj = application.GetLatestApplicationInformation().ApplicationInformationFinancialAdjustments.Where(x => x.FromDate < DateTime.Now && x.Discount.HasValue).Sum(v => v.Discount.HasValue ? v.Discount.Value : 0);
            effectiveRate = marketRate + linkRate + pricingFoRiskAdj;

            double propertyValue = variableLoanInformation.PropertyValuation.HasValue ? variableLoanInformation.PropertyValuation.Value : 0;

            // get the higest income contributer here
            int primaryApplicantLEKey = 0; int secondaryApplicantLEKey = 0; int numApplicants = 0;
            applicationRepo.GetPrimaryAndSecondaryApplicants(application.Key, out primaryApplicantLEKey, out secondaryApplicantLEKey, out numApplicants);

            ILegalEntityNaturalPerson highestIncomeContributer = legalEntityRepo.GetLegalEntityByKey(primaryApplicantLEKey) as ILegalEntityNaturalPerson;

            double creditScore = -999;
            // get latest itc
            if (highestIncomeContributer.ITCs != null && highestIncomeContributer.ITCs.Count > 0)
            {
                var latestITC = (from i in highestIncomeContributer.ITCs orderby i.ChangeDate descending select i).First() as SAHL.Common.BusinessModel.ITC;
                // get the higest income contributers credit score
                creditScore = creditScoringRepo.GetEmpiricaFromITC(latestITC);
            }

            var employmentType = employmentRepo.DetermineHighestIncomeContributersEmploymentType(highestIncomeContributer);

            //populate detail for current term loan detail
            var interestOverTermForCurrentTerm = LoanCalculator.CalculateInterestOverTerm(variableLoanInformation.LoanAgreementAmount.Value, effectiveRate, variableLoanInformation.Term.Value, false);
            var loanDetailForCurrentTerm = new ApplicationLoanDetail(variableLoanInformation.Term, variableLoanInformation.LoanAgreementAmount, variableLoanInformation.LTV, variableLoanInformation.PTI, marketRate, linkRate, pricingFoRiskAdj, effectiveRate, variableLoanInformation.MonthlyInstalment, interestOverTermForCurrentTerm);

            QualifyHighestIncomeContributorFor30YearLoanTermQuery highestIncomeContributerQuery = new QualifyHighestIncomeContributorFor30YearLoanTermQuery(
                highestIncomeContributer.CurrentAge.HasValue ? highestIncomeContributer.CurrentAge.Value : 0,
                (int)creditScore,
                String.IsNullOrWhiteSpace(highestIncomeContributer.IDNumber) ? String.Empty : highestIncomeContributer.IDNumber,
                highestIncomeContributer.GetLegalName(LegalNameFormat.Full),
                employmentType);

            // run application through the descision tree
            var query = new QualifyApplicationFor30YearLoanTermQuery(
                application.HasAttribute(OfferAttributeTypes.CreditDisqualified30YearTerm),
                variableLoanInformation.LTV.HasValue ? variableLoanInformation.LTV.Value : 0,
                variableLoanInformation.PTI.HasValue ? variableLoanInformation.PTI.Value : 0,
                application.GetHouseHoldIncome(),
                marketRate,
                pricingFoRiskAdj,
                linkRate,
                variableLoanInformation.LoanAgreementAmount.HasValue ? variableLoanInformation.LoanAgreementAmount.Value : 0,
                (OfferTypes)application.ApplicationType.Key,
                application.CurrentProduct.ProductType,
                propertyValue,
                application.IsAlphaHousing(),
                highestIncomeContributerQuery,
                application.HasFinancialAdjustment(FinancialAdjustmentTypeSources.InterestOnly)
                );

            decisionTreeService.QualifyApplicationFor30YearLoanTerm(query);

            //populate detail for 30 year term loan detail provided the application qualifies
            ApplicationLoanDetail loanDetailFor30YearTerm = null;
            if (query.Result.QualifiesForThirtyYearLoanTerm)
            {
                var loanAmount = variableLoanInformation.LoanAgreementAmount.HasValue ? variableLoanInformation.LoanAgreementAmount.Value : 0;
                var interestOverTermFor30YearTerm = LoanCalculator.CalculateInterestOverTerm(loanAmount, query.Result.InterestRateThirtyYear, query.Result.TermThirtyYear, false);
                loanDetailFor30YearTerm = new ApplicationLoanDetail(query.Result.TermThirtyYear, variableLoanInformation.LoanAgreementAmount, query.Result.LoantoValueThirtyYear, query.Result.PaymenttoIncomeThirtyYear, query.MarketRate, query.LinkRate, query.Result.PricingAdjustmentThirtyYear + query.TotalRateAdjustments, query.Result.InterestRateThirtyYear, query.Result.InstalmentThirtyYear, interestOverTermFor30YearTerm);
            }

            var qualificationResult = new QualifyApplicationFor30YearLoanTermResult(loanDetailForCurrentTerm, loanDetailFor30YearTerm, query.Result.QualifiesForThirtyYearLoanTerm,query.Result.PricingAdjustmentThirtyYear, query.Messages);
            return qualificationResult;
        }


        public decimal DetermineNCRGuidelineMinMonthlyFixedExpenses(decimal grossMonthlyIncome)
        {
            var query = new DetermineNCRGuidelineMinMonthlyFixedExpensesQuery(grossMonthlyIncome);
            decisionTreeService.DetermineNCRGuidelineMinMonthlyFixedExpenses(query);
            return query.Result;
        }
    }
}
