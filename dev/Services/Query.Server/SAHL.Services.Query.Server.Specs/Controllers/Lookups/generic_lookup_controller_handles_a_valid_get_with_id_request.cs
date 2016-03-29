using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.Interfaces.Query.DataManagers;
using SAHL.Services.Interfaces.Query.Helpers;
using SAHL.Services.Interfaces.Query.Models;
using SAHL.Services.Interfaces.Query.Parsers;
using SAHL.Services.Query.Controllers;
using SAHL.Services.Query.Controllers.Lookup;
using SAHL.Services.Query.DataManagers;
using SAHL.Services.Query.Factories;
using SAHL.Services.Query.Helper;
using SAHL.Services.Query.Interfaces;
using SAHL.Services.Query.Models;
using SAHL.Services.Query.Models.Lookup;
using SAHL.Services.Query.Parsers;
using SAHL.Services.Query.Parsers.Elemets;
using SAHL.Services.Query.Resources.Lookup;
using SAHL.Services.Query.Serialiser;
using SAHL.Services.Query.Server.Specs.Factory;
using SAHL.Services.Query.Validators;

namespace SAHL.Services.Query.Server.Specs.Controllers.Lookups
{
    public class generic_lookup_controller_handles_a_valid_get_with_id_request : WithFakes
    {

        private static LookupController LookupController;
        private static ILookupDataManager lookupDataManager;
        private static ILookupTypesHelper lookupTypesHelper;
        private static string lookupType;
        private static ILookupMetaDataModel lookupMetaDataModel;
        private static ILookupRepresentationHelper lookupRepresentationHelper;
        private static IQueryFactory queryFactory;
        public static IQueryStringQueryParser queryStringQueryParser;
        public static IJsonQueryParser jsonQueryParser;
        public static IPagingParser pagingParser;
        public static IFindQuery findManyQuery;
        private static IFindQueryValidator findQueryValidator;

        Establish that = () =>
        {
            lookupType = "generickeytype";

            lookupTypesHelper = An<ILookupTypesHelper>();
            lookupDataManager = An<ILookupDataManager>();
            linkResolver = An<ILinkResolver>();

            queryStringQueryParser = new QueryStringQueryParser();
            jsonQueryParser = new JsonQueryParser();
            pagingParser = new PagingParser();
            findQueryValidator = new FindQueryValidator();

            queryFactory = new QueryFactory(findQueryValidator, queryStringQueryParser, jsonQueryParser, pagingParser);
            findManyQuery = queryFactory.CreateEmptyFindQuery();
            
            lookupMetaDataModel = LookupMetaDataFactory.GetGenericKeyMetaDataModel();
            lookupRepresentationHelper = new LookupRepresentationHelper(linkResolver, lookupTypesHelper, lookupDataManager);

            lookupTypesHelper.WhenToldTo(x => x.IsValidLookupType(lookupType)).Return(true);
            lookupTypesHelper.WhenToldTo(x => x.FindLookupMetaData(lookupType)).Return(lookupMetaDataModel);
            lookupDataManager.WhenToldTo(x => x.GetLookup(Arg.Any<IFindQuery>(), "2AM", "dbo", "GenericKeyType", "GenericKeyTypeKey", "Description", 1))
                                        .Return(new LookupDataModel() { Id = 1, Description = "Test"});

            halSerialiser = An<IHalSerialiser>();

            LookupController = new LookupController(lookupRepresentationHelper, lookupTypesHelper, queryFactory, halSerialiser);
            LookupController.Request = HttpRequestTestDataFactory.GetEmptyWhereHttpRequest();
        };

        private Because of = () =>
        {
            LookupController.Get(lookupType, 1);
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
            lookupDataManager.WasToldTo(
                x => x.GetLookup(Arg.Any<IFindQuery>(), "2AM", "dbo", "GenericKeyType", "GenericKeyTypeKey", "Description", 1));
        };

        private It should_return_a_lookup_representation = () =>
        {
            lookupRepresentationHelper.WhenToldTo(
                x => x.GetLookupRepresentation("generickeytype", 1, Arg.Any<IFindQuery>()))
                .Equals(new LookupRepresentation() {Id = 1, Description = "Test", LookupType = "generickeytype"});
        };

        private static ILinkResolver linkResolver;
        private static IHalSerialiser halSerialiser;
    }
}
