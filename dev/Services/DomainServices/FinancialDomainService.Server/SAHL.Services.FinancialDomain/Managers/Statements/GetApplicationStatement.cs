using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.FinancialDomain.Managers.Statements
{
    public class GetApplicationStatement : ISqlStatement<OfferDataModel>
    {
        public int ApplicationNumber { get; protected set; }

        public GetApplicationStatement(int ApplicationNumber)
        {
            this.ApplicationNumber = ApplicationNumber;
        }

        public string GetStatement()
        {
            return @"select * from Offer where OfferKey = @ApplicationNumber";
        }
    }
}
