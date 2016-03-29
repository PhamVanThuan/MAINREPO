using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
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
    public class generic_lookup_controller_handles_a_valid_get_request : WithFakes
    {

        private static LookupController lookupController;
        private static ILookupDataManager lookupDataManager;
        private static ILookupTypesHelper lookupTypesHelper;
        private static string lookupType;
        private static ILookupMetaDataModel lookupMetaDataModel;
        private static ILookupRepresentationHelper lookupRepresentationHelper;
        private static IFindQuery FindManyQuery;
        private static IQueryFactory queryFactory;
        public static IQueryStringQueryParser queryStringQueryParser;
        public static IJsonQueryParser jsonQueryParser;
        public static IPagingParser pagingParser;
        public static ILinkResolver linkResolver;
        private static IFindQueryValidator findQueryValidator;

        Establish that = () =>
        {
            lookupType = "generickeytype";
            lookupDataManager = An<ILookupDataManager>();
            lookupTypesHelper = An<ILookupTypesHelper>();
            linkResolver = An<ILinkResolver>();
            lookupRepresentationHelper = new LookupRepresentationHelper(linkResolver,lookupTypesHelper, lookupDataManager);
            FindManyQuery = new FindManyQuery();

            queryStringQueryParser = new QueryStringQueryParser();
            jsonQueryParser = new JsonQueryParser();
            pagingParser = new PagingParser();
            findQueryValidator = new FindQueryValidator();

            queryFactory = new QueryFactory(findQueryValidator, queryStringQueryParser, jsonQueryParser, pagingParser);

            lookupMetaDataModel = LookupMetaDataFactory.GetGenericKeyMetaDataModel();

            lookupTypesHelper.WhenToldTo(x=> x.IsValidLookupType(lookupType)).Return(true);
            lookupTypesHelper.WhenToldTo(x => x.FindLookupMetaData(lookupType)).Return(lookupMetaDataModel);

            halSerialiser = An<IHalSerialiser>();

            lookupController = new LookupController(lookupRepresentationHelper, lookupTypesHelper, queryFactory, halSerialiser);
            lookupController.Request = HttpRequestTestDataFactory.GetEmptyWhereHttpRequest();

        };
        
        private Because of = () =>
        {
            lookupController.Get(lookupType);
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
            lookupTypesHelper.WasToldTo(x => x.FindLookupMetaData(lookupType));
        };

        private It should_call_the_database = () =>
        {
            lookupDataManager.WasToldTo(x => x.GetLookups(Arg.Any<IFindQuery>(), "2AM", "dbo", "GenericKeyType", "GenericKeyTypeKey", "Description"));
        };

        private static IHalSerialiser halSerialiser;
    }

}
