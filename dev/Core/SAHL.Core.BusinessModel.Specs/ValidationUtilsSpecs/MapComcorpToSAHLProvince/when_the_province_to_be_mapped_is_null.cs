using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Core.BusinessModel.Specs.ValidationUtilsSpecs.MapComcorpToSAHLProvince
{
    public class when_the_province_to_be_mapped_is_null : WithFakes
    {
        private static ValidationUtils validationUtils;
        private static string mappedProvince;
        private static string comcorpProvince;

        private Establish context = () =>
        {
            validationUtils = new ValidationUtils();
            comcorpProvince = null;
        };

        private Because of = () =>
        {
            mappedProvince = validationUtils.MapComcorpToSAHLProvince(comcorpProvince);
        };

        private It should_return_null = () =>
        {
            mappedProvince.ShouldBeNull();
        };
    }
}