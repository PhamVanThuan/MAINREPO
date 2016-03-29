using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.ApplicationDomain.Specs.ModelSpecs.SwitchApplicationModelSpecs
{
    public class when_constructing_affordability_given_amount_is_0 : WithFakes
    {
        private static Exception ex;
        private static double expectedAmount;
        private static AffordabilityTypeModel model;
        private Establish context = () =>
        {
            expectedAmount = 0;
        };

        private Because of = () =>
        {
            ex = Catch.Exception(() =>
            {
                model = new AffordabilityTypeModel(AffordabilityType.BasicSalary, expectedAmount, string.Empty);
            });
        };

        private It should_throw_a_validation_exception = () =>
        {
            ex.ShouldNotBeNull();
            ex.ShouldBeOfExactType(typeof(ValidationException));
        };

    }
}
