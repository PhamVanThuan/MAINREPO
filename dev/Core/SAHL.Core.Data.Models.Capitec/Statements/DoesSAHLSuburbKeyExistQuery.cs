namespace SAHL.Core.Data.Models.Capitec.Statements
{
    public class DoesSAHLSuburbKeyExistQuery : ISqlStatement<SuburbDataModel>
    {
        public int SAHLSuburbKey { get; protected set; }

        public DoesSAHLSuburbKeyExistQuery(int sahlSuburbKey)
        {
            this.SAHLSuburbKey = sahlSuburbKey;
        }

        public string GetStatement()
        {
            return @"SELECT [Id],[SAHLSuburbKey],[SuburbName],[PostalCode],[CityId]
FROM [Capitec].[geo].[Suburb] WHERE SAHLSuburbKey = @SAHLSuburbKey";
        }
    }
}