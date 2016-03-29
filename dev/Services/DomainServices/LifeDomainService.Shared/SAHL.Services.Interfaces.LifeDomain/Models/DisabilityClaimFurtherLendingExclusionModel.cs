using SAHL.Core.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.LifeDomain.Models
{
    public class DisabilityClaimFurtherLendingExclusionModel : ValidatableModel
    {
        public int AccountKey { get; protected set; }
        public string Description { get; protected set; }
        public DateTime Date { get; protected set; }
        public double Amount { get; protected set; }

        public DisabilityClaimFurtherLendingExclusionModel(int accountKey, string description, DateTime date, double amount)
        {
            this.AccountKey = accountKey;
            this.Description = description;
            this.Date = date;
            this.Amount = amount;
        }
    }
}
