using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.Interfaces.Query.DataManagers;
using SAHL.Services.Interfaces.Query.Helpers;
using SAHL.Services.Interfaces.Query.Models;
using SAHL.Services.Interfaces.Query.Parsers;
using SAHL.Services.Query.Controllers;
using SAHL.Services.Query.Controllers.Lookup;
using SAHL.Services.Query.Factories;
using SAHL.Services.Query.Helper;
using SAHL.Services.Query.Interfaces;
using SAHL.Services.Query.Parsers;
using SAHL.Services.Query.Parsers.Elemets;
using SAHL.Services.Query.Serialiser;
using SAHL.Services.Query.Server.Specs.Factory;
using SAHL.Services.Query.Validators;

namespace SAHL.Services.Query.Server.Specs.Controllers.Lookups
{
    public class generic_lookup_controller_handles_an_invalid_get_with_id_request : WithFakes
    {

        private static LookupController lookupController;
        private static ILookupDataManager lookupDataManager;
        private static ILookupTypesHelper lookupTypesHelper;
        private static string lookupType;
        private static object exception;
        private static ILookupMetaDataModel lookupMetaDataModel;
        private static ILookupRepresentationHelper lookupRepresentationHelper;
        private static IQueryFactory queryFactory;
        public static IQueryStringQueryParser queryStringQueryParser;
        public static IJsonQueryParser JsonQueryParser;
        private static IFindQuery findManyQuery;
        public static IPagingParser pagingParser;
        public static ILinkResolver linkResolver;
        private static IFindQueryValidator findQueryValidator;

        Establish that = () =>
        {
            lookupType = "GenericKeyType";
            lookupDataManager = An<ILookupDataManager>();
            lookupTypesHelper = An<ILookupTypesHelper>();
            linkResolver = An<ILinkResolver>();

            queryStringQueryParser = new QueryStringQueryParser();
            JsonQueryParser = new JsonQueryParser();
            pagingParser = new PagingParser();
            findQueryValidator= new FindQueryValidator();

            queryFactory = new QueryFactory(findQueryValidator, queryStringQueryParser, JsonQueryParser, pagingParser);
            findManyQuery = new FindManyQuery();

            lookupMetaDataModel = LookupMetaDataFactory.GetGenericKeyMetaDataModel();
            lookupRepresentationHelper = new LookupRepresentationHelper(linkResolver, lookupTypesHelper, lookupDataManager);

            lookupTypesHelper.WhenToldTo(x => x.IsValidLookupType(lookupType)).Return(false);
            lookupTypesHelper.WhenToldTo(x => x.FindLookupMetaData(lookupType)).Return(lookupMetaDataModel);

            halSerialiser = An<IHalSerialiser>();

            lookupController = new LookupController(lookupRepresentationHelper, lookupTypesHelper, queryFactory, halSerialiser);
            lookupController.Request = HttpRequestTestDataFactory.GetEmptyWhereHttpRequest();

        };

        private Because of = () =>
        {
            exception = Catch.Exception(() => lookupController.Get(lookupType, 1));
        };

        private It should_route_the_filter_to_the_query_parser_factory = () =>
        {
            queryFactory.WhenToldTo(x => x.CreateFindManyQuery(HttpRequestTestDataFactory.GetSimpleWhereFilter()))
                .Equals(FindQueryFactory.Query());
        };

        private It should_get_find_many_query_item_from_the_querystringparser = () =>
        {
            queryStringQueryParser.WhenToldTo(x => x.FindManyQuery(HttpRequestTestDataFactory.GetSimpleWhereFilter())).Equals(FindQueryFactory.Query());
        };
        
        private It should_validate_that_the_lookup_type_is_supported = () =>
        {
            lookupTypesHelper.WasToldTo(x => x.IsValidLookupType(lookupType));
        };

        private It should_get_lookup_type_meta_data = () =>
        {
            lookupTypesHelper.WasNotToldTo(x => x.FindLookupMetaData(lookupType));
        };

        private It should_call_the_database = () =>
        {
            lookupDataManager.WasNotToldTo(
                x => x.GetLookup(findManyQuery, "2AM", "dbo", "GenericKeyType", "GenericKeyTypeKey", "Description", 1));
        };

        private static IHalSerialiser halSerialiser;
    }
}
