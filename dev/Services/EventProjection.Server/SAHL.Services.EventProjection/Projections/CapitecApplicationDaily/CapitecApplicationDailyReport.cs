using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Services.EventProjection.Projections.CapitecApplicationDaily.Models;
using SAHL.Services.EventProjection.Projections.CapitecApplicationDaily.Statements;
using System;
using System.Linq;

namespace SAHL.Services.EventProjection.Projections.CapitecApplicationDaily
{
    public partial class CapitecApplicationDailyReport
    {
        private IDbFactory dbFactory;

        public CapitecApplicationDailyReport(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        private void Update(int offerKey, Guid sahlOfferStatusKey, Guid sahlOfferStageKey, string consultantADName, DateTime compositeDate)
        {
            bool isCapitecApplication = IsCapitecApplication(offerKey);
            if (isCapitecApplication)
            {
                ConsultantInfoDataModel consultantInfo = GetConsultantDetails(consultantADName);
                using (var db = dbFactory.NewDb().InAppContext())
                {
                    ISqlStatement<ApplicationDataModel> updateStatement = new UpdateCapitecApplicationStatement(offerKey, sahlOfferStatusKey, sahlOfferStageKey, consultantInfo.Name, 
                        consultantInfo.ContactNumber, compositeDate);
                    db.Update(updateStatement);
                    db.Complete();
                }
            }
        }

        private void UpdateApplicationStatus(int offerKey, Guid sahlOfferStatusKey, DateTime compositeDate)
        {
            bool isCapitecApplication = IsCapitecApplication(offerKey);
            if (isCapitecApplication)
            {
                using (var db = dbFactory.NewDb().InAppContext())
                {
                    ISqlStatement<ApplicationDataModel> statement = new UpdateCapitecApplicationStatusStatement(offerKey, sahlOfferStatusKey, compositeDate);
                    db.Update(statement);
                    db.Complete();
                }
            }
        }

        private ConsultantInfoDataModel GetConsultantDetails(string consultantADName)
        {
            ConsultantInfoDataModel legalEntity;
            using (var db = dbFactory.NewDb().InReadOnlyAppContext())
            {
                GetConsultantLegalEntityDetailsStatement getConsultantStatement = new GetConsultantLegalEntityDetailsStatement(consultantADName);
                legalEntity = db.SelectOne<ConsultantInfoDataModel>(getConsultantStatement);
                db.Complete();
            }
            if (legalEntity == null)
            {
                legalEntity = new ConsultantInfoDataModel("", "");
            }
            return legalEntity;
        }

        private bool IsCapitecApplication(int offerKey)
        {
            bool isCapitec = false;
            using (var db = dbFactory.NewDb().InReadOnlyAppContext())
            {
                GetOfferAttributeStatment isCapitecOffer = new GetOfferAttributeStatment(offerKey, SAHL.Core.BusinessModel.Enums.OfferAttributeType.CapitecLoan);
                isCapitec = db.Select<OfferAttributeDataModel>(isCapitecOffer).Count() == 1;
                db.Complete();
            }
            return isCapitec;
        }
    }
}