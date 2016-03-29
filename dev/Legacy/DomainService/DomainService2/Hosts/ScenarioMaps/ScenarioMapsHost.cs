using DomainService2.Workflow.ScenarioMaps;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.ScenarioMaps;

namespace DomainService2.Hosts.ScenarioMaps
{
    public class ScenarioMapsHost : HostBase, IScenarioMaps
    {
        public ScenarioMapsHost(ICommandHandler commandHandler)
            : base(commandHandler)
        {
        }

        public void ThrowDAO_Exception(IDomainMessageCollection messages, bool ignoreWarnings)
        {
            ThrowDAO_ExceptionCommand command = new ThrowDAO_ExceptionCommand(ignoreWarnings);
            this.CommandHandler.HandleCommand(messages, command);
        }

        public void ThrowSqlException(IDomainMessageCollection messages, bool ignoreWarnings)
        {
            ThrowSqlExceptionCommand command = new ThrowSqlExceptionCommand(ignoreWarnings);
            this.CommandHandler.HandleCommand(messages, command);
        }

        public void ThrowSqlTimeOutException(IDomainMessageCollection messages, bool ignoreWarnings)
        {
            ThrowSqlTimeOutExceptionCommand command = new ThrowSqlTimeOutExceptionCommand(ignoreWarnings);
            this.CommandHandler.HandleCommand(messages, command);
        }

        public void ThrowDomainValidationException(IDomainMessageCollection messages, bool ignoreWarnings)
        {
            ThrowDomainValidationExceptionCommand command = new ThrowDomainValidationExceptionCommand(ignoreWarnings);
            this.CommandHandler.HandleCommand(messages, command);
        }

        public void ThrowDomainMessageException(IDomainMessageCollection messages, bool ignoreWarnings)
        {
            ThrowDomainMessageExceptionCommand command = new ThrowDomainMessageExceptionCommand(ignoreWarnings);
            this.CommandHandler.HandleCommand(messages, command);
        }

        public void ThrowExceptionWithMessages(IDomainMessageCollection messages, bool ignoreWarnings)
        {
            ThrowExceptionWithMessagesCommand command = new ThrowExceptionWithMessagesCommand(ignoreWarnings);
            this.CommandHandler.HandleCommand(messages, command);
        }

        public void ThrowExceptionWithoutMessages(IDomainMessageCollection messages, bool ignoreWarnings)
        {
            ThrowExceptionWithoutMessagesCommand command = new ThrowExceptionWithoutMessagesCommand(ignoreWarnings);
            this.CommandHandler.HandleCommand(messages, command);
        }

        public void ThrowSqlApiException(IDomainMessageCollection messages, bool ignoreWarnings)
        {
            ThrowSqlApiExceptionCommand command = new ThrowSqlApiExceptionCommand(ignoreWarnings);
            this.CommandHandler.HandleCommand(messages, command);
        }
    }
}