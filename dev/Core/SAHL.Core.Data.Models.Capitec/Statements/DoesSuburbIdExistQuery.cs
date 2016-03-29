using System;

namespace SAHL.Core.Data.Models.Capitec.Statements
{
    public class DoesSuburbIdExistQuery : ISqlStatement<SuburbDataModel>
    {
        public Guid Id { get; protected set; }

        public DoesSuburbIdExistQuery(Guid id)
        {
            this.Id = id;
        }

        public string GetStatement()
        {
            return @"SELECT [Id],[SAHLSuburbKey],[SuburbName],[PostalCode],[CityId]
FROM [Capitec].[geo].[Suburb] WHERE Id = @Id";
        }
    }
}