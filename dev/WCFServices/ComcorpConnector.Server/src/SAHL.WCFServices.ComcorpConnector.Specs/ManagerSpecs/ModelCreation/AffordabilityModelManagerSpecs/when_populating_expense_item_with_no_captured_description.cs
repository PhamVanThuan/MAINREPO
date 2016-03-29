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
    public class when_populating_expense_item_with_no_captured_description : WithCoreFakes
    {
        private static AffordabilityModelManager modelManager;
        private static List<ExpenditureItem> expenditureItems;
        private static ApplicantAffordabilityModel otherExpenditure;
        private static ApplicantAffordabilityModel otherInstalments;
        private static ApplicantAffordabilityModel otherDebtRepayment;
        private static IValidationUtils validationUtils;
        private static string expectedDescription = "Unknown";

        private Establish context = () =>
        {
            validationUtils = new ValidationUtils();
            modelManager = new AffordabilityModelManager(validationUtils);
            expenditureItems = new List<ExpenditureItem>() { 
                new ExpenditureItem(){ExpenditureType = 1, ExpenditureDesc="Other", ExpenditureAmount = 200, CapturedDescription = ""},
                new ExpenditureItem(){ExpenditureType = 1, ExpenditureDesc="Other Instalments", ExpenditureAmount = 200, CapturedDescription = " "},
                new ExpenditureItem(){ExpenditureType = 1, ExpenditureDesc="Other Debt Repayment", ExpenditureAmount = 500, CapturedDescription = null},
            };
        };

        private Because of = () =>
        {
            otherExpenditure = modelManager.PopulateExpenses(expenditureItems).Where(x => x.AffordabilityType == AffordabilityType.Other).FirstOrDefault();
            otherInstalments = modelManager.PopulateExpenses(expenditureItems).Where(x => x.AffordabilityType == AffordabilityType.OtherInstalments).FirstOrDefault();
            otherDebtRepayment = modelManager.PopulateExpenses(expenditureItems).Where(x => x.AffordabilityType == AffordabilityType.Otherdebtrepayment).FirstOrDefault();
        };

        private It should_default_the_uncaptured_description_to_Unknown = () =>
        {
            otherExpenditure.Description.ShouldEqual(expectedDescription);
            otherInstalments.Description.ShouldEqual(expectedDescription);
            otherDebtRepayment.Description.ShouldEqual(expectedDescription);
        };
    }
}
