using SAHL.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.EventProjection.Managers.AttorneyInvoiceMonthlyBreakdown.Statements
{
    public class AdjustAccountsPaidStatement : ISqlStatement<object>
    {
        public Guid AttorneyId { get; protected set; }
        public int Value { get; protected set; }
        public AdjustAccountsPaidStatement(Guid attorneyId, int Value)
        {
            this.AttorneyId = attorneyId;
            this.Value = Value;
        }
        public string GetStatement()
        {
            return @"Update eventprojection.projection.AttorneyInvoiceMonthlyBreakdown set AccountsPaid = @Value where AttorneyId = @AttorneyId";
        }
    }
}
