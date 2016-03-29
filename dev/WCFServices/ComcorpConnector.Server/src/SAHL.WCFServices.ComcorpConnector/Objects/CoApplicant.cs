using System.Runtime.Serialization;

namespace SAHL.WCFServices.ComcorpConnector.Objects
{
    [DataContract]
    public class CoApplicant : Applicant
    {
        [DataMember]
        public Spouse Spouse { get; set; }
    }
}