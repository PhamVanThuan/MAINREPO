using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.ClientDomain.Managers;
using SAHL.Services.ClientDomain.Rules;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Events;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.Linq;

namespace SAHL.Services.ClientDomain.CommandHandlers
{
    public class AddUnconfirmedUnemployedEmploymentToClientCommandHandler : IDomainServiceCommandHandler<AddUnconfirmedUnemployedEmploymentToClientCommand,
        UnconfirmedUnemployedEmploymentAddedToClientEvent>
    {
        public IEmploymentDataManager employmentDataManager;
        public IDomainRuleManager<UnemployedEmploymentModel> domainRuleManager;
        private UnconfirmedUnemployedEmploymentAddedToClientEvent @event;
        private IUnitOfWorkFactory uowFactory;
        private IEventRaiser eventRaiser;

        public AddUnconfirmedUnemployedEmploymentToClientCommandHandler(IEmploymentDataManager employmentDataManager, IDomainRuleManager<UnemployedEmploymentModel> domainRuleManager,
            IUnitOfWorkFactory uowFactory, IEventRaiser eventRaiser)
        {
            this.employmentDataManager = employmentDataManager;
            this.domainRuleManager = domainRuleManager;
            this.uowFactory = uowFactory;
            this.eventRaiser = eventRaiser;
            this.domainRuleManager.RegisterRule(new EmploymentMinimumDataRequiredRule<UnemployedEmploymentModel>(employmentDataManager));
            this.domainRuleManager.RegisterRule(new BasicIncomeIsRequiredRule<UnemployedEmploymentModel>());
            this.domainRuleManager.RegisterRule(new EmploymentStartDateMustBeBeforeTodayRule<UnemployedEmploymentModel>());
        }

        public ISystemMessageCollection HandleCommand(AddUnconfirmedUnemployedEmploymentToClientCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();
            this.domainRuleManager.ExecuteRules(messages, command.UnemployedEmployment);
            if (messages.ErrorMessages().Any())
            {
                return messages;
            }
            using (var uow = uowFactory.Build())
            {
                int employmentKey = this.employmentDataManager.SaveUnemployedEmployment(command.ClientKey, command.UnemployedEmployment);
                @event = new UnconfirmedUnemployedEmploymentAddedToClientEvent(DateTime.Now, command.ClientKey, command.UnemployedEmployment.BasicIncome,
                    command.UnemployedEmployment.StartDate,
                    command.UnemployedEmployment.EmploymentStatus, command.UnemployedEmployment.SalaryPaymentDay,
                    command.UnemployedEmployment.Employer.EmployerName, employmentKey);
                eventRaiser.RaiseEvent(DateTime.Now, @event, employmentKey, (int)GenericKeyType.Employment, metadata);

                uow.Complete();
            }
            return messages;
        }
    }
}