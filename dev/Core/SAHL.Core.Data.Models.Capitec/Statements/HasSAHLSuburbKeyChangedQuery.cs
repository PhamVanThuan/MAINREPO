using System;

namespace SAHL.Core.Data.Models.Capitec.Statements
{
    public class HasSAHLSuburbKeyChangedQuery : ISqlStatement<SuburbDataModel>
    {
        public Guid Id { get; protected set; }

        public int SAHLSuburbKey { get; protected set; }

        public HasSAHLSuburbKeyChangedQuery(Guid id, int sahlSuburbKey)
        {
            this.Id = id;
            this.SAHLSuburbKey = sahlSuburbKey;
        }

        public string GetStatement()
        {
            return @"SELECT [Id],[SAHLSuburbKey],[SuburbName],[PostalCode],[CityId]
FROM [Capitec].[geo].[Suburb] WHERE Id = @Id AND SAHLSuburbKey = @SAHLSuburbKey";
        }
    }
}