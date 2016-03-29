using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;

namespace SAHL.Services.Interfaces.ApplicationDomain.Specs.ModelSpecs.RefinanceApplicationModelSpecs
{
    public class when_constructing_refinance_application_given_cash_out_is_below_0 : WithFakes
    {
        private static Exception ex;
        private static decimal estimatedPropertyValue;
        private static decimal cashOut;
        private static int term;

        private Establish context = () =>
        {
            estimatedPropertyValue = 1;
            cashOut = -1;
            term = 1;
        };

        private Because of = () =>
        {
            ex = Catch.Exception(() =>
            {
                 new RefinanceApplicationModel(OfferStatus.Open, 1, OriginationSource.SAHomeLoans, estimatedPropertyValue, term, cashOut, Product.NewVariableLoan, "reference1", 1);
            });
        };

        private It should_throw_a_validation_exception = () =>
        {
            ex.ShouldBeOfExactType(typeof(System.ComponentModel.DataAnnotations.ValidationException));
        };
    }
}