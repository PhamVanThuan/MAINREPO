using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.BusinessModel.Validation;
using System;
using System.Linq;

namespace SAHL.Core.BusinessModel.Specs.ValidationUtilsSpecs.ParseEnum
{
    public class when_the_string_starts_with_an_underscore : WithFakes
    {
        private static ValidationUtils validationUtils;
        private static string value;
        private static RemunerationType enumResult;

        private Establish context = () =>
        {
            value = "_Salaried";
            validationUtils = new ValidationUtils();
        };

        private Because of = () =>
        {
            enumResult = validationUtils.ParseEnum<RemunerationType>(value);
        };

        private It should_still_find_a_valid_match = () =>
        {
            enumResult.ShouldEqual(RemunerationType.Salaried);
        };
    }
}