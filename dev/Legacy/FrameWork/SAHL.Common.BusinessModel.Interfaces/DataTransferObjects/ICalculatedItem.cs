using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAHL.Common.BusinessModel.Interfaces.DataTransferObjects
{
    public interface ICalculatedItem
    {
		int ID { get; set; }
        double Amount { get; set; }
        int CreditCriteriaUnsecuredLendingKey { get; set; }
        double Rate { get; set; }
        int Term { get; set; }
        double TotalInstalment { get; set; }
        double LoanInstalment { get; set; }
    }
}
