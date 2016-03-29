using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.BusinessModel.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Core.BusinessModel.Specs.ValidationUtilsSpecs.ParseEnum
{
    public class when_parsing_a_mixed_case_enum : WithFakes
    {
        private static ValidationUtils validationUtils;
        private static RemunerationType result;

        private Establish context = () =>
        {
            validationUtils = new ValidationUtils();
        };

        private Because of = () =>
        {
            result = validationUtils.ParseEnum<RemunerationType>("SALaRied");
        };

        private It should_ignore_the_case_when_parsing_an_enum = () =>
        {
            result.ShouldEqual(RemunerationType.Salaried);
        };
    }
}
