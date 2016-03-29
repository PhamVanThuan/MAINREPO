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
    public class when_populating_income_item_with_no_captured_description : WithCoreFakes
    {
        private static AffordabilityModelManager modelManager;
        private static List<IncomeItem> incomeItems;
        private static ApplicantAffordabilityModel incomeFromInvestments;
        private static ApplicantAffordabilityModel otherIncome1;
        private static ApplicantAffordabilityModel otherIncome2;
        private static IValidationUtils validationUtils;
        private static string expectedDescription = "Unknown";

        private Establish context = () =>
        {
            validationUtils = new ValidationUtils();
            modelManager = new AffordabilityModelManager(validationUtils);
            incomeItems = new List<IncomeItem>() { 
                new IncomeItem(){IncomeType = 1, IncomeDesc="Income from Investments" , IncomeAmount = 1000, CapturedDescription = ""},
                new IncomeItem(){IncomeType = 1, IncomeDesc="Other Income 1" , IncomeAmount = 4444, CapturedDescription = " "},
                new IncomeItem(){IncomeType = 1, IncomeDesc="Other Income 2" , IncomeAmount = 5555, CapturedDescription = null},
            };
        };

        private Because of = () =>
        {
            incomeFromInvestments = modelManager.PopulateIncomes(incomeItems).Where(x => x.AffordabilityType == AffordabilityType.IncomefromInvestments).FirstOrDefault();
            otherIncome1 = modelManager.PopulateIncomes(incomeItems).Where(x => x.AffordabilityType == AffordabilityType.OtherIncome1).FirstOrDefault();
            otherIncome2 = modelManager.PopulateIncomes(incomeItems).Where(x => x.AffordabilityType == AffordabilityType.OtherIncome2).FirstOrDefault();
        };

        private It should_default_the_uncaptured_description_to_Unknown = () =>
        {
            incomeFromInvestments.Description.ShouldEqual(expectedDescription);
            otherIncome1.Description.ShouldEqual(expectedDescription);
            otherIncome2.Description.ShouldEqual(expectedDescription);
        };
    }
}
