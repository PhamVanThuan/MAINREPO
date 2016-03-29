using SAHL.Core.Data.Models.EventProjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.EventProjection.Managers.AttorneyInvoicesPaidLastMonth
{
    public interface IAttorneyInvoicesPaidLastMonthDataManager
    {
        void UpdateAttorneyInvoicesPaidLastMonth(AttorneyInvoicesPaidThisMonthDataModel model);
    }
}
