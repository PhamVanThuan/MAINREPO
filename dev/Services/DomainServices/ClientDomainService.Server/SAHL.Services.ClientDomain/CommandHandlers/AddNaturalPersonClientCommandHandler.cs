using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.BusinessModel.Validation;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Data.Models._2AM.Managers;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.ClientDomain.Managers;
using SAHL.Services.ClientDomain.Rules;
using SAHL.Services.ClientDomain.Utils;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Events;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;

namespace SAHL.Services.ClientDomain.CommandHandlers
{
    public class AddNaturalPersonClientCommandHandler : IDomainServiceCommandHandler<AddNaturalPersonClientCommand, NaturalPersonClientAddedEvent>
    {
        private IClientDataManager ClientDataManager;
        private ILinkedKeyManager LinkedKeyManager;
        private IEventRaiser EventRaiser;
        private IDomainRuleManager<INaturalPersonClientModel> domainRuleManager;
        private IUnitOfWorkFactory unitOfWorkFactory;
        private IValidationUtils validationUtils;

        public AddNaturalPersonClientCommandHandler(IClientDataManager clientDataManager, ILinkedKeyManager linkedKeyManager, IEventRaiser eventRaiser,
            IDomainRuleManager<INaturalPersonClientModel> domainRuleManager, IUnitOfWorkFactory unitOfWorkFactory, IValidationUtils validationUtils)
        {
            this.ClientDataManager = clientDataManager;
            this.LinkedKeyManager = linkedKeyManager;
            this.EventRaiser = eventRaiser;
            this.domainRuleManager = domainRuleManager;
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.validationUtils = validationUtils;

            this.domainRuleManager.RegisterRule(new IdNumberMustBeValidWhenProvidedForASACitizenRule(validationUtils));
            this.domainRuleManager.RegisterRule(new ClientDateOfBirthMustBePriorToTodayRule());
            this.domainRuleManager.RegisterRule(new PassportNumberMustBeValidWhenProvidedRule());
            this.domainRuleManager.RegisterRule(new PassportNumberCannotBeAValidIdentityNumberRule(validationUtils));
            this.domainRuleManager.RegisterRule(new IdNumberMustBeUniqueRule(clientDataManager, validationUtils));
            this.domainRuleManager.RegisterRule(new PassportNumberMustBeUniqueRule(validationUtils, clientDataManager));
            this.domainRuleManager.RegisterPartialRule<IClientContactDetails>(new AtLeastOneClientContactDetailShouldBeProvidedRule());
        }

        public ISystemMessageCollection HandleCommand(AddNaturalPersonClientCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();
            domainRuleManager.ExecuteRules(messages, command.NaturalPersonClient);

            if (messages.HasErrors)
            {
                return messages;
            }

            DateTime now = DateTime.Now;

            var client = command.NaturalPersonClient;
            LegalEntityDataModel legalEntityDataModel = new LegalEntityDataModel((int)LegalEntityType.NaturalPerson,
                           (int?)client.MaritalStatus,
                           (int?)client.Gender,
                           (int?)client.PopulationGroup,
                           now,
                           (int?)client.Salutation,
                           client.FirstName,
                           client.Initials,
                           client.Surname,
                           client.PreferredName,
                           client.IDNumber,
                           client.PassportNumber,
                           null, null, null, null,
                           client.DateOfBirth,
                           client.HomePhoneCode,
                           client.HomePhone,
                           client.WorkPhoneCode,
                           client.WorkPhone,
                           client.Cellphone,
                           client.EmailAddress,
                           client.FaxCode,
                           client.FaxNumber,
                           null,
                           (int?)client.CitizenshipType,
                           (int)LegalEntityStatus.Alive,
                           null, null, null, null,
                           client.Education.HasValue ? (int)client.Education.Value : (int)Education.Unknown,
                           (int?)client.HomeLanguage,
                           client.CorrespondenceLanguage.HasValue ? (int)client.CorrespondenceLanguage : (int)CorrespondenceLanguage.English,
                           null);

            LegalEntitySanitiser.Sanitise(legalEntityDataModel);

            using (var uow = unitOfWorkFactory.Build())
            {
                int legalEntityKey = ClientDataManager.AddNewLegalEntity(legalEntityDataModel);
                legalEntityDataModel.LegalEntityKey = legalEntityKey;
                LinkedKeyManager.LinkKeyToGuid(legalEntityKey, command.Id);

                NaturalPersonClientAddedEvent @event = new NaturalPersonClientAddedEvent(now, legalEntityKey, client.MaritalStatus, client.Gender,
                    client.PopulationGroup, legalEntityDataModel.IntroductionDate, client.Salutation, legalEntityDataModel.FirstNames,
                    legalEntityDataModel.Surname, legalEntityDataModel.PreferredName, legalEntityDataModel.IDNumber, legalEntityDataModel.PassportNumber, client.DateOfBirth,
                    legalEntityDataModel.HomePhoneCode, legalEntityDataModel.HomePhoneNumber, legalEntityDataModel.WorkPhoneCode, legalEntityDataModel.WorkPhoneNumber,
                    legalEntityDataModel.CellPhoneNumber, legalEntityDataModel.EmailAddress, legalEntityDataModel.FaxCode, legalEntityDataModel.FaxNumber, client.CitizenshipType,
                    (Education)legalEntityDataModel.EducationKey);
                EventRaiser.RaiseEvent(@event.Date, @event, legalEntityKey, (int)GenericKeyType.LegalEntity, metadata);
                uow.Complete();
            }
            return messages;
        }
    }
}