using System.Runtime.Serialization;

namespace SAHL.WCFServices.ComcorpConnector.Objects
{
    [DataContract]
    public class MainApplicant : Applicant
    {
        [DataMember]
        public Spouse Spouse { get; set; }
    }
}