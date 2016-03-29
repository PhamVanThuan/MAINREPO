using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Validation;
using System;
using System.Linq;

namespace SAHL.Core.BusinessModel.Specs.ValidationUtilsSpecs.ValidateIDNumberSpecs
{
    public class when_validating_a_valid_id_number : WithFakes
    {
        private static ValidationUtils utils;
        private static bool result;
        private static string validIDNumber;

        private Establish context = () =>
            {
                utils = new ValidationUtils();
                validIDNumber = "8211045229080";
            };

        private Because of = () =>
            {
                result = utils.ValidateIDNumber(validIDNumber);
            };

        private It should_return_true = () =>
            {
                result.ShouldBeTrue();
            };
    }
}