using System;
using Common.Enums;
using Common.Constants;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using Dapper;

namespace Automation.DataAccess.DataHelper
{
    public partial class _2AMDataHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountkey"></param>
        /// <param name="monthsInArrears"></param>
        public void UpdateLoanMonthsInArrears(int accountkey, decimal monthsInArrears)
        {
            dataContext.Execute(String.Format(@"update [sahldb].dbo.loanstatistics 
                            set monthsinarrears = {0}
                            where loannumber = {1}", monthsInArrears, accountkey));
        }
    }
}