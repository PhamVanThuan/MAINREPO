using System;
using SAHL.Common.Collections.Interfaces;
using SAHL.X2.Common;

namespace X2DomainService.Interface.DebtCounselling
{
    public interface IDebtCounselling : IX2WorkflowService
    {
        void NTUOpenPersonalLoan(IDomainMessageCollection messages, int debtCounsellingKey);

        bool CheckSendToLitigationRules(IDomainMessageCollection messages, int debtCounsellingKey, bool ignoreWarnings);

        void SendLitigationReminderInternalEmail(IDomainMessageCollection messages, int debtCounsellingKey);

        bool UpdateDebtCounsellingStatus(IDomainMessageCollection messages, int debtCounsellingKey, int debtCounsellingStatusKey);

        bool UpdateDebtCounsellingDebtReviewArrangement(IDomainMessageCollection messages, int accountKey, string userID);

        bool CheckAcceptProposalRules(IDomainMessageCollection messages, int debtCounsellingKey, bool ignoreWarnings);

        bool CheckPaymentReceivedRules(IDomainMessageCollection messages, int debtCounsellingKey, bool ignoreWarnings);

        bool CheckSendCounterProposalRules(IDomainMessageCollection messages, int debtCounsellingKey, bool ignoreWarnings);

        bool CheckSendDeclineLetterRules(IDomainMessageCollection messages, int debtCounsellingKey, bool ignoreWarnings);

        bool CheckSendProposalForApprovalRules(IDomainMessageCollection messages, int debtCounsellingKey, bool ignoreWarnings);

        bool UpdateActiveAcceptedProposal(IDomainMessageCollection messages, int debtCounsellingKey, bool accepted);

        void CancelDebtCounselling(IDomainMessageCollection messages, int debtCounsellingKey, int latestReasonDefintionKey);

        bool CancelDebtCounselling_EXT(IDomainMessageCollection messages, int debtCounsellingKey, long instanceID, string activityName);

        bool TerminateDebtCounselling(IDomainMessageCollection messages, int debtCounsellingKey, string userID);

        bool CheckFiveDaysTerminationReminderRules(IDomainMessageCollection messages, int debtCounsellingKey, bool ignoreWarnings);

        bool CheckNoDateNoPayRules(IDomainMessageCollection messages, int debtCounsellingKey, bool ignoreWarnings);

        bool CheckFortyFiveDayReminderRules(IDomainMessageCollection messages, int debtCounsellingKey, bool ignoreWarnings);

        void GetEworkDataForLossControlCase(IDomainMessageCollection messages, int accountKey, out string eStageName, out string eFolderID, out string ADUserName);

        DateTime? GetReviewDate(IDomainMessageCollection messages, int debtCounsellingKey);

        DateTime? GetSeventeenPointOneDateDays(IDomainMessageCollection messages, int debtCounsellingKey, int days);

        bool CheckTenDaysTerminationReminderRules(IDomainMessageCollection messages, int debtCounsellingKey, bool ignoreWarnings);

        bool RollbackTransaction(IDomainMessageCollection messages, int debtCounsellingKey);

        bool CheckSendTerminationLetterRules(IDomainMessageCollection messages, int debtCounsellingKey, bool ignoreWarnings);

        bool CheckAttorneyToOpposeRules(IDomainMessageCollection messages, int debtCounsellingKey, bool ignoreWarnings);

        void SendNotificationsCancellatonRegistered_EXT(IDomainMessageCollection messages, int debtCounsellingKey, bool recoveriesProposalReceivedStageTransitionExists);

        bool ProcessDebtCounsellingOptOut(IDomainMessageCollection messages, int accountKey, string userID);

        bool ConvertDebtCounselling(IDomainMessageCollection messages, int accountKey, string userID);

        bool DebtCounsellingOptOutRequired(IDomainMessageCollection messages, int accountKey);

        string GetInstanceSubjectForDebtCounselling(IDomainMessageCollection messages, int genericKey);

        bool UpdateHearingDetailStatusToInactive(IDomainMessageCollection messages, int debtCounsellingKey);

        void SendNotification(IDomainMessageCollection messages, int debtCounsellingKey);
    }
}