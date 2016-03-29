using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;

namespace SAHL.Services.PropertyDomain.Managers
{
    public class PropertyDataManager : IPropertyDataManager
    {
        private IDbFactory dbFactory;

        public PropertyDataManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public ComcorpOfferPropertyDetailsDataModel FindExistingComcorpOfferPropertyDetails(int ApplicationNumber)
        {
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                return db.GetByKey<ComcorpOfferPropertyDetailsDataModel, int>(ApplicationNumber);
            }
        }

        public void InsertComcorpOfferPropertyDetails(ComcorpOfferPropertyDetailsDataModel comcorpOfferPropertyDetailsDataModel)
        {
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Insert<ComcorpOfferPropertyDetailsDataModel>(comcorpOfferPropertyDetailsDataModel);
                db.Complete();
            }
        }

        public void UpdateComcorpOfferPropertyDetails(ComcorpOfferPropertyDetailsDataModel comcorpOfferPropertyDetailsDataModel)
        {
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Update<ComcorpOfferPropertyDetailsDataModel>(comcorpOfferPropertyDetailsDataModel);
                db.Complete();
            }
        }
    }
}