using SAHL.Services.Interfaces.MortgageLoanDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.V3.Framework.DomainServices
{
    public interface IMortgageLoanDomainService : IV3Service
    {
        int GetDebitOrderDayByAccount(int accountKey);
    }
}
