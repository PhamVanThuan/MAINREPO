using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.BusinessModel.Validation;
using SAHL.Core.Data.Models._2AM;
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

namespace SAHL.Services.ClientDomain.CommandHandlers
{
    public class UpdateActiveNaturalPersonClientCommandHandler : IDomainServiceCommandHandler<UpdateActiveNaturalPersonClientCommand, ActiveNaturalPersonClientUpdatedEvent>
    {
        private IClientDataManager clientDataManager;
        private IEventRaiser eventRaiser;
        private IDomainRuleManager<ActiveNaturalPersonClientModel> domainRuleManager;
        private IValidationUtils validationUtils;

        public UpdateActiveNaturalPersonClientCommandHandler(IClientDataManager clientDataManager, IEventRaiser eventRaiser, IDomainRuleManager<ActiveNaturalPersonClientModel> domainRuleManager)
        {
            this.clientDataManager = clientDataManager;
            this.eventRaiser = eventRaiser;
            this.domainRuleManager = domainRuleManager;
            this.domainRuleManager.RegisterPartialRule<IClientContactDetails>(new AtLeastOneClientContactDetailShouldBeProvidedRule());
        }

        public ISystemMessageCollection HandleCommand(UpdateActiveNaturalPersonClientCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();
            this.domainRuleManager.ExecuteRules(messages, command.ActiveNaturalPersonClient);

            if (messages.HasErrors)
            {
                return messages;
            }

            DateTime now = DateTime.Now;

            LegalEntityDataModel existingClient = clientDataManager.FindExistingClient(command.ClientKey);

            var newClientInfo = command.ActiveNaturalPersonClient;

            existingClient.PreferredName = newClientInfo.PreferredName;
            existingClient.Salutationkey = (int)newClientInfo.Salutation;
            existingClient.EducationKey = newClientInfo.Education.HasValue ? (int)newClientInfo.Education.Value : (int)Education.Unknown;
            existingClient.HomeLanguageKey = (int)newClientInfo.HomeLanguage;
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
            //we only update an active client's DOB if the current one has no value
            existingClient.DateOfBirth = !existingClient.DateOfBirth.HasValue ? newClientInfo.DateOfBirth : existingClient.DateOfBirth;

            clientDataManager.UpdateLegalEntity(existingClient);

            ActiveNaturalPersonClientUpdatedEvent @event = new ActiveNaturalPersonClientUpdatedEvent(now, existingClient.PreferredName, newClientInfo.Salutation,
                newClientInfo.Education, newClientInfo.HomeLanguage, newClientInfo.CorrespondenceLanguage, existingClient.HomePhoneCode, existingClient.HomePhoneNumber,
                existingClient.WorkPhoneCode, existingClient.WorkPhoneNumber, existingClient.FaxCode, existingClient.FaxNumber, existingClient.CellPhoneNumber, existingClient.EmailAddress);
            eventRaiser.RaiseEvent(@event.Date, @event, command.ClientKey, (int)GenericKeyType.LegalEntity, metadata);

            return messages;
        }
    }
}