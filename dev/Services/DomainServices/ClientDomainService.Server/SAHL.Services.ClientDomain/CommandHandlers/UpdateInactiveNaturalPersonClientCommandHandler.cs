using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.BusinessModel.Validation;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.ClientDomain.Managers;
using SAHL.Services.ClientDomain.Rules;
using SAHL.Services.ClientDomain.Rules.Models;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Events;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;

namespace SAHL.Services.ClientDomain.CommandHandlers
{
    public class UpdateInactiveNaturalPersonClientCommandHandler : IDomainServiceCommandHandler<UpdateInactiveNaturalPersonClientCommand, InactiveNaturalPersonClientUpdatedEvent>
    {
        private IClientDataManager clientDataManager;
        private IEventRaiser eventRaiser;
        private IDomainRuleManager<NaturalPersonClientRuleModel> domainRuleManager;

        public UpdateInactiveNaturalPersonClientCommandHandler(IClientDataManager clientDataManager, IEventRaiser eventRaiser, IDomainRuleManager<NaturalPersonClientRuleModel> domainRuleManager,
            IValidationUtils validationUtils)
        {
            this.clientDataManager = clientDataManager;
            this.eventRaiser = eventRaiser;
            this.domainRuleManager = domainRuleManager;

            this.domainRuleManager.RegisterPartialRule<INaturalPersonClientModel>(new IdNumberMustBeValidWhenProvidedForASACitizenRule(validationUtils));
            this.domainRuleManager.RegisterPartialRule<INaturalPersonClientModel>(new ClientDateOfBirthMustBePriorToTodayRule());
            this.domainRuleManager.RegisterPartialRule<INaturalPersonClientModel>(new PassportNumberMustBeValidWhenProvidedRule());
            this.domainRuleManager.RegisterPartialRule<INaturalPersonClientModel>(new PassportNumberCannotBeAValidIdentityNumberRule(validationUtils));
            this.domainRuleManager.RegisterRule(new ClientCannotBeLinkedToAnOpenAccountRule(clientDataManager));
            this.domainRuleManager.RegisterRule(new ClientCannotBeLinkedToAnOpenApplicationRule(clientDataManager));
            this.domainRuleManager.RegisterPartialRule<IClientContactDetails>(new AtLeastOneClientContactDetailShouldBeProvidedRule());
        }

        public ISystemMessageCollection HandleCommand(UpdateInactiveNaturalPersonClientCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();
            this.domainRuleManager.ExecuteRules(messages, new NaturalPersonClientRuleModel(command.ClientKey, command.NaturalPersonClient));

            if (messages.HasErrors)
            {
                return messages;
            }

            DateTime now = DateTime.Now;

            LegalEntityDataModel existingClient = clientDataManager.FindExistingClient(command.ClientKey);

            var newClientInfo = command.NaturalPersonClient;

            existingClient.IDNumber = newClientInfo.IDNumber;
            existingClient.FirstNames = newClientInfo.FirstName;
            existingClient.Surname = newClientInfo.Surname;
            existingClient.GenderKey = (int?)newClientInfo.Gender;
            existingClient.MaritalStatusKey = (int?)newClientInfo.MaritalStatus;
            existingClient.PopulationGroupKey = (int?)newClientInfo.PopulationGroup;
            existingClient.CitizenTypeKey = (int?)newClientInfo.CitizenshipType;
            existingClient.DateOfBirth = newClientInfo.DateOfBirth;
            existingClient.PassportNumber = newClientInfo.PassportNumber;
            existingClient.PreferredName = newClientInfo.PreferredName;
            existingClient.Salutationkey = (int?)newClientInfo.Salutation;
            existingClient.EducationKey = newClientInfo.Education.HasValue ? (int)newClientInfo.Education.Value : (int)Education.Unknown;
            existingClient.HomeLanguageKey = (int?)newClientInfo.HomeLanguage;
            existingClient.DocumentLanguageKey = (int)newClientInfo.CorrespondenceLanguage;
            existingClient.HomePhoneCode = newClientInfo.HomePhoneCode;
            existingClient.HomePhoneNumber = newClientInfo.HomePhone;
            existingClient.WorkPhoneCode = newClientInfo.WorkPhoneCode;
            existingClient.WorkPhoneNumber = newClientInfo.WorkPhone;
            existingClient.FaxCode = newClientInfo.FaxCode;
            existingClient.FaxNumber = newClientInfo.FaxNumber;
            existingClient.CellPhoneNumber = newClientInfo.Cellphone;
            existingClient.EmailAddress = newClientInfo.EmailAddress;
            existingClient.ChangeDate = now;

            clientDataManager.UpdateLegalEntity(existingClient);

            var @event = new InactiveNaturalPersonClientUpdatedEvent(now, newClientInfo.MaritalStatus, newClientInfo.Gender, newClientInfo.PopulationGroup,
                existingClient.IntroductionDate, newClientInfo.Salutation, existingClient.FirstNames, existingClient.Surname, existingClient.PreferredName, existingClient.IDNumber, 
                existingClient.PassportNumber, existingClient.DateOfBirth, existingClient.HomePhoneCode, existingClient.HomePhoneNumber, existingClient.WorkPhoneCode, 
                existingClient.WorkPhoneNumber, existingClient.CellPhoneNumber, existingClient.EmailAddress, existingClient.FaxCode, existingClient.FaxNumber, 
                (CitizenType)existingClient.CitizenTypeKey, (Education)existingClient.EducationKey);
            eventRaiser.RaiseEvent(@event.Date, @event, command.ClientKey, (int)GenericKeyType.LegalEntity, metadata);

            return messages;
        }
    }
}