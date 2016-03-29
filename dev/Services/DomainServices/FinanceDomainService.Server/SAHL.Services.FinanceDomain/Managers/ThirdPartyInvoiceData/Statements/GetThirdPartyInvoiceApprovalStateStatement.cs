using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData.Statements
{
    public class GetThirdPartyInvoiceApprovalStateStatement : ISqlStatement<bool>
    {
        public GetThirdPartyInvoiceApprovalStateStatement(int thirdPartyInvoiceKey)
        {
            this.ThirdPartyInvoiceKey = thirdPartyInvoiceKey;
        }

        public int ThirdPartyInvoiceKey { get; protected set; }

        public string GetStatement()
        {
            var query = @"select (case when exists (
                            select *
                            from [2AM].[dbo].ThirdPartyInvoice
                            where 
						    ThirdPartyInvoiceKey =  {0} 
							and 
							InvoiceStatusKey = {1}
                        )
                        then cast(1 as bit)
                        else cast(0 as bit) end)[IsApproved]";

            query = string.Format(query, ThirdPartyInvoiceKey, (int)InvoiceStatus.Approved);

            return query;
        }
    }
}
