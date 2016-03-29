using System;
using System.Collections.Generic;
using System.Text;
using SAHL.X2.Common;
using SAHL.X2.Common.DataAccess;
using AspectAttribute;

namespace X2DomainService.Interface.WTFSimpleTest
{
    public interface IWTFSimpleTest
    {
        //IX2ReturnData GetUserWhoWorkedOnThisLEsOtherCasesForDynamicRole(out string ADUserName, int OfferRoleTypeKey, int ApplicationKey);
        //[PerformanceBase]
        IX2ReturnData WTFRoundRobinAssignForGivenOrgStructure(IActiveDataTransaction Tran, out string AssignedUser, string DynamicRole, int GenericKey, int OrgStructureKey);
        
        //IX2ReturnData RoundRobinAssignForGivenOrgStructure(IActiveDataTransaction Tran, out string AssignedUser, string DynamicRole, int GenericKey, List<int> OrgStructureKeys);
        
        //bool Test();
    }
}
