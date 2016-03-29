using System.Net.Security;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace SAHL.WCFServices.ComcorpConnector.Objects
{
    [DataContract]
    [MessageContract(ProtectionLevel = ProtectionLevel.None)] // *** Note: This was originally set to ProtectionLevel.Sign but was causing issues
    public class SAHLRequest
    {
        [MessageBodyMember]
        public Application Application { get; set; }
    }
}