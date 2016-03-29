namespace Automation.DataAccess.DataHelper._2AM.Contracts
{
    public interface IApplicationDataHelper : IDataHelper
    {
        QueryResults GetLatestOfferInformationByOfferKey(int offerKey);
    }
}