using SAHL.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Core.Data.Models._2AM;

namespace SAHL.Services.FinancialDomain.Managers.Statements
{
    public class GetApplicationAttributeByAttributeTypeStatement : ISqlStatement<OfferAttributeDataModel>
    {
        public int ApplicationNumber { get; protected set; }
        
        public int OfferAttributeTypeKey { get; protected set; }

        public GetApplicationAttributeByAttributeTypeStatement(int applicationNumber, int offerAttributeTypeKey)
        {
            this.ApplicationNumber = applicationNumber;
            this.OfferAttributeTypeKey = offerAttributeTypeKey;
        }

        public string GetStatement()
        {
            return @"select [OfferAttributeKey]
                           ,[OfferKey]
                           ,[OfferAttributeTypeKey]
                       from [2AM].[dbo].[OfferAttribute]
                      where OfferKey = @ApplicationNumber
                        and OfferAttributeTypeKey = @OfferAttributeTypeKey";
        }
    }
}
