using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Testing;
using SAHL.Services.FinancialDomain.Managers;
using SAHL.Services.FinancialDomain.Managers.Models;
using System.Collections.Generic;

namespace SAHL.Services.FinancialDomain.Specs.ManagerSpecs.Financial
{
    public class when_syncing_attributes_given_existing_attribute_to_be_removed :  WithCoreFakes
    {
        private static IFinancialManager financialManager;
        private static IFinancialDataManager financialDataManager;

        private static int applicationNumber;
        private static int offerAttributeTypeToRemove;
        private static IEnumerable<GetOfferAttributesModel> determinedOfferAttributes;

        private Establish context = () =>
        {
            applicationNumber = 1;
            offerAttributeTypeToRemove = 99999;

            determinedOfferAttributes = new List<GetOfferAttributesModel>
            {
                new GetOfferAttributesModel{ OfferAttributeTypeKey = offerAttributeTypeToRemove, Remove = true },
                new GetOfferAttributesModel{ OfferAttributeTypeKey = 12345, Remove = false },
            };

            financialDataManager = An<IFinancialDataManager>();

            financialDataManager.WhenToldTo(x => x.ApplicationHasAttribute(applicationNumber, offerAttributeTypeToRemove))
                                .Return(true);
            
            financialManager = new FinancialManager(financialDataManager);
        };

        private Because of = () =>
        {
            financialManager.SyncApplicationAttributes(applicationNumber, determinedOfferAttributes);
        };

        private It should_delete_the_application_attribute = () =>
        {
            financialDataManager.WasToldTo(x => x.RemoveApplicationAttribute(applicationNumber, offerAttributeTypeToRemove));
        };
    }
}
