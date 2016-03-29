using SAHL.Core.Data;

namespace SAHL.Services.ApplicationDomain.Managers.Application.Statements
{
    public class DoesApplicationExistStatement : ISqlStatement<int>
    {
        public int ApplicationNumber { get; protected set; }

        public DoesApplicationExistStatement(int applicationNumber)
        {
            this.ApplicationNumber = applicationNumber;
        }

        public string GetStatement()
        {
            var sql = @"SELECT
                            COUNT(*) AS Total
                        FROM
                            [2AM].[dbo].[Offer]
                        WHERE
                            OfferKey = @ApplicationNumber";
            return sql;
        }
    }
}