using SAHL.Core.Services;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Services.Interfaces.ApplicationDomain.Queries;

namespace SAHL.Services.ApplicationDomain.QueryStatements
{
    public class DoesAccountBelongToClientQueryStatement : IServiceQuerySqlStatement<DoesAccountBelongToClientQuery, DoesAccountBelongToClientQueryResult>
    {
        public string GetStatement()
        {
            return @"SELECT 
                        TOP 1 o.OfferKey
                    FROM 
                        [2AM].[dbo].[Offer] o
                    JOIN 
                        [2AM].[dbo].[OfferRole] ofr ON o.OfferKey  = ofr.OfferKey
                    JOIN 
                        [2AM].[dbo].[LegalEntity] le ON ofr.LegalEntityKey  = le.LegalEntityKey
                    WHERE 
                        o.ReservedAccountKey = @AccountKey
                    AND 
                        le.IdNumber = @IdNumber";
        }
    }
}
