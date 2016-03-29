using SAHL.Services.Capitec.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Batch.Common.ServiceContracts
{
    public interface ICapitecClientService
    {
        bool CreateApplication(CapitecApplication capitecApplication, int messageId);
    }
}
