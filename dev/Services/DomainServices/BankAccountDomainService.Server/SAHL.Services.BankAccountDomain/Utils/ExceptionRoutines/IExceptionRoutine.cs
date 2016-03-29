using SAHL.Core.Data.Models._2AM;
using System;
using System.Linq;

namespace SAHL.Services.BankAccountDomain.Utils
{
    public interface IExceptionRoutine
    {
        int Execute(CDVExceptionsDataModel cdvExceptionModel);
    }
}