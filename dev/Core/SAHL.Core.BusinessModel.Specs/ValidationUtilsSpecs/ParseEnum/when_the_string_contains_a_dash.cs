using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.BusinessModel.Validation;

namespace SAHL.Core.BusinessModel.Specs.ValidationUtilsSpecs.ParseEnum
{
    public class when_the_string_contains_a_dash : WithFakes
    {
        private static ValidationUtils validationUtils;
        private static CitizenType result;
        private static string stringToParse;

        private Establish context = () =>
        {
            validationUtils = new ValidationUtils();
            stringToParse = "SA Citizen – Non Resident";
        };

        private Because of = () =>
        {
            result = validationUtils.ParseEnum<CitizenType>(stringToParse);
        };

        private It should_replace_the_forward_slash_with_an_underscore_and_return_a_valid_enum = () =>
        {
            result.ShouldEqual(CitizenType.SACitizen_NonResident);
        };
    }
}