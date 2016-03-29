using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.FinancialDomain.Managers.Models;
using SAHL.Services.Interfaces.FinancialDomain.Models;
using System.Collections.Generic;

namespace SAHL.Services.FinancialDomain.Managers
{
    public interface IFinancialManager
    {
        void SetApplicationInformationSPVKey(int applicationNumber, int SPVKey);

        void SyncApplicationAttributes(int applicationNumber, IEnumerable<GetOfferAttributesModel> determinedOfferAttributes);
    }
}