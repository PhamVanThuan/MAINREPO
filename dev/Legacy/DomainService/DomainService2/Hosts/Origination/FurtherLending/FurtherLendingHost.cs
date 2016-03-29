using DomainService2.Workflow.Origination.FurtherLending;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.Origination;

namespace DomainService2.Hosts.Origination.FurtherLending
{
    public class FurtherLendingHost : HostBase, IFL
    {
        public FurtherLendingHost(ICommandHandler commandHandler)
            : base(commandHandler)
        {
        }

        public bool IsFurtherLoanApplication(IDomainMessageCollection messages, int applicationKey)
        {
            IsFurtherLoanApplicationCommand command = new IsFurtherLoanApplicationCommand(applicationKey);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public bool IsFurtherAdvanceApplication(IDomainMessageCollection messages, int applicationKey)
        {
            IsFurtherAdvanceApplicationCommand command = new IsFurtherAdvanceApplicationCommand(applicationKey);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public bool IsReadvanceAdvanceApplication(IDomainMessageCollection messages, int applicationKey)
        {
            IsReadvanceAdvanceApplicationCommand command = new IsReadvanceAdvanceApplicationCommand(applicationKey);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public void RemoveDetailTypes(IDomainMessageCollection messages, int applicationKey)
        {
            RemoveDetailTypesCommand command = new RemoveDetailTypesCommand(applicationKey);
            this.CommandHandler.HandleCommand<RemoveDetailTypesCommand>(messages, command);
        }

        public void ArchiveFLRelatedCases(IDomainMessageCollection messages, long instanceID, int applicationKey, string ADUser)
        {
            ArchiveFLRelatedCasesCommand command = new ArchiveFLRelatedCasesCommand(applicationKey, ADUser, instanceID);
            this.CommandHandler.HandleCommand<ArchiveFLRelatedCasesCommand>(messages, command);
        }

        public void FLCompleteUnholdNextApplicationWhereApplicable(IDomainMessageCollection messages, int applicationKey)
        {
            FLCompleteUnholdNextApplicationWhereApplicableCommand command = new FLCompleteUnholdNextApplicationWhereApplicableCommand(applicationKey);
            this.CommandHandler.HandleCommand<FLCompleteUnholdNextApplicationWhereApplicableCommand>(messages, command);
        }

        public void AddDetailTypes(IDomainMessageCollection messages, int applicationKey, string ADUser)
        {
            AddDetailTypesCommand command = new AddDetailTypesCommand(applicationKey, ADUser);
            this.CommandHandler.HandleCommand<AddDetailTypesCommand>(messages, command);
        }

        public bool AppsInProgOfHigherPri(IDomainMessageCollection messages, int applicationKey)
        {
            AppsInProgOfHigherPriCommand command = new AppsInProgOfHigherPriCommand(applicationKey);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public bool HighestPriority(IDomainMessageCollection messages, int applicationKey)
        {
            HighestPriorityCommand command = new HighestPriorityCommand(applicationKey);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public bool ValuationRequired(IDomainMessageCollection messages, int applicationKey)
        {
            ValuationRequiredCommand command = new ValuationRequiredCommand(applicationKey);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        //public double LTVForFLCase(IDomainMessageCollection messages, int applicationKey)
        //{
        //    LTVForFLCaseCommand command = new LTVForFLCaseCommand(applicationKey);
        //    this.CommandHandler.HandleCommand<LTVForFLCaseCommand>(messages, command);
        //    return command.LTV;
        //}

        public void InitialFLNTU(IDomainMessageCollection messages, string ADUser, int applicationKey, long instanceID)
        {
            InitialFLNTUCommand command = new InitialFLNTUCommand(applicationKey, ADUser, instanceID);
            this.CommandHandler.HandleCommand<InitialFLNTUCommand>(messages, command);
        }

        public void AddAccountMemoMessageOnReceiptOfApplication(IDomainMessageCollection messages, int applicationKey, string ADUser, string assignedTo)
        {
            AddAccountMemoMessageOnReceiptOfApplicationCommand command = new AddAccountMemoMessageOnReceiptOfApplicationCommand(applicationKey, ADUser, assignedTo);
            this.CommandHandler.HandleCommand<AddAccountMemoMessageOnReceiptOfApplicationCommand>(messages, command);
        }

        public void SuretySignedConfirmed(IDomainMessageCollection messages, int applicationKey)
        {
            SuretySignedConfirmedCommand command = new SuretySignedConfirmedCommand(applicationKey);
            this.CommandHandler.HandleCommand<SuretySignedConfirmedCommand>(messages, command);
        }

        public bool CanRollbackReadvanceCorrectionTransaction(IDomainMessageCollection messages, int applicationKey)
        {
            CanRollbackReadvanceCorrectionTransactionCommand command = new CanRollbackReadvanceCorrectionTransactionCommand(applicationKey);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public bool CheckCanDisburseReadvanceRules(IDomainMessageCollection messages, int applicationKey, bool ignoreWarnings)
        {
            CheckCanDisburseReadvanceRulesCommand command = new CheckCanDisburseReadvanceRulesCommand(applicationKey, ignoreWarnings);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public bool CheckInformClientRule(IDomainMessageCollection messages, int applicationKey, bool ignoreWarnings)
        {
            CheckInformClientRuleCommand command = new CheckInformClientRuleCommand(applicationKey, ignoreWarnings);
            this.CommandHandler.HandleCommand<CheckInformClientRuleCommand>(messages, command);
            return command.Result;
        }

        public bool CheckRapidShouldGotoCreditRules(IDomainMessageCollection messages, int applicationKey, bool ignoreWarnings)
        {
            CheckRapidShouldGotoCreditRulesCommand command = new CheckRapidShouldGotoCreditRulesCommand(applicationKey, ignoreWarnings);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public bool CheckSuretyForReAdvanceRules(IDomainMessageCollection messages, int applicationKey, bool ignoreWarnings)
        {
            CheckSuretyForReAdvanceRulesCommand command = new CheckSuretyForReAdvanceRulesCommand(applicationKey, ignoreWarnings);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public bool CheckNTUDeclineFinalRules(IDomainMessageCollection messages, int applicationKey, bool ignoreWarnings)
        {
            CheckNTUDeclineFinalRulesCommand command = new CheckNTUDeclineFinalRulesCommand(applicationKey, ignoreWarnings);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        //public bool CheckOptOutSuperLoRules(IDomainMessageCollection messages, int applicationKey, bool ignoreWarnings)
        //{
        //    CheckOptOutSuperLoRulesCommand command = new CheckOptOutSuperLoRulesCommand(applicationKey, ignoreWarnings);
        //    this.CommandHandler.HandleCommand(messages, command);
        //    return command.Result;
        //}

        public bool CheckSuperLoOptOutRequiredRules(IDomainMessageCollection messages, int applicationKey, bool ignoreWarnings)
        {
            CheckSuperLoOptOutRequiredRulesCommand command = new CheckSuperLoOptOutRequiredRulesCommand(applicationKey, ignoreWarnings);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public bool CheckSuperLoOptOutRequiredWithNoMessagesRules(IDomainMessageCollection messages, int applicationKey, bool ignoreWarnings)
        {
            CheckSuperLoOptOutRequiredWithNoMessagesRulesCommand command = new CheckSuperLoOptOutRequiredWithNoMessagesRulesCommand(applicationKey, ignoreWarnings);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public bool CheckIsFurtherAdvanceBelowLAARules(IDomainMessageCollection messages, int applicationKey, bool ignoreWarnings)
        {
            CheckIsFurtherAdvanceBelowLAARulesCommand command = new CheckIsFurtherAdvanceBelowLAARulesCommand(applicationKey, ignoreWarnings);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public bool OptOutSuperLo(IDomainMessageCollection messages, int applicationKey, string aDUser)
        {
            OptOutSuperLoCommand command = new OptOutSuperLoCommand(applicationKey, aDUser);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }
    }
}