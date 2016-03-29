using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.Interfaces.Query.DataManagers;
using SAHL.Services.Interfaces.Query.Helpers;
using SAHL.Services.Query.Helper;

namespace SAHL.Services.Query.Server.Specs.Factory
{
    public class LookupFactory : WithFakes
    {

        private static ILookupTypesHelper lookupTypesHelper;

        public static ILookupTypesHelper GetLookupTypeHelper(out ISupportedLookupDataManager supportedLookupDataManager)
        {
            supportedLookupDataManager = An<ISupportedLookupDataManager>();
            var supportedLookupModels = LookupMetaDataFactory.CreateSupportedLookupModels();
            supportedLookupDataManager.WhenToldTo(x => x.GetSupportedLookups()).Return(supportedLookupModels);
            supportedLookupDataManager.WhenToldTo(x => x.GetLookupSchema("generickeytype")).Return(LookupMetaDataFactory.GetGenericKeyMetaDataModel());
            lookupTypesHelper = new LookupTypesHelper(supportedLookupDataManager);
            lookupTypesHelper.LoadValidLookupTypes();
            return lookupTypesHelper;
        }

    }
}