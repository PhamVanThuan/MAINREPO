using System;
using System.Collections.Generic;
using System.Text;
using SAHL.X2.Common;
using SAHL.X2.Common.DataAccess;
using AspectAttribute;

namespace X2DomainService.Interface.X2EngineTest
{
    public interface IX2EngineTest
    {
        //[PerformanceBase]
        IX2ReturnData WTFRoundRobinAssignForGivenOrgStructure(IActiveDataTransaction Tran, out string AssignedUser, string DynamicRole, int GenericKey, int OrgStructureKey);
    }
}
