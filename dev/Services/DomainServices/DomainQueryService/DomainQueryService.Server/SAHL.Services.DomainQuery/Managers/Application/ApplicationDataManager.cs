using SAHL.Core.Data;
using SAHL.Services.DomainQuery.Managers.Application.Statements;

namespace SAHL.Services.DomainQuery.Managers.Application
{
    public class ApplicationDataManager : IApplicationDataManager
    {
        private IDbFactory dbFactory;

        public ApplicationDataManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public bool DoesOfferExist(int offerKey)
        {
            var sql = new DoesOfferExistForOfferKeyStatement(offerKey);
            int offerCount = -1;
            using (var db = dbFactory.NewDb().InReadOnlyAppContext())
            {
                offerCount = db.SelectOne<int>(sql);
            }
            return offerCount == 1;
        }
    }
}