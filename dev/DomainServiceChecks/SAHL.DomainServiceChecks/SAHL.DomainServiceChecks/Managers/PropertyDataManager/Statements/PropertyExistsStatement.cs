using SAHL.Core.Data;

namespace SAHL.DomainServiceChecks.Managers.PropertyDataManager.Statements
{
    public class PropertyExistsStatement : ISqlStatement<int>
    {
        public int PropertyKey { get; protected set; }

        public PropertyExistsStatement(int propertyKey)
        {
            this.PropertyKey = propertyKey;
        }

        public string GetStatement()
        {
            var query = "SELECT count(1)  FROM [2AM].[dbo].[Property] WHERE [PropertyKey] = @PropertyKey";
            return query;
        }
    }
}
 