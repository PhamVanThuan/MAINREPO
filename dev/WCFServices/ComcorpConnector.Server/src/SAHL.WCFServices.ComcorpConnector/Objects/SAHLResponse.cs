using System.Net.Security;
using System.ServiceModel;

namespace SAHL.WCFServices.ComcorpConnector.Objects
{
    [MessageContract(ProtectionLevel = ProtectionLevel.None)]
    public class SAHLResponse
    {
        [MessageBodyMember]
        public string SAHLReference { get; set; }
    }
}