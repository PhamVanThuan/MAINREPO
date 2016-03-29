using System;
using DomainService2.Workflow.DebtCounselling;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.DebtCounselling;

namespace DomainService2.Hosts.DebtCounselling
{
    public class DebtCounsellingHost : HostBase, IDebtCounselling
    {
        public DebtCounsellingHost(ICommandHandler commandHandler)
            : base(commandHandler)
        {
        }

        public void NTUOpenPersonalLoan(IDomainMessageCollection messages, int debtCounsellingKey)
        {
            var command = new NTUOpenPersonalLoanCommand(debtCounsellingKey);
            this.CommandHandler.HandleCommand(messages, command);
        }

        public bool CheckSendToLitigationRules(IDomainMessageCollection messages, int debtCounsellingKey, bool ignoreWarnings)
        {
            var command = new CheckSendToLitigationRulesCommand(debtCounsellingKey, ignoreWarnings);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public void SendLitigationReminderInternalEmail(IDomainMessageCollection messages, int debtCounsellingKey)
        {
            var command = new SendLitigationReminderInternalEmailCommand(debtCounsellingKey);
            this.CommandHandler.HandleCommand(messages, command);
        }

        public bool UpdateDebtCounsellingStatus(IDomainMessageCollection messages, int debtCounsellingKey, int debtCounsellingStatusKey)
        {
            var command = new UpdateDebtCounsellingStatusCommand(debtCounsellingKey, debtCounsellingStatusKey);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public bool UpdateDebtCounsellingDebtReviewArrangement(IDomainMessageCollection messages, int accountKey, string userID)
        {
            var command = new UpdateDebtCounsellingDebtReviewArrangementCommand(accountKey, userID);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public bool CheckAcceptProposalRules(IDomainMessageCollection messages, int debtCounsellingKey, bool ignoreWarnings)
        {
            var command = new CheckAcceptProposalRulesCommand(debtCounsellingKey, ignoreWarnings);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public bool CheckPaymentReceivedRules(IDomainMessageCollection messages, int debtCounsellingKey, bool ignoreWarnings)
        {
            var command = new CheckPaymentReceivedRulesCommand(debtCounsellingKey, ignoreWarnings);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public bool CheckSendCounterProposalRules(IDomainMessageCollection messages, int debtCounsellingKey, bool ignoreWarnings)
        {
            var command = new CheckSendCounterProposalRulesCommand(debtCounsellingKey, ignoreWarnings);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public bool CheckSendDeclineLetterRules(IDomainMessageCollection messages, int debtCounsellingKey, bool ignoreWarnings)
        {
            var command = new CheckSendDeclineLetterRulesCommand(debtCounsellingKey, ignoreWarnings);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public bool CheckSendProposalForApprovalRules(IDomainMessageCollection messages, int debtCounsellingKey, bool ignoreWarnings)
        {
            var command = new CheckSendProposalForApprovalRulesCommand(debtCounsellingKey, ignoreWarnings);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public bool UpdateActiveAcceptedProposal(IDomainMessageCollection messages, int debtCounsellingKey, bool accepted)
        {
            var command = new UpdateActiveAcceptedProposalCommand(debtCounsellingKey, accepted);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public void CancelDebtCounselling(IDomainMessageCollection messages, int debtCounsellingKey, int latestReasonDefintionKey)
        {
            CancelDebtCounsellingCommand command = new CancelDebtCounsellingCommand(debtCounsellingKey, latestReasonDefintionKey);
            this.CommandHandler.HandleCommand<CancelDebtCounsellingCommand>(messages, command);
        }

        public bool CancelDebtCounselling_EXT(IDomainMessageCollection messages, int debtCounsellingKey, long instanceID, string activityName)
        {
            var command = new CancelDebtCounselling_EXTCommand(instanceID, activityName, debtCounsellingKey);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public bool TerminateDebtCounselling(IDomainMessageCollection messages, int debtCounsellingKey, string userID)
        {
            var command = new TerminateDebtCounsellingCommand(debtCounsellingKey, userID);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public bool CheckFiveDaysTerminationReminderRules(IDomainMessageCollection messages, int debtCounsellingKey, bool ignoreWarnings)
        {
            var command = new CheckFiveDaysTerminationReminderRulesCommand(debtCounsellingKey, ignoreWarnings);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public bool CheckNoDateNoPayRules(IDomainMessageCollection messages, int debtCounsellingKey, bool ignoreWarnings)
        {
            var command = new CheckNoDateNoPayRulesCommand(debtCounsellingKey, ignoreWarnings);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public bool CheckFortyFiveDayReminderRules(IDomainMessageCollection messages, int debtCounsellingKey, bool ignoreWarnings)
        {
            var command = new CheckFortyFiveDayReminderRulesCommand(debtCounsellingKey, ignoreWarnings);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public void GetEworkDataForLossControlCase(IDomainMessageCollection messages, int accountKey, out string eStageName, out string eFolderID, out string eADUserName)
        {
            eStageName = string.Empty;
            eFolderID = string.Empty;
            eADUserName = string.Empty;

            var command = new GetEworkDataForLossControlCaseCommand(accountKey);
            this.CommandHandler.HandleCommand(messages, command);

            eStageName = command.eStageName;
            eFolderID = command.eFolderID;
            eADUserName = command.eADUserName;
        }

        public DateTime? GetReviewDate(IDomainMessageCollection messages, int debtCounsellingKey)
        {
            var command = new GetReviewDateCommand(debtCounsellingKey);
            this.CommandHandler.HandleCommand<GetReviewDateCommand>(messages, command);
            return command.ReviewDateResult.Value;
        }

        public DateTime? GetSeventeenPointOneDateDays(IDomainMessageCollection messages, int debtCounsellingKey, int days)
        {
            var command = new GetSeventeenPointOneDateDaysCommand(debtCounsellingKey, days);
            this.CommandHandler.HandleCommand<GetSeventeenPointOneDateDaysCommand>(messages, command);
            return command.SeventeenPointOneDatePlusDaysResult;
        }

        public bool CheckTenDaysTerminationReminderRules(IDomainMessageCollection messages, int debtCounsellingKey, bool ignoreWarnings)
        {
            var command = new CheckTenDaysTerminationReminderRulesCommand(debtCounsellingKey, ignoreWarnings);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public bool RollbackTransaction(IDomainMessageCollection messages, int debtCounsellingKey)
        {
            var command = new RollbackTransactionCommand(debtCounsellingKey);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public bool CheckSendTerminationLetterRules(IDomainMessageCollection messages, int debtCounsellingKey, bool ignoreWarnings)
        {
            var command = new CheckSendTerminationLetterRulesCommand(debtCounsellingKey, ignoreWarnings);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public bool CheckAttorneyToOpposeRules(IDomainMessageCollection messages, int debtCounsellingKey, bool ignoreWarnings)
        {
            var command = new CheckAttorneyToOpposeRulesCommand(debtCounsellingKey, ignoreWarnings);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public void SendNotificationsCancellatonRegistered_EXT(IDomainMessageCollection messages, int debtCounsellingKey, bool recoveriesProposalReceivedStageTransitionExists)
        {
            var command = new SendNotificationsCancellatonRegistered_EXTCommand(debtCounsellingKey, recoveriesProposalReceivedStageTransitionExists);
            this.CommandHandler.HandleCommand<SendNotificationsCancellatonRegistered_EXTCommand>(messages, command);
        }

        public bool ProcessDebtCounsellingOptOut(IDomainMessageCollection messages, int accountKey, string userID)
        {
            var command = new ProcessDebtCounsellingOptOutCommand(accountKey, userID);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public bool ConvertDebtCounselling(IDomainMessageCollection messages, int accountKey, string userID)
        {
            var command = new ConvertDebtCounsellingCommand(accountKey, userID);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public bool DebtCounsellingOptOutRequired(IDomainMessageCollection messages, int accountKey)
        {
            var command = new DebtCounsellingOptOutRequiredCommand(accountKey);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public string GetInstanceSubjectForDebtCounselling(IDomainMessageCollection messages, int genericKey)
        {
            var command = new GetInstanceSubjectForDebtCounsellingCommand(genericKey);
            this.CommandHandler.HandleCommand<GetInstanceSubjectForDebtCounsellingCommand>(messages, command);
            return command.LegalEntityNameResult;
        }
        public bool UpdateHearingDetailStatusToInactive(IDomainMessageCollection messages, int debtCounsellingKey)
        {
            var command = new UpdateHearingDetailStatusToInactiveCommand(debtCounsellingKey);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public void SendNotification(IDomainMessageCollection messages, int debtCounsellingKey)
        {
            var command = new SendNotificationCommand(debtCounsellingKey);
        }
    }
}