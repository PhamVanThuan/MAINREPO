using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;

namespace SAHL.DomainServiceChecks.Managers.ApplicationDataManager.Statements
{
    public class IsOpenApplicationStatement : ISqlStatement<int>
    {
        public int ApplicationNumber { get; protected set; }
        public int OfferStatusKey { get; protected set; }

        public IsOpenApplicationStatement(int ApplicationNumber)
        {
            this.ApplicationNumber = ApplicationNumber;
            this.OfferStatusKey = (int)OfferStatus.Open;
        }

        public string GetStatement()
        {
            var query = @"SELECT 
                            COUNT(1) AS Total 
                        FROM
                            [2AM].[dbo].[Offer] 
                        WHERE 
                            [OfferKey] = @ApplicationNumber
                        AND 
                            [OfferStatusKey] = @OfferStatusKey";

            return query;
        }
    }
}
