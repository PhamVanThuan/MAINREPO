using SAHL.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.FrontEndTest.Commands
{
    public class PostTransactionCommand : ServiceCommand, IFrontEndTestCommand
    {
        public int financialServiceKey {get; protected set;}
        public int transactionTypeKey {get; protected set;}
        public decimal amount {get; protected set;} 
        public DateTime effectiveDate {get; protected set;}
        public string reference {get; protected set;}
        public string userId { get; protected set; }

        public PostTransactionCommand(int financialServiceKey, int transactionTypeKey, decimal amount, DateTime effectiveDate, string reference, string userId)
        {
            this.financialServiceKey = financialServiceKey;
            this.transactionTypeKey = transactionTypeKey;
            this.amount = amount;
            this.effectiveDate = effectiveDate;
            this.reference = reference;
            this.userId = userId;
        }
    }
}
