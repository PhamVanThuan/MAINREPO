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
    public class when_birthday_has_yet_to_be_celebrated : WithFakes
    {
        private static ValidationUtils utils;
        private static int age;
        private static DateTime dateOfBirth;

        private Establish context = () =>
        {
            dateOfBirth = DateTime.Now.AddYears(-30).AddMonths(+1);
            utils = new ValidationUtils();
        };

        private Because of = () =>
        {
            age = utils.GetAgeFromDateOfBirth(dateOfBirth);
        };

        private It should_return_age_as_twenty_nine = () =>
        {
            age.ShouldEqual(29);
        };


    }
}
