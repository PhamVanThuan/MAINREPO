namespace DomainService2.Hosts.Life
{
    using System;
    using DomainService2.Workflow.Life;
    using SAHL.Common.Collections.Interfaces;
    using X2DomainService.Interface.Life;

    public class LifeHost : HostBase, ILife
    {
        public LifeHost(ICommandHandler commandHandler)
            : base(commandHandler)
        {
        }

        public DateTime GetActivityTimeWaitForCallback(IDomainMessageCollection messages, int applicationKey)
        {
            GetActivityTimeWaitForCallbackCommand command = new GetActivityTimeWaitForCallbackCommand(applicationKey);
            this.CommandHandler.HandleCommand<GetActivityTimeWaitForCallbackCommand>(messages, command);
            return command.ActivityTimeResult;
        }

        public bool ContinueSale(IDomainMessageCollection messages, int applicationKey)
        {
            ContinueSaleCommand command = new ContinueSaleCommand(applicationKey);
            this.CommandHandler.HandleCommand<ContinueSaleCommand>(messages, command);
            return command.Result;
        }

        public void AwaitingConfirmationTimeout(IDomainMessageCollection messages, int applicationKey)
        {
            AwaitingConfirmationTimeoutCommand command = new AwaitingConfirmationTimeoutCommand(applicationKey);
            this.CommandHandler.HandleCommand<AwaitingConfirmationTimeoutCommand>(messages, command);
        }

        public void AcceptBenefits(IDomainMessageCollection messages, int applicationKey)
        {
            AcceptBenefitsCommand command = new AcceptBenefitsCommand(applicationKey);
            this.CommandHandler.HandleCommand<AcceptBenefitsCommand>(messages, command);
        }

        public void OlderThan45Days(IDomainMessageCollection messages, int applicationKey, long instanceID)
        {
            OlderThan45DaysCommand command = new OlderThan45DaysCommand(applicationKey, instanceID);
            this.CommandHandler.HandleCommand<OlderThan45DaysCommand>(messages, command);
        }

        public void ActivateLifePolicy(IDomainMessageCollection messages, int applicationKey)
        {
            ActivateLifePolicyCommand command = new ActivateLifePolicyCommand(applicationKey);
            this.CommandHandler.HandleCommand<ActivateLifePolicyCommand>(messages, command);
        }

        public void CreateInstance(IDomainMessageCollection messages, int loanNumber, long instanceID, string assignTo, out int applicationKey, out string name, out string subject, out int priority)
        {
            applicationKey = -1;
            name = string.Empty;
            subject = string.Empty;
            priority = -1;

            CreateInstanceCommand command = new CreateInstanceCommand(loanNumber, instanceID, assignTo);
            this.CommandHandler.HandleCommand(messages, command);

            applicationKey = command.ApplicationKey;
            name = command.Name;
            subject = command.Subject;
            priority = command.Priority;
        }

        public void PolicyNTUd(IDomainMessageCollection messages, int applicationKey)
        {
            PolicyNTUdCommand command = new PolicyNTUdCommand(applicationKey);
            this.CommandHandler.HandleCommand<PolicyNTUdCommand>(messages, command);
        }

        public void ReadyToCallback(IDomainMessageCollection messages, int applicationKey, long instanceID)
        {
            ReadyToCallbackCommand command = new ReadyToCallbackCommand(applicationKey, instanceID);
            this.CommandHandler.HandleCommand<ReadyToCallbackCommand>(messages, command);
        }

        public void DeclineQuote(IDomainMessageCollection messages, int applicationKey)
        {
            DeclineQuoteCommand command = new DeclineQuoteCommand(applicationKey);
            this.CommandHandler.HandleCommand<DeclineQuoteCommand>(messages, command);
        }

        public void NTUPolicy(IDomainMessageCollection messages, int applicationKey)
        {
            NTUPolicyCommand command = new NTUPolicyCommand(applicationKey);
            this.CommandHandler.HandleCommand<NTUPolicyCommand>(messages, command);
        }

        public void ReactivatePolicy(IDomainMessageCollection messages, int applicationKey)
        {
            ReactivatePolicyCommand command = new ReactivatePolicyCommand(applicationKey);
            this.CommandHandler.HandleCommand<ReactivatePolicyCommand>(messages, command);
        }

        public int GetPolicyTypeKeyForOfferLife(IDomainMessageCollection messages, int applicationKey)
        {
            GetPolicyTypeKeyForOfferLifeCommand command = new GetPolicyTypeKeyForOfferLifeCommand(applicationKey);
            this.CommandHandler.HandleCommand<GetPolicyTypeKeyForOfferLifeCommand>(messages, command);

            return command.PolicyTypeKeyResult;
        }
    }
}