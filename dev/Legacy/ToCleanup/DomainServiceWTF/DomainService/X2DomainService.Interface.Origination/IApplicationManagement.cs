using System;
using System.Collections.Generic;
using System.Text;
using SAHL.X2.Common;
using SAHL.X2.Common.DataAccess;

namespace X2DomainService.Interface.Origination
{
    public interface IApplicationManagement
    {
        IX2ReturnData SendNTUFinalResubMail(int ApplicationKey);
        IX2ReturnData SendEmailToConsultantForQAComplete(int GenericKey, Int64 InstanceID, string ReasonTypeGroup, out bool b);
        IX2ReturnData SendEmailToConsultantForQuery(int GenericKey, Int64 InstanceID, string ReasonTypeGroup, out bool b);

        IX2ReturnData SaveApplicationForValidation(IActiveDataTransaction Tran, int ApplicationKey, out bool b);
        IX2ReturnData GetOfferType(int ApplicationKey, out int OfferTypeKey);

        IX2ReturnData RefreshAppDocuments(IActiveDataTransaction Tran, int ApplicationKey);
        IX2ReturnData CheckCreditSubmissionRules(int ApplicationKey, bool IgnoreWarnings, bool IsFL, bool IsResub, out bool b);
        IX2ReturnData OverRideCheck(int ApplicationKey, bool IgnoreWarnings, bool IsFL);
        IX2ReturnData ResubOverRideCheck(int ApplicationKey, bool IgnoreWarnings);
        IX2ReturnData LOAAccepted(int ApplicationKey, bool IgnoreMessages, out bool b);

        IX2ReturnData ArchiveApplicationManagementChildren(IActiveDataTransaction Tran, Int64 InstanceID, string ADUser);
        IX2ReturnData RemoveDetailTypes(IActiveDataTransaction Tran, int ApplicationKey);
        IX2ReturnData RollbackDisbursment(IActiveDataTransaction Tran, int ApplicationKey);
        IX2ReturnData TranslateCondition(int ApplicationKey, Int64 InstanceID, bool IsFL, out bool b);
        IX2ReturnData DeclineByCreditConsultantEMail(int ApplicationKey);
        IX2ReturnData EMailValuationDone(int ApplicationKey, Int64 InstanceID);
        IX2ReturnData ReturnNonDisbursedLoanToProspect(IActiveDataTransaction Tran, int ApplicationKey);
        IX2ReturnData CreateAccountForApplicaiton(IActiveDataTransaction Tran, int ApplicationKey);
        IX2ReturnData CheckCanMoveToDisbursement(int ApplicationKey, bool IgnoreMessages, out bool b);
        IX2ReturnData NTUCase(IActiveDataTransaction Tran, int GenericKey, out bool b);
        IX2ReturnData RemoveNonNCAComplientAccountInformation(IActiveDataTransaction Tran, int ApplicationKey);

        #region EWork Stuff
        IX2ReturnData AddDetailTypeInstructionSent(IActiveDataTransaction Tran, int ApplicationKey);
        IX2ReturnData CreateEWorkCase(int ApplicationKey, out string EFolderID, out bool b);
        IX2ReturnData PerformEWorkAction(string EWorkFolderID, string ActionToPerform, int GenericKey, string AssignedUser);
        IX2ReturnData CheckEWorkAtCorrectState(int ApplicationKey, out bool b);
        #endregion
    }
}
