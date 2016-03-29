using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;

namespace SAHL.Services.FinancialDomain.Managers.Statements
{
    public class GetLatestApplicationOfferInformationStatement : ISqlStatement<OfferInformationDataModel>
    {
        public GetLatestApplicationOfferInformationStatement(int applicationNumber)
        {
            this.ApplicationNumber = applicationNumber;
        }

        public int ApplicationNumber { get; protected set; }

        public string GetStatement()
        {
            return "SELECT TOP 1 * FROM dbo.[OfferInformation] WHERE [OfferKey] = @ApplicationNumber ORDER BY OfferInsertDate DESC";
        }
    }
}
