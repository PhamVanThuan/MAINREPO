using System.Collections.Generic;
using DomainService2.Workflow.Origination.Valuations;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.Origination;

namespace DomainService2.Hosts.Origination.Valuations
{
    public class ValuationsHost : HostBase, IValuations
    {
        public ValuationsHost(ICommandHandler commandHandler)
            : base(commandHandler)
        {
        }

        public Dictionary<string, object> GetValuationData(IDomainMessageCollection messages, int applicationKey)
        {
            var command = new GetValuationDataCommand(applicationKey);
            this.CommandHandler.HandleCommand(messages, command);
            return command.ValuationDataResult;
        }

        public void SendEmailToAllOpenApplicationConsultantsForValuationComplete(IDomainMessageCollection messages, int valuationKey, int applicationKey)
        {
            var command = new SendEmailToAllOpenApplicationConsultantsForValuationCompleteCommand(valuationKey, applicationKey);
            this.CommandHandler.HandleCommand(messages, command);
        }

        public void SetValuationStatus(IDomainMessageCollection messages, int valuationKey, int statusKey)
        {
            var command = new SetValuationStatusCommand(valuationKey, statusKey);
            this.CommandHandler.HandleCommand(messages, command);
        }

        public void SetValuationIsActive(IDomainMessageCollection messages, int valuationKey)
        {
            var command = new SetValuationIsActiveCommand(valuationKey);
            this.CommandHandler.HandleCommand(messages, command);
        }

        public void SetValuationActiveAndSave(IDomainMessageCollection messages, int valuationKey)
        {
            var command = new SetValuationActiveAndSaveCommand(valuationKey);
            this.CommandHandler.HandleCommand(messages, command);
        }

        public void RecalcHOC(IDomainMessageCollection messages, int valuationKey, int applicationKey, bool ignoreWarnings)
        {
            var command = new RecalcHOCCommand(valuationKey, applicationKey, ignoreWarnings);
            this.CommandHandler.HandleCommand(messages, command);
        }

        public bool CheckIfCanPerformFurtherValuation(IDomainMessageCollection messages, long appManInstanceID)
        {
            var command = new CheckIfCanPerformFurtherValuationCommand(appManInstanceID);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public bool CheckValuationExistsRecentRules(IDomainMessageCollection messages, int applicationKey, bool ignoreWarnings)
        {
            var command = new CheckValuationExistsRecentRulesCommand(applicationKey, ignoreWarnings);
            base.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public bool CheckValuationInOrderRules(IDomainMessageCollection messages, int valuationKey, bool ignoreWarnings)
        {
            var command = new CheckValuationInOrderRulesCommand(valuationKey, ignoreWarnings);
            base.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }
    }
}