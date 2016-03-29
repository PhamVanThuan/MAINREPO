using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Services.Interfaces.ApplicationDomain.Models;

namespace SAHL.Services.Interfaces.ApplicationDomain.Specs.ModelSpecs.SwitchApplicationModelSpecs
{
    public class when_constructing_switch_application_given_estimated_market_value_is_0 : WithFakes
    {
        private static System.Exception ex;

        private Establish context = () =>
        {
        };

        private Because of = () =>
        {
            ex = Catch.Exception(() =>
            {
                new SwitchApplicationModel(OfferStatus.Open, 1, OriginationSource.SAHomeLoans, 0, 0, 0, 0, Product.NewVariableLoan, "reference1", 1);
            });
        };

        private It should_throw_a_validation_exception = () =>
        {
            ex.ShouldBeOfExactType(typeof(System.ComponentModel.DataAnnotations.ValidationException));
        };
    }
}