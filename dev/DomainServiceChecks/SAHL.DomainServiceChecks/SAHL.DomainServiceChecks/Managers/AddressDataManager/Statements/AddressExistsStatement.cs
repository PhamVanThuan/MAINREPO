using SAHL.Core.Data;

namespace SAHL.DomainServiceChecks.Managers.AddressDataManager.Statements
{
    public class AddressExistsStatement : ISqlStatement<int>
    {
        public int AddressKey { get; protected set; }

        public AddressExistsStatement(int AddressKey)
        {
            this.AddressKey = AddressKey;
        }

        public string GetStatement()
        {
            var query = "SELECT COUNT(1) AS Total FROM [2AM].[dbo].[Address] WHERE AddressKey = @AddressKey";
            return query;
        }
    }
}
