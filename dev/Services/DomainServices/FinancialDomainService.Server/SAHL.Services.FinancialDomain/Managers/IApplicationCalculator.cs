using SAHL.Core.Data.Models._2AM;
using SAHL.Services.Interfaces.FinancialDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.FinancialDomain.Managers
{
    public interface IApplicationCalculator
    {
        PricedMortgageLoanApplicationInformationModel PriceApplication(MortgageLoanApplicationInformationModel applicationInfo, OriginationFeesModel originationFees);
    }
}
