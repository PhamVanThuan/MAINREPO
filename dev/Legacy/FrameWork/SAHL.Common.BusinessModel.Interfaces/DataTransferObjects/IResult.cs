using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAHL.Common.BusinessModel.Interfaces.DataTransferObjects
{
    public interface IResult
    {
        double Amount { get; set; }
        System.Collections.Generic.List<ICalculatedItem> CalculatedItems { get; set; }
        bool CreditLifePolicy { get; set; }
        double CreditLifePremium { get; set; }
        double InitiationFee { get; set; }
        double MonthlyFee { get; set; }
    }
}
