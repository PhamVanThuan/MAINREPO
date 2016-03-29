using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.DomainServiceChecks.Managers.ThirdPartyInvoiceDataManager;
using SAHL.DomainServiceChecks.Managers.ThirdPartyInvoiceDataManager.Statements;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.DomainServiceCheck.Specs.Managers.ThirdPartyInvoice
{
    public class when_getting_a_third_party_that_does_exist : WithFakes
    {
        private static FakeDbFactory fakeDb;
        private static ThirdPartyInvoiceDataManager dataManager;
        private static IEnumerable<ThirdPartyDataModel> thirdParty;
        private static Guid thirdPartyId;
        private static bool thirdPartyExists;

        private Establish context = () =>
        {
            thirdPartyId = Guid.NewGuid();
            thirdParty = new List<ThirdPartyDataModel> { new ThirdPartyDataModel(1, true, 1, null, 1, 35) };
            fakeDb = new FakeDbFactory();
            dataManager = new ThirdPartyInvoiceDataManager(fakeDb);
            fakeDb.FakedDb.InReadOnlyAppContext().WhenToldTo(x => x.Select<ThirdPartyDataModel>(Arg.Any<ThirdPartyExistsStatement>()))
                .Return(thirdParty);
        };

        private Because of = () =>
        {
            thirdPartyExists = dataManager.DoesThirdPartyExist(thirdPartyId);
        };

        private It should_return_true = () =>
        {
            thirdPartyExists.ShouldBeTrue();
        };

        private It should_use_the_third_party_id_provided = () =>
        {
            fakeDb.FakedDb.InReadOnlyAppContext().Received().Select(Arg.Is<ThirdPartyExistsStatement>(y => y.ThirdPartyId == thirdPartyId));
        };
    }
}