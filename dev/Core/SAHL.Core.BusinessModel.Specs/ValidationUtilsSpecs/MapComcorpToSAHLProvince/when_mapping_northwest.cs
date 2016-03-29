using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Validation;
using System;
using System.Linq;

namespace SAHL.Core.BusinessModel.Specs.ValidationUtilsSpecs.MapComcorpToSAHLProvince
{
    public class when_mapping_northwest : WithFakes
    {
        private static ValidationUtils validationUtils;
        private static string mappedProvince;
        private static string comcorpProvince;

        private Establish context = () =>
        {
            validationUtils = new ValidationUtils();
            comcorpProvince = "Northwest";
        };

        private Because of = () =>
        {
            mappedProvince = validationUtils.MapComcorpToSAHLProvince(comcorpProvince);
        };

        private It should_return_the_SAHL_province = () =>
        {
            mappedProvince.ShouldEqual("North West");
        };
    }
}