using SAHL.Core.Data;
using SAHL.DomainServiceChecks.Managers.PropertyDataManager.Statements;

namespace SAHL.DomainServiceChecks.Managers.PropertyDataManager
{
    public class PropertyDataManager : IPropertyDataManager
    {
        private IDbFactory dbFactory;

        public PropertyDataManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public bool IsPropertyOnOurSystem(int propertyKey)
        {
            PropertyExistsStatement query = new PropertyExistsStatement(propertyKey);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var results = db.SelectOne(query);
                return (results > 0);
            }
        }
    }
}