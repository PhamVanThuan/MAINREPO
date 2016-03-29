using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using System;
using System.Linq;

namespace SAHL.Services.FrontEndTest.Managers.Statements
{
    public class RemoveApplicationMailingAddressStatement : ISqlStatement<OfferMailingAddressDataModel>
    {
        public int ApplicationNumber { get; protected set; }

        public RemoveApplicationMailingAddressStatement(int applicationNumber)
        {
            this.ApplicationNumber = applicationNumber;
        }

        public string GetStatement()
        {
            return @"delete from [2am].dbo.OfferMailingAddress where offerKey = @ApplicationNumber";
        }
    }
}