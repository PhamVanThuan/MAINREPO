using SAHL.Core.Data;
using System;

namespace SAHL.Services.EventProjection.Managers.AttorneyInvoiceMonthlyBreakdown.Statements
{
    public class GetAttorneyRegisteredNameStatement : ISqlStatement<string>
    {
        public Guid AttorneyId { get; protected set; }

        public GetAttorneyRegisteredNameStatement(Guid attorneyId)
        {
            this.AttorneyId = attorneyId;
        }

        public string GetStatement()
        {
            return @"SELECT le.RegisteredName FROM [2AM].[dbo].[LegalEntity] le
	                        WHERE le.LegalEntityKey =  (SELECT [LegalEntityKey]
	                        FROM [2AM].[dbo].[ThirdParty]
	                        WHERE [Id] = @AttorneyId)";
        }
    }
}