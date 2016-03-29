using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.CATS.Models;

namespace SAHL.Services.Interfaces.CATS.Queries
{
    public class IsThereACatsFileBeingProcessedForProfileQuery : ServiceQuery<bool>, ICATSServiceQuery
    {
        public int CatsBatchTypeKey { get; protected set; }

        public IsThereACatsFileBeingProcessedForProfileQuery(int catsBatchTypeKey)
        {
            CatsBatchTypeKey = catsBatchTypeKey;
        }
    }
}
