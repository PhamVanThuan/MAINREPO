using SAHL.Services.Capitec.Models.Shared;
using SAHL.Shared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Batch.Common.Messages
{
    public class CapitecApplicationMessage : MessageBase , IBatchMessage
    {

        public CapitecApplicationMessage(CapitecApplication capitecApplication)
        {
            this.CapitecApplication = capitecApplication;
        }

        public CapitecApplication CapitecApplication { get; protected set; }

        public int BatchID
        {
            get;
            set;
        }

        public int FailureCount
        {
            get;
            set;
        }
    }
}
