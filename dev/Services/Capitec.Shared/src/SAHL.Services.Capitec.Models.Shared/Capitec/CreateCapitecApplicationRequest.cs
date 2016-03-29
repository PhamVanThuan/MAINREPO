using SAHL.Core.Messaging.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Capitec.Models.Shared.Capitec
{
    [DataContract]
    public class CreateCapitecApplicationRequest : IMessage
    {
        public CreateCapitecApplicationRequest(CapitecApplication capitecApplication, Guid id)
        {
            this.CapitecApplication = capitecApplication;
            this.Id = id;
        }

        [DataMember]
        public CapitecApplication CapitecApplication { get; protected set; }

        [DataMember]
        public Guid Id
        {
            get;
            protected set;
        }
    }
}
