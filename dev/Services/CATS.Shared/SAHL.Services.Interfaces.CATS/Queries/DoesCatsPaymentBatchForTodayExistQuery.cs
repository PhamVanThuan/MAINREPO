using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.CATS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.CATS.Queries
{
    public class DoesCatsPaymentBatchForTodayExistQuery : ServiceQuery<DoesCatsPaymentBatchForTodayExistModel>, ICATSServiceQuery, ISqlServiceQuery<DoesCatsPaymentBatchForTodayExistModel>
    {
    }
}
