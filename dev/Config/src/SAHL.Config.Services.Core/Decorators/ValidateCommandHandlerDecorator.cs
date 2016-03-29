using System.ComponentModel.DataAnnotations;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Validation;

namespace SAHL.Config.Services.Core.Decorators

{
    public class ValidateCommandHandlerDecorator<TCommand> : IServiceCommandHandlerDecorator<TCommand> where TCommand : IServiceCommand
    {
        private IValidateCommand CommandValidator { get; set; }

        private IServiceCommandHandler<TCommand> innerHandler;

        public IServiceCommandHandler<TCommand> InnerCommandHandler
        {
            get { return this.innerHandler; }
        }

        public ValidateCommandHandlerDecorator(IServiceCommandHandler<TCommand> innerHandler, IValidateCommand commandValidator)
        {
            CommandValidator = commandValidator;
            this.innerHandler = innerHandler;
        }

        public ISystemMessageCollection HandleCommand(TCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection messages = ValidateIfPossible(command);

            if (!messages.HasErrors)
            {
                messages.Aggregate(innerHandler.HandleCommand(command, metadata));
            }

            return messages;
        }

        private SystemMessageCollection ValidateIfPossible(TCommand command)
        {
            var messages = new SystemMessageCollection();

            foreach (var validationResult in CommandValidator.Validate(command))
            {
                messages.AddMessage(CreateMessageFromValidationresult(validationResult));
            }
            return messages;
        }

        private static SystemMessage CreateMessageFromValidationresult(ValidationResult validationResult)
        {
            return new SystemMessage(
                string.Format("There was a validation error: [{0}], processing has been halted",
                    validationResult.ErrorMessage), SystemMessageSeverityEnum.Error);
        }
    }
}