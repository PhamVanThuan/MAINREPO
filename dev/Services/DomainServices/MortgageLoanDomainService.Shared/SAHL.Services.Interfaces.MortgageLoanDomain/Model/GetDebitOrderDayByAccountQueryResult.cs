using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.MortgageLoanDomain.Model
{
    public class GetDebitOrderDayByAccountQueryResult
    {
        public GetDebitOrderDayByAccountQueryResult(int debitOrderDay)
        {
            this.DebitOrderDay = debitOrderDay;
        }

        public int DebitOrderDay { get; set; }
    }
}
