using SAHL.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.FinancialDomain.Managers.Statements
{
    public class RemoveApplicationAttributeStatement : ISqlStatement<int>
    {
        public int ApplicationNumber { get; protected set; }
        public int OfferAttributeTypeKey { get; protected set; }

        public RemoveApplicationAttributeStatement(int applicationNumber, int offerAttributeTypeKey)
        {
            this.ApplicationNumber = applicationNumber;
            this.OfferAttributeTypeKey = offerAttributeTypeKey;
        }

        public string GetStatement()
        {
            return @"delete from [2AM].[dbo].[OfferAttribute]
                     where OfferKey = @ApplicationNumber
                       and OfferAttributeTypeKey = @OfferAttributeTypeKey";
        }
    }
}
