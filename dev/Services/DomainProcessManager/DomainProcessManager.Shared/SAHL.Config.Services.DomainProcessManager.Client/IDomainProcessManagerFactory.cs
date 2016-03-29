using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Config.Services.DomainProcessManager.Client
{
    public interface IDomainProcessManagerClientApiFactory
    {
        DomainProcessManagerClientApi Create();
    }
}