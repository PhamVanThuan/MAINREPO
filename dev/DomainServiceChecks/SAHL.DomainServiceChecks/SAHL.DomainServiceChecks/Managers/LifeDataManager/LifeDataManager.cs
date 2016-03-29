using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.DomainServiceChecks.Managers.LifeDataManager.Statements;

namespace SAHL.DomainServiceChecks.Managers.LifeDataManager
{
    public class LifeDataManager : ILifeDataManager
    {
        private IDbFactory dbFactory;

        public LifeDataManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public DisabilityClaimDataModel GetDisabilityClaimByKey(int disabilityClaimKey)
        {
            GetDisabilityClaimByKeyStatement query = new GetDisabilityClaimByKeyStatement(disabilityClaimKey);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var results = db.SelectOne<DisabilityClaimDataModel>(query);
                return results;
            }
        }
    }
}