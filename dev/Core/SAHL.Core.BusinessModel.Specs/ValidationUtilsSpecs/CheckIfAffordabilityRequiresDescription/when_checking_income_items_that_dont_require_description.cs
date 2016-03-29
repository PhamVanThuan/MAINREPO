using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.BusinessModel.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Core.BusinessModel.Specs.ValidationUtilsSpecs.MapComcorpToSAHLProvince
{
    public class when_checking_income_items_that_dont_require_description : WithFakes
    {
        private static ValidationUtils validationUtils;
        private static List<AffordabilityType> incomeItemsNotRequiringDescription;
        private static List<bool> affordabilityRequiresDescription;

        private Establish context = () =>
        {
            validationUtils = new ValidationUtils();
            incomeItemsNotRequiringDescription = new List<AffordabilityType>()
            {
                AffordabilityType.BasicSalary,
                AffordabilityType.CommissionandOvertime,
                AffordabilityType.Rental
            };

            affordabilityRequiresDescription = new List<bool>();
        };

        private Because of = () =>
        {
            foreach (var affordabilityType in incomeItemsNotRequiringDescription)
            {
                affordabilityRequiresDescription.Add(validationUtils.CheckIfAffordabilityRequiresDescription(affordabilityType));
            } 
        };

        private It should_return_false = () =>
        {
            foreach (var requiresDescription in affordabilityRequiresDescription)
            {
                requiresDescription.ShouldBeFalse();
            }
        };
    }
}