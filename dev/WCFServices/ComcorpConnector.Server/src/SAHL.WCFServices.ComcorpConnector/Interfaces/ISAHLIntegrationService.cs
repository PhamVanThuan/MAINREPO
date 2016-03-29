using SAHL.WCFServices.ComcorpConnector.Objects;
using SAHL.WCFServices.ComcorpConnector.Objects.Document;
using System.Net.Security;
using System.ServiceModel;
using SAHL.WCF.Validation.Engine;

namespace SAHL.WCFServices.ComcorpConnector.Interfaces
{
    [ServiceContract]
    public interface ISAHLIntegrationService : IHasModelStateService
    {
        [OperationContract]
        [FaultContract(typeof(SAHLFault), ProtectionLevel = ProtectionLevel.None)]
        SAHLResponse Submit(SAHLRequest request);
    }
}