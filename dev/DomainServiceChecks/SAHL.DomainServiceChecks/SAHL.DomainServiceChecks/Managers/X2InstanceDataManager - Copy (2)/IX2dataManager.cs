using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.DomainServiceChecks.Managers.X2InstanceDataManager
{
    public interface IX2DataManager
    {
        bool DoesInstanseIdExist(int instanceId);
    }
}
