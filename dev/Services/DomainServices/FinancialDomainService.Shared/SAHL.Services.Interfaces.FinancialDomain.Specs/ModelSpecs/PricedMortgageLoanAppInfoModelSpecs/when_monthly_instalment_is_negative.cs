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
    public class when_monthly_instalment_is_negative : WithFakes
    {
        private static PricedMortgageLoanApplicationInformationModel model;
        private static Exception ex;

        private static Decimal LoanAmountNoFees = 1M;
        private static Decimal LoanAgreementAmount = 1M;
        private static Decimal LTV = 1M;
        private static RateConfigurationValuesModel RateConfigurationValues = new RateConfigurationValuesModel(1, 1M, 1M);
        private static Decimal MonthlyInstalment = -1M;
        private static Decimal PTI = 0.2M;
        private static PricedCreditCriteriaModel ApplicationCreditCriteria = new PricedCreditCriteriaModel(1, 1, 1);

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
            ex.Message.ShouldEqual("MonthlyInstalment must be greater than 0.");
        };
    }
}