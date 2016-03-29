using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.Capitec.ViewModels.Apply
{
    public class UnEmployedDetails
    {
        public UnEmployedDetails(decimal grossMonthlyIncome = 0)
        {
            this.GrossMonthlyIncome = grossMonthlyIncome;
        }
        public decimal GrossMonthlyIncome { get; set; }
    }
}
