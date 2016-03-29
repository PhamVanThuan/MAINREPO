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
    public class when_validating_a_valid_passport_number : WithFakes
    {
        private static ValidationUtils utils;
        private static bool result;
        private static string validPassportNumber;

        private Establish context = () =>
        {
            utils = new ValidationUtils();
            validPassportNumber = "987654321";            
        };

        private Because of = () =>
        {
            result = utils.ValidatePassportNumber(validPassportNumber);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}
