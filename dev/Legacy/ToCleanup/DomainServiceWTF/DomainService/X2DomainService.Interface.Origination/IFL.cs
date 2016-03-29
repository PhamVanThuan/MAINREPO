using System;
using System.Collections.Generic;
using System.Text;
using SAHL.X2.Common;
using SAHL.X2.Common.DataAccess;

namespace X2DomainService.Interface.Origination
{
    public interface IFL
    {
        IX2ReturnData IsFurtherLoan(IActiveDataTransaction Tran, int ApplicationKey, out bool b);
        IX2ReturnData IsFurtherAdvance(IActiveDataTransaction Tran, int ApplicationKey, out bool b);
        IX2ReturnData CheckCanDisburseReadvance(int ApplicationKey, bool IgnoreWarnings, out bool b);
        IX2ReturnData CanRollBackDisburseMent(int ApplicationKey, out bool b);
        IX2ReturnData RemoveDetailTypes(IActiveDataTransaction Tran, int ApplicationKey);
        IX2ReturnData ArchiveFLRelatedCases(IActiveDataTransaction Tran, Int64 InstanceID, int ApplicationKey, string ADUser);

        IX2ReturnData FLCompleteUnholdNextApplicationWhereApplicable(IActiveDataTransaction Tran, int ApplicationKey);
        IX2ReturnData AddDetailTypes(IActiveDataTransaction Tran, int ApplicationKey, string ADUser);
        IX2ReturnData AppsInProgOfHigherPri(int ApplicationKey, out bool b);
        IX2ReturnData HighestPriority(int ApplicationKey, out bool b);
        IX2ReturnData ArchiveFLRelatedCases(IActiveDataTransaction Tran, Int64 InstanceID, string ADUser, int ApplicationKey);
        IX2ReturnData RoundRobinAndAssignOtherFLCases(IActiveDataTransaction Tran, int ApplicationKey, string DynamicRole, int OrgStructureKey, out string AssignedUser);

        IX2ReturnData ValuationRequired(int ApplicationKey, out bool b);
        IX2ReturnData LTVForFLCase(int ApplicationKey, out double LTV);
        IX2ReturnData InitialFLNTU(IActiveDataTransaction Tran, string ADUser, int ApplicationKey, Int64 InstanceID);
        IX2ReturnData CheckInformClientRules(int ApplicationKey, bool IgnoreMessages);
    }
}
