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
    public class when_populating_basic_salary_for_applicant_affordability : WithCoreFakes
    {
        private static AffordabilityModelManager modelManager;
        private static IncomeItem comcorpBasicSalaryItem;
        private static List<IncomeItem> incomeItems;
        private static ApplicantAffordabilityModel basicSalary;
        private static IValidationUtils validationUtils;

        private Establish context = () =>
        {
            validationUtils = new ValidationUtils();
            modelManager = new AffordabilityModelManager(validationUtils);
            incomeItems = IntegrationServiceTestHelper.PopulateIncomeItems();
            comcorpBasicSalaryItem = incomeItems.Where(x => x.IncomeDesc == "Basic Salary").First();
        };

        private Because of = () =>
        {
            basicSalary = modelManager.PopulateIncomes(incomeItems)
                .Where(x => x.AffordabilityType == AffordabilityType.BasicSalary).FirstOrDefault();
        };

        private It should_have_mapped_the_basic_salary = () =>
        {
            basicSalary.ShouldNotBeNull();
        };

        private It should_map_the_basic_salary_amount_to_the_IncomeAmount_field = () =>
        {
            basicSalary.Amount.ShouldEqual(comcorpBasicSalaryItem.IncomeAmount);
        };
    }
}
