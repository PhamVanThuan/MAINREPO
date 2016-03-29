using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.EventProjection.Projections.ComcorpProgress.Model;
using SAHL.Services.EventProjection.Projections.ComcorpProgress.Statements;

namespace SAHL.Services.EventProjection.Projections.ComcorpProgress
{
    public partial class ComcorpProgressLiveReply
    {
        private IDbFactory dbFactory;

        public ComcorpProgressLiveReply(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public bool IsComcorpApplication(int offerKey)
        {
            bool result = false;
            using (var db = dbFactory.NewDb().InReadOnlyAppContext())
            {
                HasOfferAttributeStatement statment = new HasOfferAttributeStatement(offerKey, SAHL.Core.BusinessModel.Enums.OfferAttributeType.ComcorpLoan);
                OfferAttributeDataModel model = db.SelectOne(statment);
                result = model != null;
                db.Complete();
            }
            return result;
        }

        public ComcorpApplicationInformationDataModel GetComcorpApplication(int offerKey)
        {
            ComcorpApplicationInformationDataModel model;
            using (var db = dbFactory.NewDb().InReadOnlyAppContext())
            {
                GetOfferInformationForLiveReplyStatement statment = new GetOfferInformationForLiveReplyStatement(offerKey);
                model = db.SelectOne(statment);
                db.Complete();
            }
            return model;
        }
    }
}