using System;

namespace SAHL.Core.Data.Models.Capitec.Statements
{
    public class DoesCountryIdExistQuery : ISqlStatement<CountryDataModel>
    {
        public Guid Id { get; protected set; }

        public DoesCountryIdExistQuery(Guid id)
        {
            this.Id = id;
        }

        public string GetStatement()
        {
            return "SELECT Id,SAHLCountryKey,CountryName FROM [Capitec].[geo].[Country] WHERE Id = @Id";
        }
    }
}