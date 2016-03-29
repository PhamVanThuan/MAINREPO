using System;
using System.Collections.Generic;
using System.Text;
using SAHL.X2.Common;
using SAHL.X2.Common.DataAccess;

namespace X2DomainService.Interface.Origination
{
    public interface ICommon
    {
        IX2ReturnData GetnWorkingDaysFromToday(int nDays);
        IX2ReturnData GetCaseName(out string CaseName, int ApplicationKey);
        IX2ReturnData GetApplicatioIDFromSourceInstanceID(Int64 InstanceID, out int ApplicationID);
        IX2ReturnData GetPreviousOfferRoleKey(int ApplicationKey, string DynamicRole, out int Key);
        
        // Role related
        IX2ReturnData MakeApplicationRoleInactive(IActiveDataTransaction Tran, int ApplicationKey, List<int> ApplicationRoleTypeKeys);
        IX2ReturnData ResolveDynamicRoleToUserName(out string AssignedUser, int ApplicationKey, string DynamicRole, long InstanceID);
        IX2ReturnData GetUserWhoWorkedOnThisLEsOtherCasesForDynamicRole(out string ADUserName, int OfferRoleTypeKey, int ApplicationKey);
        IX2ReturnData RoundRobinAssignForGivenOrgStructure(IActiveDataTransaction Tran, out string AssignedUser, string DynamicRole, int GenericKey, int OrgStructureKey);
        IX2ReturnData RoundRobinAssignForGivenOrgStructure(IActiveDataTransaction Tran, out string AssignedUser, string DynamicRole, int GenericKey, List<int> OrgStructureKeys);
        IX2ReturnData ReassignOrEscalateCaseToUser(IActiveDataTransaction Tran, int GenericKey, string DynamicRole, string ADUser, bool MarkPreviousRoleAsInactive);
        IX2ReturnData AssignCreateorAsDynamicRole(IActiveDataTransaction Tran, out string AssignedTo, int ApplicationKey, long InstanceID, string DynamicRole);
        IX2ReturnData AssignToUserInSameBranch(IActiveDataTransaction Tran, string DynamicRole, int ApplicationKey, long InstanceID, bool UseCaseCreator, string ADUserName, string DynamicRolePrefix);
        IX2ReturnData MarkUsersAsInactive(IActiveDataTransaction Tran, int ApplicationKey, List<int> OfferRoleTypeKeys);
        IX2ReturnData UpdateAssignedUserInIDM(IActiveDataTransaction Tran, int ApplicationKey, bool IsFL);
        IX2ReturnData HasCaseBeenAssignedToThisDynamicRoleBefore(string DynamicRole, int ApplicationKey, out string user);
        IX2ReturnData GetApplicationRoleForCase(int GenericKey, string DynamicRole, out int ApplicationRoleKey);
        
        // Various
        IX2ReturnData GetFollowupTime(int MemoKey);
        IX2ReturnData UpdateParentVars(IActiveDataTransaction Tran, Int64 ChildInstanceID, Dictionary<string, object> dict);
        
        // Offer related
        IX2ReturnData UpdateOfferStatus(IActiveDataTransaction Tran, int ApplicationKey, int OfferStatus, int OfferInformationType);
        IX2ReturnData SetOfferEndDate(IActiveDataTransaction Tran, int ApplicationKey);
        IX2ReturnData RecalculateHouseHoldIncomeAndSave(IActiveDataTransaction Tran, int ApplicationKey);
        IX2ReturnData CreateNewRevision(IActiveDataTransaction Tran, int ApplicationKey);
        IX2ReturnData UpdateAccountStatus(IActiveDataTransaction Tran, int ApplicationKey, int AccountStatusKey);
        IX2ReturnData CreateAccountForApplication(IActiveDataTransaction Tran, int ApplicationKey);

        IX2ReturnData ValuationInProgress(IActiveDataTransaction Tran, Int64 InstanceID, int GenericKey, out bool b);
        IX2ReturnData SendSMSToMainApplicants(string Message, int ApplicationKey);
        IX2ReturnData HasInstancePerformedActivityBefore(Int64 InstanceID, string Activity);
        IX2ReturnData HasSourceInstancePerformedActivityBefore(Int64 SourceInstanceID, string Activity);
    }
}
