using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces.DataTransferObjects;

namespace SAHL.Common.BusinessModel.DataTransferObjects
{
    public class Result : IResult
    {
        public Result()
        {
            CalculatedItems = new List<ICalculatedItem>();
        }

        public double Amount { get; set; }

        public bool CreditLifePolicy { get; set; }

        public double InitiationFee { get; set; }

        public double MonthlyFee { get; set; }

        public double CreditLifePremium { get; set; }

        public List<ICalculatedItem> CalculatedItems { get; set; }
    }
}
