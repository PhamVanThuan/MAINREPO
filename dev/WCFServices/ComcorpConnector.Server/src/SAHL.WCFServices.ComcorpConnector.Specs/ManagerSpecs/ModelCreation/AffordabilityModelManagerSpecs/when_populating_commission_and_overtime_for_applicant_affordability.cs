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
    public class when_populating_commission_and_overtime_for_applicant_affordability : WithCoreFakes
    {
        private static AffordabilityModelManager modelManager;
        private static IncomeItem comcorpCommissionAndOvertimeItem;
        private static List<IncomeItem> incomeItems;
        private static ApplicantAffordabilityModel commissionAndOvertime;
        private static IValidationUtils validationUtils;

        private Establish context = () =>
        {
            validationUtils = new ValidationUtils();
            modelManager = new AffordabilityModelManager(validationUtils);
            incomeItems = IntegrationServiceTestHelper.PopulateIncomeItems();
            comcorpCommissionAndOvertimeItem = incomeItems.Where(x => x.IncomeDesc == "Commission and Overtime").First();
        };

        private Because of = () =>
        {
            commissionAndOvertime = modelManager.PopulateIncomes(incomeItems)
                .Where(x => x.AffordabilityType == AffordabilityType.CommissionandOvertime).FirstOrDefault();
        };

        private It should_have_mapped_the_commission_and_overtime_income = () =>
        {
            commissionAndOvertime.ShouldNotBeNull();
        };

        private It should_map_the_commission_and_overtime_amount_to_the_IncomeAmount_field = () =>
        {
            commissionAndOvertime.Amount.ShouldEqual(comcorpCommissionAndOvertimeItem.IncomeAmount);
        };
    }
}
