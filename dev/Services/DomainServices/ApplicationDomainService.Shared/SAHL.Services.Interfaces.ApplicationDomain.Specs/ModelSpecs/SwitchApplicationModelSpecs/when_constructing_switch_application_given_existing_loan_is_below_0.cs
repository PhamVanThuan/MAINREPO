using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Services.Interfaces.ApplicationDomain.Models;

namespace SAHL.Services.Interfaces.ApplicationDomain.Specs.ModelSpecs.SwitchApplicationModelSpecs
{
    public class when_constructing_switch_application_given_existing_loan_is_below_0 : WithFakes
    {
        private static System.Exception ex;
        private static decimal estimatedPropertyValue;
        private static decimal existingLoan;
        private static decimal cashOut;
        private static int term;

        private Establish context = () =>
        {
            estimatedPropertyValue = 1;
            existingLoan = -1;
            cashOut = 1;
            term = 1;
        };

        private Because of = () =>
        {
            ex = Catch.Exception(() =>
            {
                new SwitchApplicationModel(OfferStatus.Open, 1, OriginationSource.SAHomeLoans, existingLoan, estimatedPropertyValue, term, cashOut, Product.NewVariableLoan, "reference1", 1);
            });
        };

        private It should_throw_a_validation_exception = () =>
        {
            ex.ShouldBeOfExactType(typeof(System.ComponentModel.DataAnnotations.ValidationException));
        };
    }
}