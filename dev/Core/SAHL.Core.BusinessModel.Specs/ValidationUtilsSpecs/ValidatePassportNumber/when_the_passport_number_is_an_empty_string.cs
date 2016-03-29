using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Core.BusinessModel.Specs.ValidationUtilsSpecs.ValidatePassportNumber
{
    public class when_the_passport_number_is_an_empty_string : WithFakes
    {
        private static ValidationUtils utils;
        private static bool result;
        private static string inValidPassportNumber;

        private Establish context = () =>
        {
            utils = new ValidationUtils();
            inValidPassportNumber = string.Empty;
        };

        private Because of = () =>
        {
            result = utils.ValidatePassportNumber(inValidPassportNumber);
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}
