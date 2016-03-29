using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using System;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Managers.Application.Statements
{
    public class GetApplicationDebitOrderStatement : ISqlStatement<OfferDebitOrderDataModel>
    {
        public int ApplicationNumber { get; protected set; }

        public GetApplicationDebitOrderStatement(int applicationNumber)
        {
            this.ApplicationNumber = applicationNumber;
        }

        public string GetStatement()
        {
            var sql = @"SELECT * FROM [2AM].[dbo].[OfferDebitOrder] WHERE  OfferKey = @ApplicationNumber";
            return sql;
        }
    }
}