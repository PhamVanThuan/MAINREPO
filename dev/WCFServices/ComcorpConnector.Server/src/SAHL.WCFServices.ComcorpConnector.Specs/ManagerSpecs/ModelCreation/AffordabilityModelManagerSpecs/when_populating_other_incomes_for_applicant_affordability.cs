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
    public class when_populating_other_incomes_for_applicant_affordability : WithCoreFakes
    {
        private static AffordabilityModelManager modelManager;
        private static List<IncomeItem> comcorpOtherIncomeItems;
        private static List<IncomeItem> incomeItems;
        private static List<ApplicantAffordabilityModel> otherIncomes;
        private static IValidationUtils validationUtils;

        private Establish context = () =>
        {
            validationUtils = new ValidationUtils();
            modelManager = new AffordabilityModelManager(validationUtils);
            incomeItems = IntegrationServiceTestHelper.PopulateIncomeItems();
            comcorpOtherIncomeItems = (from i in incomeItems where i.IncomeDesc.Contains("Other Income") select i).ToList();
        };

        private Because of = () =>
        {
            otherIncomes = modelManager.PopulateIncomes(incomeItems)
                .Where(x => x.AffordabilityType == AffordabilityType.OtherIncome1 || x.AffordabilityType == AffordabilityType.OtherIncome2).ToList();
        };

        private It should_map_both_of_the_other_incomes = () =>
        {
            otherIncomes.Count.ShouldEqual(2);
        };

        private It should_map_the_otherIncome1_amount = () =>
        {
            otherIncomes.Where(x => x.AffordabilityType == AffordabilityType.OtherIncome1).First().Amount
                .ShouldEqual(comcorpOtherIncomeItems.Where(x => x.IncomeDesc.Equals("Other Income 1")).First().IncomeAmount);
        };

        private It should_map_the_otherIncome2_amount = () =>
        {
            otherIncomes.Where(x => x.AffordabilityType == AffordabilityType.OtherIncome2).First().Amount
                .ShouldEqual(comcorpOtherIncomeItems.Where(x => x.IncomeDesc.Equals("Other Income 2")).First().IncomeAmount);
        };

        private It should_map_the_otherIncome1_description = () =>
        {
            otherIncomes.Where(x => x.AffordabilityType == AffordabilityType.OtherIncome1).First().Description
                .ShouldEqual(comcorpOtherIncomeItems.Where(x => x.IncomeDesc.Equals("Other Income 1")).First().CapturedDescription);
        };

        private It should_map_the_otherIncome2_description = () =>
        {
            otherIncomes.Where(x => x.AffordabilityType == AffordabilityType.OtherIncome2).First().Description
                .ShouldEqual(comcorpOtherIncomeItems.Where(x => x.IncomeDesc.Equals("Other Income 2")).First().CapturedDescription);
        };
    }
}
