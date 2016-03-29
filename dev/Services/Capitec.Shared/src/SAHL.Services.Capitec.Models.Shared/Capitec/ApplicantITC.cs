using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace SAHL.Services.Capitec.Models.Shared
{
    [DataContract]
    public class ApplicantITC
    {
        public ApplicantITC(DateTime itcDate, string request, string response)
        {
            this.ITCDate = itcDate;
            this.Request = request;
            this.Response = response;
        }

        [DataMember]
        public DateTime ITCDate { get; protected set; }

        [DataMember]
        public string Request { get; protected set; }

        [DataMember]
        public string Response { get; protected set; }
    }
}
