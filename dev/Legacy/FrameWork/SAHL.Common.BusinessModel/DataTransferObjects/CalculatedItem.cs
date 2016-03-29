using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces.DataTransferObjects;

namespace SAHL.Common.BusinessModel.DataTransferObjects
{
    public class CalculatedItem : ICalculatedItem
    {
        public CalculatedItem(int id, double amount, int term, double rate, double totalInstalment, int creditCriteriaUnsecuredLendingKey, double loanInstalment)
        {
            this.ID = id;
            this.Amount = amount;
            this.Term = term;
            this.Rate = rate;
            this.TotalInstalment = totalInstalment;
            this.CreditCriteriaUnsecuredLendingKey = creditCriteriaUnsecuredLendingKey;
            this.LoanInstalment = loanInstalment;
        }

        public int ID { get; set; }

        public double Amount { get; set; }

        public int Term { get; set; }

        public double Rate { get; set; }

        public double TotalInstalment { get; set; }

        public int CreditCriteriaUnsecuredLendingKey { get; set; }

        public double LoanInstalment { get; set; }
    }
}
