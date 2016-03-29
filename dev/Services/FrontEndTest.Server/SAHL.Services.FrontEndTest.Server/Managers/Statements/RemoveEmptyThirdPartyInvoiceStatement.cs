using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using System;
using System.Linq;

namespace SAHL.Services.FrontEndTest.Managers.Statements
{
    public class RemoveEmptyThirdPartyInvoiceStatement : ISqlStatement<int>
    {
        public int ThirdPartyInvoiceKey{ get; protected set;}
        
        public RemoveEmptyThirdPartyInvoiceStatement(int thirdPartyInvoiceKey)
        {
            this.ThirdPartyInvoiceKey = thirdPartyInvoiceKey;
        }
        
        public string GetStatement()
        {
            return @"delete from [FETest].[dbo].[EmptyThirdPartyInvoices] WHERE ThirdPartyInvoiceKey = @ThirdPartyInvoiceKey";
        }
    }
}
