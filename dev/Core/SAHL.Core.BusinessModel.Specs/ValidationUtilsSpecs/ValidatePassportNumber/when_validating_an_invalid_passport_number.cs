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
    public class when_validating_an_invalid_passport_number : WithFakes
    {
        private static ValidationUtils utils;
        private static bool result;
        private static string inValidPassportNumber;

        private Establish context = () =>
        {
            utils = new ValidationUtils();
            inValidPassportNumber = "9876";
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