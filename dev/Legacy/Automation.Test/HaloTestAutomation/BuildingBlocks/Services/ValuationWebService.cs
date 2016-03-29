using BuildingBlocks.Services.Contracts;
using System.ServiceModel;
namespace BuildingBlocks.Services
{
    public class ValuationWebService : ClientBase<IValuationWebService>, IValuationWebService
    {
        public System.Data.DataSet SubmitCompletedValuationLightstone(int UniqueID, string XMLData)
        {
            return base.Channel.SubmitCompletedValuationLightstone(UniqueID, XMLData);
        }

        public System.Data.DataSet SubmitRejectedValuationLightstone(int UniqueID, string XMLData)
        {
            return base.Channel.SubmitRejectedValuationLightstone(UniqueID, XMLData);
        }

        public System.Data.DataSet SubmitAmendedValuationLightstone(int UniqueID, string XMLData)
        {
            return base.Channel.SubmitAmendedValuationLightstone(UniqueID, XMLData);
        }
    }
}