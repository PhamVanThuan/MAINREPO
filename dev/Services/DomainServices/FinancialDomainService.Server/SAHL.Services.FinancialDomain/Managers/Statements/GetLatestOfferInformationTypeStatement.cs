using SAHL.Core.Data;
using SAHL.Services.FinancialDomain.Managers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.FinancialDomain.Managers.Statements
{
    public class GetLatestOfferInformationTypeStatement : ISqlStatement<int>
    {
        public int ApplicationNumber { get; protected set; }

        public GetLatestOfferInformationTypeStatement(int ApplicationNumber)
        {
            this.ApplicationNumber = ApplicationNumber;
        }

        public string GetStatement()
        {
            return @"select 
                    oi.OfferInformationTypeKey 
                    from [2am].dbo.OfferInformation oi
                    join [2am].dbo.Offer o on oi.OfferKey = o.OfferKey
                    where oi.OfferInformationKey = (select max(OfferInformationKey) 
                    from [2am].dbo.OfferInformation where OfferKey = @ApplicationNumber)";
        }
    }
}
