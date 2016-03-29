using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Testing;
using SAHL.Services.FinancialDomain.CommandHandlers.Internal;
using SAHL.Services.FinancialDomain.Managers;
using SAHL.Services.FinancialDomain.Managers.Models;
using SAHL.Services.Interfaces.FinancialDomain.Commands.Internal;
using System.Collections.Generic;

namespace SAHL.Services.FinancialDomain.Specs.QueryHandlerSpecs.DetermineSPV
{
    public class when_determining_SPV_given_2_offer_attributes : WithCoreFakes
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

        private static int determinedOfferAttributeTypeKey1 = 99;
        private static int determinedOfferAttributeTypeKey2 = 88;

        private static int determinedOfferAttributeTypeKey3 = 339;

        private static string offerAttributeCSV;

        private static IEnumerable<GetOfferAttributesModel> determinedAttributes;

        private Establish context = () =>
        {
            applicationNumber = 1;
            employmentType = EmploymentType.Salaried;
            householdIncome = 12000;
            isStaffLoan = false;
            isGEPF = false;
            ltv = 0.9m;

            determinedAttributes = new List<GetOfferAttributesModel>(){
                new GetOfferAttributesModel { OfferAttributeTypeKey = determinedOfferAttributeTypeKey1, Remove = false },
                new GetOfferAttributesModel { OfferAttributeTypeKey = determinedOfferAttributeTypeKey2, Remove = false },
                new GetOfferAttributesModel { OfferAttributeTypeKey = determinedOfferAttributeTypeKey3, Remove = true } // discard
            };

            offerAttributeCSV = determinedOfferAttributeTypeKey1 + "," + determinedOfferAttributeTypeKey2;

            financialDataManager = An<IFinancialDataManager>();
            financialDataManager.WhenToldTo(x => x.DetermineApplicationAttributes(applicationNumber, ltv, employmentType, householdIncome, isStaffLoan, isGEPF))
                                .Return(determinedAttributes);

            financialDataManager.WhenToldTo(x => x.GetValidSPV(Param.IsAny<decimal>(), Param.IsAny<string>()))
                                .Return(new GetValidSPVResultModel());

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

        private It should_determine_a_valid_SPV_using_a_comma_separated_list_containing_the_2_offer_attributes = () =>
        {
            financialDataManager.WasToldTo(x => x.GetValidSPV(ltv, offerAttributeCSV));
        };
    }
}