using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.AddressDomain.Managers;
using SAHL.Services.AddressDomain.Managers.Statements;
using SAHL.Services.Interfaces.AddressDomain.Model;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.AddressDomain.Specs.Managers.AddressData
{
    public class when_finding_address_from_free_text_address : WithCoreFakes
    {
        private static AddressDataManager addressDataManager;
        private static FreeTextAddressModel freeTextAddress;
        private static FakeDbFactory dbFactory;
        private static IEnumerable<AddressDataModel> existingAddresses;
        private static IEnumerable<AddressDataModel> results;

        private Establish context = () =>
        {
            freeTextAddress = new FreeTextAddressModel(AddressFormat.FreeText, "42 Wallaby Way", "Sydney", "", "", "", "Australia");
            existingAddresses = new AddressDataModel[] { new AddressDataModel(12, 1, null, null, null, null, null, null, null, null, null, null, null, null, null, null,
                null, null, "42 Wallaby Way", "Sydney", "", "", "") };
            dbFactory = new FakeDbFactory();
            addressDataManager = new AddressDataManager(dbFactory);
            dbFactory.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.Select<AddressDataModel>(Param.IsAny<ISqlStatement<AddressDataModel>>()))
                .Return(existingAddresses);
        };

        private Because of = () =>
        {
            results = addressDataManager.FindAddressFromFreeTextAddress(freeTextAddress);
        };

        private It should_query_for_an_existing_address = () =>
        {
            dbFactory.FakedDb.DbReadOnlyContext.WasToldTo(x => x.Select(Param<FindAddressFromFreeTextAddressStatement>.Matches(m =>
                m.FreeText1 == freeTextAddress.FreeText1 &&
                m.FreeText2 == freeTextAddress.FreeText2 &&
                m.FreeText3 == freeTextAddress.FreeText3 &&
                m.FreeText4 == freeTextAddress.FreeText4 &&
                m.FreeText5 == freeTextAddress.FreeText5
                )));
        };

        private It should_return_the_key_for_the_existing_address = () =>
        {
            results.First().AddressKey.ShouldEqual(existingAddresses.First().AddressKey);
        };
    }
}