using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.Interfaces.FinancialDomain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.FinancialDomain.Specs.ModelSpecs.PricedMortgageLoanAppInfoModelSpecs
{
    public class when_application_credit_criteria_is_not_provided : WithFakes
    {
        private static PricedMortgageLoanApplicationInformationModel model;
        private static Exception ex;

        private static Decimal LoanAmountNoFees = 1M;
        private static Decimal LoanAgreementAmount = 1M;
        private static Decimal LTV = 0.7M;
        private static RateConfigurationValuesModel RateConfigurationValues = new RateConfigurationValuesModel(1, 1M, 1M);
        private static Decimal MonthlyInstalment = 2M;
        private static Decimal PTI = 0.2M;
        private static PricedCreditCriteriaModel ApplicationCreditCriteria = null;

        private Establish context = () =>
        {
            ex = Catch.Exception(() =>
            {
                model = new PricedMortgageLoanApplicationInformationModel(LoanAgreementAmount, LoanAmountNoFees, LTV, RateConfigurationValues, MonthlyInstalment, PTI, ApplicationCreditCriteria);
            });
        };

        private It should_throw_a_validation_exception = () =>
        {
            ex.ShouldBeOfExactType(typeof(ValidationException));
        };

        private It should_not_construct_the_model = () =>
        {
            model.ShouldBeNull();
        };

        private It should_return_a_message = () =>
        {
            ex.Message.ShouldEqual("ApplicationCreditCriteria must be provided.");
        };
    }
}