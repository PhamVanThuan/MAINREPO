using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.BusinessModel.Validation;
using System;
using System.Linq;

namespace SAHL.Core.BusinessModel.Specs.ValidationUtilsSpecs.ParseEnum
{
    public class when_parsing_a_valid_enum : WithFakes
    {
        private static ValidationUtils validationUtils;
        private static RemunerationType result;

        private Establish context = () =>
            {
                validationUtils = new ValidationUtils();
            };

        private Because of = () =>
        {
            result = validationUtils.ParseEnum<RemunerationType>("Salaried");
        };

        private It should = () =>
        {
            result.ShouldEqual(RemunerationType.Salaried);
        };
    }
}