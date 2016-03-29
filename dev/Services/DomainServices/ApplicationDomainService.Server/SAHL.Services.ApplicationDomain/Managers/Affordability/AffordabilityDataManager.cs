using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.ApplicationDomain.Managers.Affordability.Statements;
using SAHL.Services.Interfaces.ApplicationDomain.Models;

namespace SAHL.Services.ApplicationDomain.Managers.Affordability
{
    public class AffordabilityDataManager : IAffordabilityDataManager
    {
        private IDbFactory dbFactory;

        public AffordabilityDataManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public void SaveAffordability(AffordabilityTypeModel affordabilityTypeModel, int clientKey, int applicationNumber)
        {
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                LegalEntityAffordabilityDataModel model = new LegalEntityAffordabilityDataModel(clientKey,
                    (int)affordabilityTypeModel.AffordabilityType, affordabilityTypeModel.Amount,
                    affordabilityTypeModel.Description, applicationNumber);

                db.Insert<LegalEntityAffordabilityDataModel>(model);
                db.Complete();
            }
        }

        public bool IsDescriptionRequired(Core.BusinessModel.Enums.AffordabilityType affordabilityType)
        {
            var doesAffordabilityRequireDescriptionQuery = new DoesAffordabilityRequireDescriptionStatement(affordabilityType);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                return db.SelectOne<bool>(doesAffordabilityRequireDescriptionQuery);
            }
        }

        public System.Collections.Generic.IEnumerable<LegalEntityAffordabilityDataModel> GetAffordabilityAssessment(int clientKey, int applicationNumber)
        {
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                string whereClause = string.Format("LegalEntityKey = {0} AND OfferKey = {1}", clientKey, applicationNumber);
                return db.SelectWhere<LegalEntityAffordabilityDataModel>(whereClause);
            }
        }
    }
}