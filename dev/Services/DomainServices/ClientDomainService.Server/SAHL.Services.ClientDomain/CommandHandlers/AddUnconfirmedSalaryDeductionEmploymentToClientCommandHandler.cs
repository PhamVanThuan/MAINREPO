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
    public class AddUnconfirmedSalaryDeductionEmploymentToClientCommandHandler : IDomainServiceCommandHandler<AddUnconfirmedSalaryDeductionEmploymentToClientCommand, 
        UnconfirmedSalaryDeductionEmploymentAddedToClientEvent>
    {
        private IEmploymentDataManager EmploymentDataManager;
        private IClientDataManager ClientDataManager;
        private IDomainRuleManager<SalaryDeductionEmploymentModel> DomainRuleManager;
        private UnconfirmedSalaryDeductionEmploymentAddedToClientEvent @event;
        private IUnitOfWorkFactory uowFactory;
        private IEventRaiser eventRaiser;

        public AddUnconfirmedSalaryDeductionEmploymentToClientCommandHandler(IEmploymentDataManager employmentDataManager, IClientDataManager clientDataManager, 
            IDomainRuleManager<SalaryDeductionEmploymentModel> domainRuleManager, IUnitOfWorkFactory uowFactory, IEventRaiser eventRaiser)
        {
            this.EmploymentDataManager = employmentDataManager;
            this.ClientDataManager = clientDataManager;
            this.DomainRuleManager = domainRuleManager;
            this.uowFactory = uowFactory;
            this.eventRaiser = eventRaiser;

            domainRuleManager.RegisterRuleForContext(new HousingAllowanceIsRequiredForSalaryDeductionRule(), OriginationSource.SAHomeLoans);
            domainRuleManager.RegisterRuleForContext(new HousingAllowanceIsRequiredForSalaryDeductionRule(), OriginationSource.Capitec);
            domainRuleManager.RegisterRule(new EmploymentMinimumDataRequiredRule<SalaryDeductionEmploymentModel>(employmentDataManager));
            domainRuleManager.RegisterRule(new BasicIncomeIsRequiredRule<SalaryDeductionEmploymentModel>());
            domainRuleManager.RegisterRule(new EmploymentStartDateMustBeBeforeTodayRule<SalaryDeductionEmploymentModel>());
            domainRuleManager.RegisterPartialRule<IEmployeeDeductions>(new EmployeeDeductionsCanOnlyContainOneOfEachTypeRule());
        }

        public ISystemMessageCollection HandleCommand(AddUnconfirmedSalaryDeductionEmploymentToClientCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();
            this.DomainRuleManager.ExecuteRules(messages, command.SalaryDeductionEmploymentModel);
            if (messages.ErrorMessages().Any())
            {
                return messages;
            }
            using (var uow = uowFactory.Build())
            {
                int employmentKey = this.EmploymentDataManager.SaveSalaryDeductionEmployment(command.ClientKey, command.SalaryDeductionEmploymentModel);
                @event = new UnconfirmedSalaryDeductionEmploymentAddedToClientEvent(DateTime.Now, command.ClientKey, command.SalaryDeductionEmploymentModel.HousingAllowance, 
                    command.SalaryDeductionEmploymentModel.RemunerationType, command.SalaryDeductionEmploymentModel.BasicIncome, command.SalaryDeductionEmploymentModel.StartDate,
                    command.SalaryDeductionEmploymentModel.EmploymentStatus, command.SalaryDeductionEmploymentModel.SalaryPaymentDay, 
                    command.SalaryDeductionEmploymentModel.Employer.EmployerName, command.SalaryDeductionEmploymentModel.Employer.TelephoneCode, 
                    command.SalaryDeductionEmploymentModel.Employer.TelephoneNumber, command.SalaryDeductionEmploymentModel.Employer.ContactPerson, 
                    command.SalaryDeductionEmploymentModel.Employer.ContactEmail, command.SalaryDeductionEmploymentModel.Employer.EmployerBusinessType, 
                    command.SalaryDeductionEmploymentModel.Employer.EmploymentSector, employmentKey);
                eventRaiser.RaiseEvent(DateTime.Now, @event, employmentKey, (int)GenericKeyType.Employment, metadata);

                uow.Complete();
            }
            return messages;
        }
    }
}