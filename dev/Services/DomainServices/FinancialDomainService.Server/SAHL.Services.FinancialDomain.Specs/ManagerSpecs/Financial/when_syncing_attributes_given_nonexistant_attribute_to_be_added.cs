using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Testing;
using SAHL.Services.FinancialDomain.Managers;
using SAHL.Services.FinancialDomain.Managers.Models;
using System.Collections.Generic;

namespace SAHL.Services.FinancialDomain.Specs.ManagerSpecs.Financial
{
    public class when_syncing_attributes_given_nonexistant_attribute_to_be_added : WithCoreFakes
    {
        private static IFinancialManager financialManager;
        private static IFinancialDataManager financialDataManager;

        private static int applicationNumber;
        private static int offerAttributeTypeToAdd;
        private static IEnumerable<GetOfferAttributesModel> determinedOfferAttributes;

        private Establish context = () =>
        {
            applicationNumber = 1;
            offerAttributeTypeToAdd = 123456;

            determinedOfferAttributes = new List<GetOfferAttributesModel>
            {
                new GetOfferAttributesModel{ OfferAttributeTypeKey = offerAttributeTypeToAdd, Remove = false },
                new GetOfferAttributesModel{ OfferAttributeTypeKey = 9999, Remove = true },
            };

            financialDataManager = An<IFinancialDataManager>();

            financialDataManager.WhenToldTo(x => x.ApplicationHasAttribute(applicationNumber, offerAttributeTypeToAdd))
                                .Return(false);

            financialManager = new FinancialManager(financialDataManager);
        };

        private Because of = () =>
        {
            financialManager.SyncApplicationAttributes(applicationNumber, determinedOfferAttributes);
        };

        private It should_add_the_application_attribute = () =>
        {
            financialDataManager.WasToldTo(x => x.AddApplicationAttribute(applicationNumber, offerAttributeTypeToAdd));
        };
    }
}
