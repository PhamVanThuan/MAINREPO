using SAHL.Core.Data.Models.EventProjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.EventProjection.Managers.AttorneyInvoicesPaidThisMonth
{
    public interface IAttorneyInvoicesPaidThisMonthDataManager
    {
        void IncrementCountAndAddInvoiceToValueColumn(decimal invoiceTotalValue);
        void ClearAttorneyInvoicesPaidThisMonthStatement();
        AttorneyInvoicesPaidThisMonthDataModel GetAttorneyInvoicesPaidThisMonthStatement();
    }
}
