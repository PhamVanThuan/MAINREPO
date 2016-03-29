using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.Interfaces.FinancialDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.FinancialDomain.Specs.ModelSpecs.PricedMortgageLoanAppInfoModelSpecs
{
    public class when_all_properties_are_set : WithFakes
    {
        private static PricedMortgageLoanApplicationInformationModel model;
        private static Exception ex;

        private static Decimal LoanAmountNoFees = 1M;
        private static Decimal LoanAgreementAmount = 1M;
        private static Decimal LTV = 0.7M;
        private static RateConfigurationValuesModel rateConfigurationValues = new RateConfigurationValuesModel(1, 1M, 1M);
        private static Decimal MonthlyInstalment = 2M;
        private static Decimal PTI = 0.2M;
        private static PricedCreditCriteriaModel ApplicationCreditCriteria = new PricedCreditCriteriaModel(1, 1, 1);

        private Establish context = () =>
        {
            ex = Catch.Exception(() =>
            {
                model = new PricedMortgageLoanApplicationInformationModel(LoanAgreementAmount, LoanAmountNoFees, LTV, rateConfigurationValues, MonthlyInstalment, PTI, ApplicationCreditCriteria);
            });
        };

        private It should_not_throw_a_validation_exception = () =>
        {
            ex.ShouldBeNull();
        };

        private It should_construct_the_model = () =>
        {
            model.ShouldNotBeNull();
        };
    }
}