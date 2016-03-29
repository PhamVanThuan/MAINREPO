using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Validation;
using System;
using System.Linq;

namespace SAHL.Core.BusinessModel.Specs.ValidationUtilsSpecs.GetAgeFromDateOfBirthSpecs
{
    public class when_birthday_has_been_celebrated : WithFakes
    {
        private static ValidationUtils utils;
        private static int age;
        private static DateTime dateOfBirth;

        private Establish context = () =>
        {
            dateOfBirth = DateTime.Now.AddYears(-30).AddDays(-1);
            utils = new ValidationUtils();
        };

        private Because of = () =>
        {
            age = utils.GetAgeFromDateOfBirth(dateOfBirth);
        };

        private It should_return_age_as_thirty = () =>
        {
            age.ShouldEqual(30);
        };
    }
}