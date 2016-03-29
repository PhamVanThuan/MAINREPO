using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Capitec.Managers.Application.Models
{
    public class CalculatedLoanDetailsModel
    {
        public decimal Instalment { get; set; }
        public decimal InterestRate { get; set; }
        public decimal LoanAmount { get; set; }
        public decimal LTV { get; set; }
        public decimal PTI { get; set; }
        public bool EligibleApplication { get; set; }
        public int TermInMonths { get; set; }

        public bool CapitaliseFees { get; set; }
    }
}
