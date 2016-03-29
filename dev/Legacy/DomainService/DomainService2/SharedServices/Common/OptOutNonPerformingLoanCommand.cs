using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainService2.SharedServices.Common
{
    public class OptOutNonPerformingLoanCommand : StandardDomainServiceCommand
    {
        public OptOutNonPerformingLoanCommand(int accountKey)
        {
            this.AccountKey = accountKey;
        }

        public int AccountKey { get; protected set; }
    }
}