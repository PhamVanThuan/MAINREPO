using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.DecisionTree.Models;
using SAHL.V3.Framework.Model;
using SAHL.V3.Framework.Repositories;
using SAHL.V3.Framework.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.V3.Framework.Specs.RepositorySpecs.DecisionTreeRepositorySpecs
{

    public class when_asked_to_qualify_a_mortgage_loan_application_for_30_year_term : BaseSpec
    {

        Establish context = () =>
        {
            decisionTreeService.WhenToldTo<IDecisionTreeService>(x => x.QualifyApplicationFor30YearLoanTerm(Param.IsAny<QualifyApplicationFor30YearLoanTermQuery>())).Callback<QualifyApplicationFor30YearLoanTermQuery>(y =>
            {
                y.SetTreeResult(treeResult, SystemMessageCollection.Empty());
            });
        };

        Because of = () =>
        {
            result = decisionTreeRepo.QualifyApplication(application.Key);
        };

        It should_return_a_valid_result = () =>
        {
            result.ShouldNotBeNull();
        };

        It should_contain_no_messages = () =>
        {
            result.Messages.AllMessages.Count().ShouldEqual<int>(0);
        };

        It should_allow_a_30_year_term_conversion = () =>
        {
            result.QualifiesForThirtyYearTerm.ShouldBeTrue();
        };

        It should_populate_the_current_loan_detail = () =>
        {
            result.LoanDetailForCurrentTerm.ShouldNotBeNull();
            result.LoanDetailForCurrentTerm.LoanAgreementAmount.ShouldEqual(variableLoanInformation.LoanAgreementAmount);
            result.LoanDetailForCurrentTerm.Term.ShouldEqual(variableLoanInformation.Term);
            result.LoanDetailForCurrentTerm.MarketRate.ShouldEqual(marketRate);
            result.LoanDetailForCurrentTerm.LTV.ShouldEqual(variableLoanInformation.LTV);
            result.LoanDetailForCurrentTerm.PTI.ShouldEqual(variableLoanInformation.PTI);
            result.LoanDetailForCurrentTerm.Instalment.ShouldEqual(variableLoanInformation.MonthlyInstalment);
        };

        It should_populate_the_30_year_loan_detail = () =>
        {
            result.LoanDetailFor30YearTerm.ShouldNotBeNull();
            result.LoanDetailFor30YearTerm.LoanAgreementAmount.ShouldEqual(variableLoanInformation.LoanAgreementAmount);
            result.LoanDetailFor30YearTerm.Term.ShouldEqual(360);
            result.LoanDetailFor30YearTerm.MarketRate.ShouldEqual(variableLoanInformation.MarketRate);
            result.LoanDetailFor30YearTerm.LTV.ShouldEqual(treeResult.LoantoValueThirtyYear);
            result.LoanDetailFor30YearTerm.PTI.ShouldEqual(treeResult.PaymenttoIncomeThirtyYear);
            result.LoanDetailFor30YearTerm.PricingForRiskAdjustment.ShouldEqual(pricingAdjustmentThirtyYear + pricingFoRiskAdj);
            result.LoanDetailFor30YearTerm.Instalment.ShouldEqual(treeResult.InstalmentThirtyYear);
        };

        It should_populate_pricingAdjustmentThirtyYear = () =>
        {
            result.PricingAdjustmentThirtyYear.ShouldEqual(pricingAdjustmentThirtyYear);
        };

    }

}
