using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Testing;
using SAHL.Services.FinancialDomain.CommandHandlers.Internal;
using SAHL.Services.FinancialDomain.Managers;
using SAHL.Services.FinancialDomain.Managers.Models;
using SAHL.Services.Interfaces.FinancialDomain.Commands.Internal;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.FinancialDomain.Specs.QueryHandlerSpecs.DetermineSPV
{
    public class when_determining_SPV_and_it_fails_with_messages : WithCoreFakes
    {
        private static DetermineSPVQuery command;
        private static DetermineSPVQueryHandler handler;
        private static IFinancialDataManager financialDataManager;

        private static int applicationNumber;
        private static EmploymentType employmentType;
        private static decimal householdIncome;
        private static bool isStaffLoan;
        private static bool isGEPF;
        private static decimal ltv;

        private static IEnumerable<GetOfferAttributesModel> determinedAttributes;

        private static string errorMessage = "Unable to determine an SPV for this Application";
        private static GetValidSPVResultModel getValidSPVResult;

        private Establish context = () =>
        {
            applicationNumber = 1;
            employmentType = EmploymentType.Salaried;
            householdIncome = 12000;
            isStaffLoan = false;
            isGEPF = false;
            ltv = 0.9m;

            determinedAttributes = new List<GetOfferAttributesModel>() { };

            financialDataManager = An<IFinancialDataManager>();
            financialDataManager.WhenToldTo(x => x.DetermineApplicationAttributes(applicationNumber, ltv, employmentType, householdIncome, isStaffLoan, isGEPF))
                                .Return(determinedAttributes);

            getValidSPVResult = new GetValidSPVResultModel
            {
                Message = errorMessage
            };

            financialDataManager.WhenToldTo(x => x.GetValidSPV(Param.IsAny<decimal>(), Param.IsAny<string>()))
                                .Return(getValidSPVResult);

            command = new DetermineSPVQuery(applicationNumber, employmentType, householdIncome, isStaffLoan, ltv, isGEPF);
            handler = new DetermineSPVQueryHandler(financialDataManager);
        };

        private Because of = () =>
        {
            messages = handler.HandleQuery(command);
        };

        private It should_determine_application_attributes = () =>
        {
            financialDataManager.WasToldTo(x => x.DetermineApplicationAttributes(applicationNumber, ltv, employmentType, householdIncome, isStaffLoan, isGEPF));
        };

        private It should_determine_a_valid_SPV = () =>
        {
            financialDataManager.WasToldTo(x => x.GetValidSPV(ltv, ""));
        };

        private It should_contain_an_error_message = () =>
        {
            messages.ErrorMessages().First().Message.ShouldEqual(errorMessage);
            messages.ErrorMessages().Count().ShouldEqual(1);
        };
    }
}