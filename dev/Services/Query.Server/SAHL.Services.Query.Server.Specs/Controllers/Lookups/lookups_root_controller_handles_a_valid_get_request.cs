using System.Net.Http;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.Interfaces.Query.DataManagers;
using SAHL.Services.Interfaces.Query.Helpers;
using SAHL.Services.Interfaces.Query.Models;
using SAHL.Services.Query.Controllers;
using SAHL.Services.Query.Helper;
using SAHL.Services.Query.Interfaces;
using SAHL.Services.Query.Resources.Lookup;
using SAHL.Services.Query.Server.Specs.Factory;

namespace SAHL.Services.Query.Server.Specs.Controllers.Lookups
{
    public class lookups_root_controller_handles_a_valid_get_request : WithFakes
    {

        private static ISupportedLookupDataManager supportedLookupDataManager;
        private static ILookupDataManager lookupDataManager;
        private static ILookupTypesHelper lookupTypesHelper;
        private static ILookupMetaDataModel lookupMetaDataModel;
        private static ILookupRepresentationHelper lookupRepresentationHelper;
        private static LookupTypeListRepresentation lookupTypeListRepresentation;
        public static ILinkResolver linkResolver;

        Establish that = () =>
        {
            lookupDataManager = An<ILookupDataManager>();
            linkResolver = An<ILinkResolver>();

            supportedLookupDataManager = An<ISupportedLookupDataManager>();
            var supportedLookupModels = LookupMetaDataFactory.CreateSupportedLookupModels();
            supportedLookupDataManager.WhenToldTo(x => x.GetSupportedLookups()).Return(supportedLookupModels);
            
            lookupTypesHelper = new LookupTypesHelper(supportedLookupDataManager);

            lookupTypesHelper.LoadValidLookupTypes();

            lookupRepresentationHelper = new LookupRepresentationHelper(linkResolver, lookupTypesHelper, lookupDataManager);
        };

        private Because of = () =>
        {
            lookupTypeListRepresentation = lookupRepresentationHelper.GetLookupTypesRepresentation();
        };

        private It should_return_correctly_mapped_object = () =>
        {
            lookupTypeListRepresentation.Equals(LookupMetaDataFactory.GetLookupTypeListRepresentation());
        };
        
    }
}
