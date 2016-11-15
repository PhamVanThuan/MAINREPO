﻿using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.BusinessModel.Validation;
using System;
using System.Linq;

namespace SAHL.Core.BusinessModel.Specs.ValidationUtilsSpecs.MapComcorpToSAHLProvince
{
    public class when_checking_expense_item_other_debt_repayment : WithFakes
    {
        private static ValidationUtils validationUtils;
        private static AffordabilityType affordabilityType;
        private static bool affordabilityRequiresDescription;

        private Establish context = () =>
        {
            validationUtils = new ValidationUtils();
            affordabilityType = AffordabilityType.Otherdebtrepayment;
        };

        private Because of = () =>
        {
            affordabilityRequiresDescription = validationUtils.CheckIfAffordabilityRequiresDescription(affordabilityType);
        };

        private It should_return_true = () =>
        {
            affordabilityRequiresDescription.ShouldBeTrue();
        };
    }
}