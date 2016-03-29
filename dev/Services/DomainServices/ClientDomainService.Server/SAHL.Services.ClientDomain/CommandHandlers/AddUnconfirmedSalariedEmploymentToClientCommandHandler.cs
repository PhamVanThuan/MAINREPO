using System;
using System.Linq;
using SAHL.Core.Data;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.ClientDomain.Managers;
using SAHL.Services.ClientDomain.Rules;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Events;
using SAHL.Services.Interfaces.ClientDomain.Models;
using SAHL.Core.Events;
using SAHL.Core.BusinessModel.Enums;

namespace SAHL.Services.ClientDomain.CommandHandlers
{
    public class AddUnconfirmedSalariedEmploymentToClientCommandHandler : IDomainServiceCommandHandler<AddUnconfirmedSalariedEmploymentToClientCommand, 
        UnconfirmedSalariedEmploymentAddedToClientEvent>
    {
        public IEmploymentDataManager EmploymentDataManager;
        private IClientDataManager ClientDataManager;
        public IDomainRuleManager<SalariedEmploymentModel> DomainRuleManager;
        private UnconfirmedSalariedEmploymentAddedToClientEvent @event;
        private IUnitOfWorkFactory uowFactory;
        private IEventRaiser eventRaiser;

        public AddUnconfirmedSalariedEmploymentToClientCommandHandler(IEmploymentDataManager employmentDataManager, IClientDataManager clientDataManager, 
            IDomainRuleManager<SalariedEmploymentModel> domainRuleManager, IUnitOfWorkFactory uowFactory, IEventRaiser eventRaiser)
        {
            this.EmploymentDataManager = employmentDataManager;
            this.DomainRuleManager = domainRuleManager;
            this.ClientDataManager = clientDataManager;
            this.uowFactory = uowFactory;
            this.eventRaiser = eventRaiser;
            this.DomainRuleManager.RegisterRule(new EmploymentMinimumDataRequiredRule<SalariedEmploymentModel>(EmploymentDataManager));
            this.DomainRuleManager.RegisterRule(new BasicIncomeIsRequiredRule<SalariedEmploymentModel>());
            this.DomainRuleManager.RegisterRule(new EmploymentStartDateMustBeBeforeTodayRule<SalariedEmploymentModel>());
            this.DomainRuleManager.RegisterPartialRule<IEmployeeDeductions>(new EmployeeDeductionsCanOnlyContainOneOfEachTypeRule());
        }

        public ISystemMessageCollection HandleCommand(AddUnconfirmedSalariedEmploymentToClientCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();
            this.DomainRuleManager.ExecuteRules(messages, command.SalariedEmploymentModel);
            if (messages.ErrorMessages().Any())
            {
                return messages;
            }
            using (var uow = uowFactory.Build())
            {
                int employmentKey = this.EmploymentDataManager.SaveSalariedEmployment(command.ClientKey, command.SalariedEmploymentModel);
                @event = new UnconfirmedSalariedEmploymentAddedToClientEvent(DateTime.Now, command.ClientKey, command.SalariedEmploymentModel.BasicIncome, 
                    command.SalariedEmploymentModel.StartDate,
                    command.SalariedEmploymentModel.EmploymentStatus, command.SalariedEmploymentModel.SalaryPaymentDay,
                    command.SalariedEmploymentModel.Employer.EmployerName, command.SalariedEmploymentModel.Employer.TelephoneCode,
                    command.SalariedEmploymentModel.Employer.TelephoneNumber, command.SalariedEmploymentModel.Employer.ContactPerson,
                    command.SalariedEmploymentModel.Employer.ContactEmail, command.SalariedEmploymentModel.Employer.EmployerBusinessType,
                    command.SalariedEmploymentModel.Employer.EmploymentSector, employmentKey);
                eventRaiser.RaiseEvent(DateTime.Now, @event, employmentKey, (int)GenericKeyType.Employment, metadata);

                uow.Complete();
            }
            return messages;
        }
    }
}