using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Validation;
using System;
using System.Linq;

namespace SAHL.Core.BusinessModel.Specs.ValidationUtilsSpecs.ValidateIDNumberSpecs
{
    public class when_the_id_number_is_an_empty_string : WithFakes
    {
        private static ValidationUtils utils;
        private static bool result;
        private static string invalidIdNumber;

        private Establish context = () =>
        {
            utils = new ValidationUtils();
            invalidIdNumber = string.Empty;
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