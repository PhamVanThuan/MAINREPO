using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.BusinessModel.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Core.BusinessModel.Specs.ValidationUtilsSpecs.MapComcorpToSAHLProvince
{
    public class when_checking_expense_items_that_dont_require_description : WithFakes
    {
        private static ValidationUtils validationUtils;
        private static List<AffordabilityType> expenseItemsNotRequiringDescription;
        private static List<bool> affordabilityRequiresDescription;

        private Establish context = () =>
        {
            validationUtils = new ValidationUtils();
            expenseItemsNotRequiringDescription = new List<AffordabilityType>()
            {
                AffordabilityType.SalaryDeductions,
                AffordabilityType.Foodandgroceries,
                AffordabilityType.BondPayments,
                AffordabilityType.AllCarRepayments,
                AffordabilityType.CreditCard,
                AffordabilityType.Overdraft,
                AffordabilityType.RetailAccounts,
                AffordabilityType.Creditlinerepayment,
                AffordabilityType.PlannedSavings,
                AffordabilityType.Medicalexpenses,
                AffordabilityType.Clothing,
                AffordabilityType.Water_lights_refuseremoval,
                AffordabilityType.Ratesandtaxes,
                AffordabilityType.Transport_petrolcosts,
                AffordabilityType.Insurance_funeralpolicies,
                AffordabilityType.Domesticworkerwage_gardenservices,
                AffordabilityType.Telephone,
                AffordabilityType.Educationfees,
                AffordabilityType.Childsupport,
                AffordabilityType.Rentalrepayment,
                AffordabilityType.Personalloans
            };

            affordabilityRequiresDescription = new List<bool>();
        };

        private Because of = () =>
        {
            foreach (var affordabilityType in expenseItemsNotRequiringDescription)
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