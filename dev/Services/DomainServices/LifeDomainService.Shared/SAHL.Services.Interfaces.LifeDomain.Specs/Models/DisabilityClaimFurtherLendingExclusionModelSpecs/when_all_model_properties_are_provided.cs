using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Services.Interfaces.LifeDomain.Models;
using System;

namespace SAHL.Services.Interfaces.LifeDomain.Specs.Models.DisabilityClaimFurtherLendingExclusionModelSpecs
{
    public class when_all_model_properties_are_provided : WithFakes
    {
        private static DisabilityClaimFurtherLendingExclusionModel model;
        private static string description;
        private static DateTime dateDisbursed;
        private static double amountIncludingFees;
        private static Exception ex;

        private Establish context = () =>
        {
            description = "Disability claim";
            dateDisbursed = DateTime.Today;
            amountIncludingFees = 10000;
        };

        private Because of = () =>
        {
            ex = Catch.Exception(() =>
            {
                model = new DisabilityClaimFurtherLendingExclusionModel(Param.IsAny<int>(), description, dateDisbursed, amountIncludingFees);
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