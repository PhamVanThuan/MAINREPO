using SAHL.Core.Data.Models._2AM;

namespace SAHL.Services.PropertyDomain.Managers
{
    public interface IPropertyDataManager
    {
        ComcorpOfferPropertyDetailsDataModel FindExistingComcorpOfferPropertyDetails(int ApplicationNumber);

        void InsertComcorpOfferPropertyDetails(ComcorpOfferPropertyDetailsDataModel comcorpOfferPropertyDetailsDataModel);

        void UpdateComcorpOfferPropertyDetails(ComcorpOfferPropertyDetailsDataModel comcorpOfferPropertyDetailsDataModel);
    }
}