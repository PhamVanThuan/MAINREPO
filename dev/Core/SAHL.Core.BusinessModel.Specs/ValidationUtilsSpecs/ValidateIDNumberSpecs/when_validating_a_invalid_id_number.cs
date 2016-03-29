using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Validation;
using System;
using System.Linq;

namespace SAHL.Core.BusinessModel.Specs.ValidationUtilsSpecs.ValidateIDNumberSpecs
{
    public class when_validating_an_invalid_id_number : WithFakes
    {
        private static ValidationUtils utils;
        private static bool result;
        private static string invalidIdNumber;

        private Establish context = () =>
            {
                utils = new ValidationUtils();
                invalidIdNumber = "8211045229000";
            };

        private Because of = () =>
            {
                result = utils.ValidateIDNumber(invalidIdNumber);
            };

        private It should_return_false = () =>
            {
                result.ShouldBeFalse();
            };
    }
}