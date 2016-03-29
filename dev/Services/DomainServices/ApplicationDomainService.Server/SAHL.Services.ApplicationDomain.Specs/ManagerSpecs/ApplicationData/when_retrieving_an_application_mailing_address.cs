using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.ApplicationDomain.Managers.Application;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.ApplicationData
{
    public class when_retrieving_an_application_mailing_address : WithFakes
    {
        private static IApplicationDataManager applicationDataManager;
        private static FakeDbFactory dbFactory;
        private static IEnumerable<OfferMailingAddressDataModel> expectedApplicationMailingAddresses;
        private static IEnumerable<OfferMailingAddressDataModel> result;
        private static int applicationNumber;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            applicationDataManager = new ApplicationDataManager(dbFactory);
            applicationNumber = 121;
            expectedApplicationMailingAddresses = new List<OfferMailingAddressDataModel>{new OfferMailingAddressDataModel(applicationNumber,212,true,
                (int)SAHL.Core.BusinessModel.Enums.OnlineStatementFormat.PDFFormat,
                (int)SAHL.Core.BusinessModel.Enums.Language.English,22,(int)SAHL.Core.BusinessModel.Enums.CorrespondenceMedium.Post)};
            dbFactory.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.Select<OfferMailingAddressDataModel>(Param.IsAny<ISqlStatement<OfferMailingAddressDataModel>>()))
                .Return(expectedApplicationMailingAddresses);
        };

        private Because of = () =>
        {
            result = applicationDataManager.GetApplicationMailingAddress(applicationNumber);
        };

        private It should_query_for_application_mailing_address_using_application_number = () =>
        {
            dbFactory.FakedDb.DbReadOnlyContext.WasToldTo(x => x.Select<OfferMailingAddressDataModel>(Param.IsAny<ISqlStatement<OfferMailingAddressDataModel>>()));
        };

        private It should_get_mailing_address_of_application = () =>
        {
            result.First().LegalEntityKey.ShouldEqual(expectedApplicationMailingAddresses.First().LegalEntityKey);
        };
    }
}