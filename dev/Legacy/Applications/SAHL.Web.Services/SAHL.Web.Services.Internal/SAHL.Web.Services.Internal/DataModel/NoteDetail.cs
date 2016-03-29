using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace SAHL.Web.Services.Internal.DataModel
{
    [DataContract]
    public class NoteDetail
    {
        [DataMember]
        public int Key
        {
            get;
            set;
        }
        
        [DataMember]
        public int LegalEntityKey { get; set; }

        [DataMember]
        public string LegalEntityDisplayName { get; set; }

        [DataMember]
        public DateTime InsertedDate
        {
            get;
            set;
        }

        [DataMember]
        public string NoteText
        {
            get;
            set;
        }

        [DataMember]
        public string Tag
        {
            get;
            set;
        }

        [DataMember]
        public string WorkflowState
        {
            get;
            set;
        }
    }
}