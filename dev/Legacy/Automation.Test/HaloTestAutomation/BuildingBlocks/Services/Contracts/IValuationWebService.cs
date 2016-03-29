using System.Data;
using System.ServiceModel;
namespace BuildingBlocks.Services.Contracts
{
    [ServiceContract]
    public interface IValuationWebService
    {
        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IValuation/SubmitCompletedValuationLightstone", ReplyAction = "http://tempuri.org/IValuation/SubmitCompletedValuationLightstoneResponse")]
        DataSet SubmitCompletedValuationLightstone(int UniqueID, string XMLData);

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IValuation/SubmitRejectedValuationLightstone", ReplyAction = "http://tempuri.org/IValuation/SubmitRejectedValuationLightstoneResponse")]
        DataSet SubmitRejectedValuationLightstone(int UniqueID, string XMLData);

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IValuation/SubmitAmendedValuationLightstone", ReplyAction = "http://tempuri.org/IValuation/SubmitAmendedValuationLightstoneResponse")]
        DataSet SubmitAmendedValuationLightstone(int UniqueID, string XMLData);
    }
}
