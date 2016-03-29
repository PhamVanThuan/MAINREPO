using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.BusinessModel.Validation;

namespace SAHL.Core.BusinessModel.Specs.ValidationUtilsSpecs.ParseEnum
{
    public class when_the_string_contains_a_comma : WithFakes
    {
        private static ValidationUtils validationUtils;
        private static AffordabilityType result;
        private static string stringToParse;

        private Establish context = () =>
        {
            validationUtils = new ValidationUtils();
            stringToParse = "Water, lights, refuse removal";
        };

        private Because of = () =>
        {
            result = validationUtils.ParseEnum<AffordabilityType>(stringToParse);
        };

        private It should_replace_the_forward_comma_with_an_underscore_and_return_a_valid_enum = () =>
        {
            result.ShouldEqual(AffordabilityType.Water_lights_refuseremoval);
        };
    }
}