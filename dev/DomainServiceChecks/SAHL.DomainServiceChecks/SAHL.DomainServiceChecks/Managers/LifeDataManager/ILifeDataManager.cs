using SAHL.Core.Data.Models._2AM;

namespace SAHL.DomainServiceChecks.Managers.LifeDataManager
{
    public interface ILifeDataManager
    {
        DisabilityClaimDataModel GetDisabilityClaimByKey(int disabilityClaimKey);
    }
}