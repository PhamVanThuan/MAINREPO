using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using SAHL.WCF.Validation.Engine;

namespace SAHL.WCFServices.ComcorpConnector.Objects
{
    [DataContract]
    public class SAHLFault 
    {
        [DataMember]
        public int FaultCode { get; set; }

        [DataMember]
        public string FaultDescription { get; set; }
    }
}