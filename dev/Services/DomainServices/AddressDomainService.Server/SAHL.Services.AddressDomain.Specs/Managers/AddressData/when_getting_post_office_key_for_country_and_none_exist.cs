using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.AddressDomain.Managers;
using SAHL.Services.AddressDomain.Managers.Statements;

namespace SAHL.Services.AddressDomain.Specs.Managers.AddressData
{
    public class when_getting_post_office_key_for_country_and_none_exist : WithCoreFakes
    {
        private static AddressDataManager addressDataManager;
        private static FakeDbFactory dbFactory;
        private static int? result;
        private static string country;

        private Establish context = () =>
        {
            country = "Australia";
            dbFactory = new FakeDbFactory();
            addressDataManager = new AddressDataManager(dbFactory);
        };

        private Because of = () =>
        {
            result = addressDataManager.GetPostOfficeKeyForCountry(country);
        };

        private It should_query_for_an_existing_address = () =>
        {
            dbFactory.FakedDb.DbReadOnlyContext.WasToldTo(x => x.SelectOne(Param<GetPostOfficeKeyForCountryStatement>.Matches(m =>
                m.Country == country
                )));
        };

        private It should_return_null_for_the_post_office_key = () =>
        {
            result.ShouldBeNull();
        };
    }
}