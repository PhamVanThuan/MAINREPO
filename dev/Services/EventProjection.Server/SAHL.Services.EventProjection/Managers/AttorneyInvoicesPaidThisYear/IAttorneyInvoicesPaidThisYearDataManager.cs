using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.EventProjection.Managers.AttorneyInvoicesPaidThisYear
{
    public interface IAttorneyInvoicesPaidThisYearDataManager
    {
        void IncrementCountAndAddInvoiceToValueColumn(decimal invoiceTotalValue);
        void ClearAttorneyInvoicesPaidThisYear();
    }
}
