using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData.Statements;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.FinanceDomain.Specs.Managers.ThirdPartyInvoiceData
{
    public class when_getting_user_capabilities_by_org_str_Key : WithCoreFakes
    {
        private static IThirdPartyInvoiceDataManager dataManager;
        private static FakeDbFactory dbFactory;
        private static int userOrgStructureKey;
        private static List<string> expectedCapabilities;
        private static List<string> results;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            dataManager = new ThirdPartyInvoiceDataManager(dbFactory);
            userOrgStructureKey = 1001;
            expectedCapabilities = new List<string>() { "Invoice Processor", "Invoice Approver", "Loss Control Fee Consultant", "Loss Control Fee Invoice Approver Under R15000" };
            dbFactory.FakedDb.InReadOnlyAppContext()
                .WhenToldTo(x => x.Select(Param<GetUserCapabilitiesByUserOrgStructureKeyStatement>
                    .Matches(y => y.UserOrganisationStructureKey == userOrgStructureKey))).Return(expectedCapabilities);
        };

        private Because of = () =>
        {
            results = dataManager.GetUserCapabilitiesByUserOrgStructureKey(userOrgStructureKey);
        };

        private It should_retrieve_capabilities = () =>
        {
            dbFactory.FakedDb.InReadOnlyAppContext()
                .WasToldTo(x => x.Select(Param<GetUserCapabilitiesByUserOrgStructureKeyStatement>
                    .Matches(y => y.UserOrganisationStructureKey == userOrgStructureKey)));
        };

        private It should_return_capabilities = () =>
        {
            results.All(expectedCapabilities.Contains).ShouldBeTrue();
        };
    }
}