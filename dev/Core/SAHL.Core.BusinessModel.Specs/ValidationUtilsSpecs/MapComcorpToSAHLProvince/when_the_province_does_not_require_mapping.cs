using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Validation;
using System;
using System.Linq;

namespace SAHL.Core.BusinessModel.Specs.ValidationUtilsSpecs.MapComcorpToSAHLProvince
{
    public class when_the_province_does_not_require_mapping : WithFakes
    {
        private static ValidationUtils validationUtils;
        private static string mappedProvince;
        private static string comcorpProvince;

        private Establish context = () =>
        {
            validationUtils = new ValidationUtils();
            comcorpProvince = "Gauteng";
        };

        private Because of = () =>
        {
            mappedProvince = validationUtils.MapComcorpToSAHLProvince(comcorpProvince);
        };

        private It should_return_value_provided = () =>
        {
            mappedProvince.ShouldEqual("Gauteng");
        };
    }
}