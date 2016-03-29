using System;

namespace SAHL.Core.Data.Models.Capitec.Statements
{
    public class ChangeSuburbsDetailsQuery : ISqlStatement<SuburbDataModel>
    {
        public Guid Id { get; protected set; }

        public string SuburbName { get; protected set; }

        public int SAHLSuburbKey { get; protected set; }

        public string PostalCode { get; protected set; }

        public Guid CityId { get; protected set; }

        public ChangeSuburbsDetailsQuery(Guid id, string suburbName, int sahlSuburbKey, string postalCode, Guid cityId)
        {
            this.Id = id;
            this.SuburbName = suburbName;
            this.SAHLSuburbKey = sahlSuburbKey;
            this.PostalCode = postalCode;
            this.CityId = cityId;
        }

        public string GetStatement()
        {
            return @"UPDATE [Capitec].[geo].[Suburb]
SET [SAHLSuburbKey] = @sahlSuburbKey,[SuburbName] = @suburbName,[PostalCode] = @postalCode,[CityId] = @cityId
WHERE Id = @Id";
        }
    }
}