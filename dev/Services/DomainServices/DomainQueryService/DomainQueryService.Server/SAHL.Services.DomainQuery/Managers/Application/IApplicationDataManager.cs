using System.Collections.Generic;

namespace SAHL.Services.DomainQuery.Managers.Application
{
    public interface IApplicationDataManager
    {
        bool DoesOfferExist(int offerKey);
    }
}