using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Testing;
using SAHL.DomainProcessManager.Models;
using SAHL.WCFServices.ComcorpConnector.Managers.ModelCreation;
using SAHL.WCFServices.ComcorpConnector.Objects;
using SAHL.WCFServices.ComcorpConnector.Specs.HandlerSpecs.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using SAHL.Core.BusinessModel.Validation;

namespace SAHL.WCFServices.ComcorpConnector.Specs.ManagerSpecs.ModelCreation.AffordabilityModelManagerSpecs
{
    public class when_populating_rental_income_for_applicant_affordability : WithCoreFakes
    {
        private static AffordabilityModelManager modelManager;
        private static IncomeItem comcorpRentalIncomeItem;
        private static List<IncomeItem> incomeItems;
        private static ApplicantAffordabilityModel rentalIncome;
        private static IValidationUtils validationUtils;

        private Establish context = () =>
        {
            validationUtils = new ValidationUtils();
            modelManager = new AffordabilityModelManager(validationUtils);
            incomeItems = IntegrationServiceTestHelper.PopulateIncomeItems();
            comcorpRentalIncomeItem = incomeItems.Where(x => x.IncomeDesc == "Rental").First();
        };

        private Because of = () =>
        {
            rentalIncome = modelManager.PopulateIncomes(incomeItems)
                .Where(x => x.AffordabilityType == AffordabilityType.Rental).FirstOrDefault();
        };

        private It should_have_mapped_the_rental_income_type = () =>
        {
            rentalIncome.ShouldNotBeNull();
        };

        private It should_map_the_rental_income_amount_to_the_IncomeAmount_field = () =>
        {
            rentalIncome.Amount.ShouldEqual(comcorpRentalIncomeItem.IncomeAmount);
        };
    }
}
