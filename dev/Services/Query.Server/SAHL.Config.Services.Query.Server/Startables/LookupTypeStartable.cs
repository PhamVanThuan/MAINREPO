using SAHL.Core.IoC;
using SAHL.Services.Interfaces.Query.Helpers;

namespace SAHL.Config.Services.Query.Server
{
    public class LookupTypeStartable : IStartable
    {
        private readonly ILookupTypesHelper lookupTypesHelper;

        public LookupTypeStartable(ILookupTypesHelper lookupTypesHelper)
        {
            this.lookupTypesHelper = lookupTypesHelper;
        }

        public void Start()
        {
            this.lookupTypesHelper.LoadValidLookupTypes();
        }
    }
}
