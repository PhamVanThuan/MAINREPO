using SAHL.Core.Data;
using SAHL.DomainServiceChecks.Managers.AddressDataManager.Statements;

namespace SAHL.DomainServiceChecks.Managers.AddressDataManager
{
    public class AddressDataManager : IAddressDataManager
    {
        private IDbFactory dbFactory;

        public AddressDataManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public bool IsAddressInOurSystem(int AddressKey)
        {
            AddressExistsStatement query = new AddressExistsStatement(AddressKey);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var results = db.SelectOne(query);
                return (results > 0);
            }
        }
    }
}