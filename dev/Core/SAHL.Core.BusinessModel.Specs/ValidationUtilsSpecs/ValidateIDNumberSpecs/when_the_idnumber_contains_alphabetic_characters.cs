using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Core.BusinessModel.Specs.ValidationUtilsSpecs.ValidateIDNumberSpecs
{
    public class when_the_idnumber_contains_alphabetic_characters : WithFakes
    {
        private static ValidationUtils utils;
        private static bool result;
        private static string invalidIdNumber;

        private Establish context = () =>
        {
            utils = new ValidationUtils();
            invalidIdNumber = "8211O45229O8O";
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
