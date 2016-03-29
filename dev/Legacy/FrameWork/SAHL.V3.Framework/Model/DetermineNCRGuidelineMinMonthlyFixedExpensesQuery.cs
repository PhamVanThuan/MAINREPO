using SAHL.Core.SystemMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.V3.Framework.Model
{
    public class DetermineNCRGuidelineMinMonthlyFixedExpensesQuery
    {
        public DetermineNCRGuidelineMinMonthlyFixedExpensesQuery (decimal grossMonthlyIncome)
	    {
            this.GrossMonthlyIncome = grossMonthlyIncome;
	    }

        public decimal GrossMonthlyIncome { get; protected set; }

        public decimal Result { get; protected set; }

        public ISystemMessageCollection Messages { get; private set; }

        public void SetResult(decimal result, ISystemMessageCollection messages)
        {
            this.Messages = messages;
            this.Result = result;
        }
    }
}
