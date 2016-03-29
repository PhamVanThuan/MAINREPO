using SAHL.Common.BusinessModel.Interfaces.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    public interface IGroupExposureRepository
    {
        List<IGroupExposureItem> GetGroupExposureInfoByLegalEntity(int LegalEntityKey);

        double GetAccountGroupExposurePTI(int LegalEntityKey, double HouseholdIncome);

        double GetAccountGroupExposureLTV(int accountKey, double valuation, double furtherLendingAmount);
    }
}
