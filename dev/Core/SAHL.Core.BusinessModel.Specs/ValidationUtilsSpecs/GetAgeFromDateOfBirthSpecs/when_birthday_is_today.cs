using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Core.BusinessModel.Specs.ValidationUtilsSpecs.GetAgeFromDateOfBirthSpecs
{
    public class when_birthday_is_today : WithFakes
    {
        private const int YearOfBirth = 1982;

        private static ValidationUtils utils;
        private static int age;
        private static DateTime dateOfBirth;
        private static int expectedAge;

        private Establish context = () =>
        {
            var month = DateTime.Today.Month;
            var day = DateTime.Today.Day;
            dateOfBirth = new DateTime(YearOfBirth, month, day);
            utils = new ValidationUtils();

            expectedAge = DateTime.Now.Year - YearOfBirth;
        };

        private Because of = () =>
        {
            age = utils.GetAgeFromDateOfBirth(dateOfBirth);
        };

        private It should_return_age_as_thirty = () =>
        {
            age.ShouldEqual(expectedAge);
        };
    }
}
