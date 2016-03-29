using SAHL.Core.Data;

namespace SAHL.DomainServiceChecks.Managers.ApplicationDataManager.Statements
{
    public class IsActiveClientRoleStatement : ISqlStatement<int>
    {
        public int ApplicationRoleKey { get; protected set; }

        public IsActiveClientRoleStatement(int ApplicationRoleKey)
        {
            this.ApplicationRoleKey = ApplicationRoleKey;
        }

        public string GetStatement()
        {
            var query = @"SELECT 
                            COUNT(1) AS Total 
                        FROM
                            [2AM].dbo.OfferRole ofr 
                        JOIN
                            [2am].dbo.OfferRoleType ort on ofr.OfferRoleTypeKey = ort.OfferRoleTypeKey
                        WHERE 
                            ofr.OfferRoleKey = @ApplicationRoleKey
                        AND
                            ofr.GeneralStatusKey = 1 --open
                        AND 
                            ort.OfferRoleTypeGroupKey = 3 --client";

            return query;
        }
    }
}
