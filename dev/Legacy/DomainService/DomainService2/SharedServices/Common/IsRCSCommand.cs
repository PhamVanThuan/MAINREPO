using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainService2.SharedServices.Common
{
    public class IsRCSAccountCommand : StandardDomainServiceCommand
    {
        public int AccountKey { get; set; }
        public bool Result { get; set; }
        public IsRCSAccountCommand(int accountKey)
        {
            this.AccountKey = accountKey;
        }
    }
}
