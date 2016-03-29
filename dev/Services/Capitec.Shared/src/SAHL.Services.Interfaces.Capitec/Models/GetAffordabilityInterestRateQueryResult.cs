using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.Capitec.Models
{
    public class GetAffordabilityInterestRateQueryResult
    {
        public decimal AmountQualifiedFor { get; set; }
        public decimal Instalment { get; set; }
        public decimal InterestRate { get; set; }
        public decimal PropertyPriceQualifiedFor { get; set; }
        public int TermInMonths { get; set; }
        public decimal PaymentToIncome { get; set; }
    }
}
