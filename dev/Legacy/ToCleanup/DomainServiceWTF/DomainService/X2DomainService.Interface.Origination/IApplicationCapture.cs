using System;
using System.Collections.Generic;
using System.Text;
using SAHL.X2.Common;
using System.Transactions;
using SAHL.X2.Common.DataAccess;

namespace X2DomainService.Interface.Origination
{
    public interface IApplicationCapture
    {
        bool IsEstateAgentOther(IActiveDataTransaction Tran, Int64 InstanceID, string CreatorAdUserName, int ApplicationKey, out string AssignedTo);
        bool IsEstateAgent(IActiveDataTransaction Tran, string CreatorAdUserName, out bool b);
        bool IsEstateAgentConsultant(IActiveDataTransaction Tran, Int64 InstanceID, string CreatorADUser, int ApplicationKey, string DynamicRole, out string AssignedTo);
        IX2ReturnData BranchSubmitApplicationRulesCheck(out bool b, int ApplicationKey, bool IgnoreWarnings);
        IX2ReturnData BranchSubmitApplicationPromoteLeadToMainApp(IActiveDataTransaction Tran, out bool b, int ApplicationKey, bool IgnoreWarnings);
        IX2ReturnData ManagerSubmitApplicationRulesCheck(out bool b, int ApplicationKey, bool IgnoreWarnings);
        IX2ReturnData ManagerSubmitApplicationPromoteLeadToMainApp(IActiveDataTransaction Tran, out bool b, int ApplicationKey, bool IgnoreWarnings);
        IX2ReturnData ReassignOrEscalateCaseToUser(IActiveDataTransaction Tran, out string AssignedTo, int ApplicationKey, string DynamicRole, string CurrentADUser, bool MarkPreviousInactive, int PreviousRoleKey);
        IX2ReturnData SubmitApplicationToAppMan(IActiveDataTransaction Tran, long InstanceID, int ApplicationKey);
        IX2ReturnData SendReminderEMail(int ApplicationKey, Int64 InstanceID);
        IX2ReturnData OnStartActivity_Direct_Consultant(IActiveDataTransaction Tran, Int64 InstanceID, string CreatorAdUser, int ApplicationKey, string DynamicRole, out string AssignedTo, out bool b);
        IX2ReturnData CreateCommissionableConsultantRole(IActiveDataTransaction Tran, int ApplicationKey, string ADUserName);
        IX2ReturnData Test(IActiveDataTransaction Tran);
    }
}
