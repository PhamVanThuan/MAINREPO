using SAHL.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.FinancialDomain.Managers.Statements
{
    public class RemoveAllApplicationFeesStatement : ISqlStatement<int>
    {
        public int ApplicationNumber { get; protected set; }

        public RemoveAllApplicationFeesStatement(int ApplicationNumber)
        {
            this.ApplicationNumber = ApplicationNumber;
        }

        public string GetStatement()
        {
            return @"DELETE FROM [2AM].[dbo].[OfferExpense] WHERE OfferKey = @ApplicationNumber and ExpenseTypeKey in (1,4,5)";
        }
    }
}
