using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.DecisionTree.Models;
using SAHL.V3.Framework.Model;
using SAHL.V3.Framework.Repositories;
using SAHL.V3.Framework.Services;
using SAHL.V3.Framework.Specs.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SAHL.V3.Framework.Specs.RepositorySpecs.DecisionTreeRepositorySpecs
{
    public class BaseSpec : WithFakes
    {
        public static IDecisionTreeService decisionTreeService;
        public static ILegalEntityRepository legalEntityRepo;
        public static ICreditScoringRepository creditScoringRepo;
        public static IEmploymentRepository employmentRepo;
        public static IApplicationRepository applicationRepo;
        public static IDecisionTreeRepository decisionTreeRepo;
        public static IApplication application;
        public static QualifyApplicationFor30YearLoanTermResult expectedResult;
        public static IApplicationProductNewVariableLoan vlInfo;
        public static IApplicationInformationVariableLoan variableLoanInformation;
        public static double? marketRate = 0.061;
        public static double linkRate = 0.021;
        public static double pricingFoRiskAdj = 0.005;
        public static double effectiveRate = 0D;
        public static double? propertyValuation = 1555000;
        public static double? loanAgreementAmount = 1555000;
        public static double? ltv = 0.031;
        public static double? pti = 0.051;
        public static double? monthlyInstalment = 5500.00;
        public static double interestRateThirtyYear = 0.15D;
        public static double pricingAdjustmentThirtyYear = 0.003;
        public static int term = 240;
        public static QualifyApplicationFor30YearLoanTermResult result;
        public static ThirtyYearMortgageLoanEligibility_QueryResult treeResult;
        public static IApplicationInformation applicationInformation;

        public BaseSpec()
        {
            Init();
        }

        public void Init()
        {
            applicationInformation = An<IApplicationInformation>();
            vlInfo = An<IApplicationProductNewVariableLoan>();
            variableLoanInformation = An<IApplicationInformationVariableLoan>();
            //
            application = An<IApplication>();
            application.WhenToldTo(x => x.Key).Return(1);
            //
            variableLoanInformation.WhenToldTo(x => x.MarketRate).Return(marketRate);
            IRateConfiguration rc = An<IRateConfiguration>();
            IMargin margin = An<IMargin>();
            margin.WhenToldTo(x => x.Value).Return(linkRate);
            rc.WhenToldTo(x => x.Margin).Return(margin);
            variableLoanInformation.WhenToldTo(x => x.RateConfiguration).Return(rc);
            variableLoanInformation.WhenToldTo(x => x.PropertyValuation).Return(propertyValuation);
            variableLoanInformation.WhenToldTo(x => x.LoanAgreementAmount).Return(loanAgreementAmount);
            variableLoanInformation.WhenToldTo(x => x.Term).Return(term);
            variableLoanInformation.WhenToldTo(x => x.LTV).Return(ltv);
            variableLoanInformation.WhenToldTo(x => x.PTI).Return(pti);
            variableLoanInformation.WhenToldTo(x => x.MonthlyInstalment).Return(monthlyInstalment);

            vlInfo.WhenToldTo(x => x.VariableLoanInformation).Return(variableLoanInformation);
            application.WhenToldTo(x => x.CurrentProduct).Return(vlInfo);
            application.WhenToldTo(x => x.GetRateAdjustments()).Return(pricingFoRiskAdj);
            var fadj = An<IApplicationInformationFinancialAdjustment>();
            fadj.WhenToldTo(x => x.FromDate).Return(DateTime.Now.AddMinutes(-10));
            fadj.WhenToldTo(x => x.Discount).Return(pricingFoRiskAdj);
            var finadjustments = new StubEventList<IApplicationInformationFinancialAdjustment>();
            finadjustments.Add(fadj);

            applicationInformation.WhenToldTo(x => x.ApplicationInformationFinancialAdjustments).Return(finadjustments);
            application.WhenToldTo(x => x.GetLatestApplicationInformation()).Return(applicationInformation);
            //
            decisionTreeService = An<IDecisionTreeService>();
            legalEntityRepo = An<ILegalEntityRepository>();
            creditScoringRepo = An<ICreditScoringRepository>();
            employmentRepo = An<IEmploymentRepository>();
            applicationRepo = An<IApplicationRepository>();
            decisionTreeRepo = new DecisionTreeRepository(decisionTreeService, applicationRepo, legalEntityRepo, creditScoringRepo, employmentRepo);

            applicationRepo.WhenToldTo(x => x.GetApplicationByKey(Param.IsAny<int>())).Return(application);

            //
            ILegalEntityNaturalPerson le = An<ILegalEntityNaturalPerson>();
            legalEntityRepo.WhenToldTo(x => x.GetLegalEntityByKey(Param.IsAny<int>())).Return(le);
            employmentRepo.WhenToldTo(x => x.DetermineHighestIncomeContributersEmploymentType(Param.IsAny<ILegalEntityNaturalPerson>())).Return(SAHL.Common.Globals.EmploymentTypes.Salaried);

            treeResult = new ThirtyYearMortgageLoanEligibility_QueryResult();
            treeResult.InterestRateThirtyYear = interestRateThirtyYear;
            treeResult.PricingAdjustmentThirtyYear = pricingAdjustmentThirtyYear;
            treeResult.LoantoValueThirtyYear = 0.7;
            treeResult.PaymenttoIncomeThirtyYear = 0.3;
            treeResult.QualifiesForThirtyYearLoanTerm = true;
            treeResult.InstalmentThirtyYear = 3333.00;
            treeResult.TermThirtyYear = 360;

            decisionTreeService.WhenToldTo(x => x.QualifyApplicationFor30YearLoanTerm(Param.IsAny<QualifyApplicationFor30YearLoanTermQuery>()));
        }
    }



}
