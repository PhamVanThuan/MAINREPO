using System;
using System.Linq;

namespace SAHL.DomainServiceChecks.Managers.ThirdPartyInvoiceDataManager
{
    public interface IThirdPartyInvoiceDataManager
    {
        bool DoesThirdPartyInvoiceExist(int thirdPartyInvoiceKey);

        bool DoesThirdPartyExist(Guid thirdPartyId);
    }
}