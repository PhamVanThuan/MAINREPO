using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.BusinessModel.Validation;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Data.Models._2AM.Managers;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.ApplicationDomain.Rules;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using SAHL.Services.Interfaces.DomainQuery;
using System;

namespace SAHL.Services.ApplicationDomain.CommandHandlers
{
    public class AddLeadApplicantToApplicationCommandHandler : IDomainServiceCommandHandler<AddLeadApplicantToApplicationCommand, LeadApplicantAddedToApplicationEvent>
    {
        private IApplicantDataManager applicantDataManager;
        private IDomainRuleManager<AddLeadApplicantToApplicationCommand> domainRuleManager;
        private LeadApplicantAddedToApplicationEvent LeadApplicantAddedToApplicationEvent;
        private IEventRaiser eventRaiser;
        private IUnitOfWorkFactory unitOfWorkFactory;
        private ILinkedKeyManager linkedKeyManager;
        private IValidationUtils validationUtils;
        private IDomainQueryServiceClient domainQueryClient;

        public AddLeadApplicantToApplicationCommandHandler(IApplicantDataManager applicantDataManager, IUnitOfWorkFactory unitOfWorkFactory,
            IDomainRuleManager<AddLeadApplicantToApplicationCommand> domainRuleContext, IEventRaiser eventRaiser, ILinkedKeyManager linkedKeyManager, IValidationUtils validationUtils,
            IDomainQueryServiceClient domainQueryClient)
        {
            this.applicantDataManager = applicantDataManager;
            this.domainRuleManager = domainRuleContext;
            this.eventRaiser = eventRaiser;
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.linkedKeyManager = linkedKeyManager;
            this.validationUtils = validationUtils;
            this.domainQueryClient = domainQueryClient;

            this.domainRuleManager.RegisterRule(new ApplicantsMustBeBetween18And65YearsOldRule(applicantDataManager, validationUtils));
            this.domainRuleManager.RegisterRule(new ClientCannotBeAnExistingApplicantRule(applicantDataManager));
            this.domainRuleManager.RegisterRule(new ApplicantMustHaveFirstNamesAndSurnameRule(domainQueryClient));
            this.domainRuleManager.RegisterRule(new ApplicantMustHaveAtLeastOneContactDetailRule(domainQueryClient));
        }

        public ISystemMessageCollection HandleCommand(AddLeadApplicantToApplicationCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();

            this.domainRuleManager.ExecuteRules(messages, command);

            if (messages.HasErrors)
            {
                return messages;
            }

            using (var uow = unitOfWorkFactory.Build())
            {
                int offerRoleKey = applicantDataManager.AddApplicantRole(new OfferRoleDataModel(command.ClientKey, command.ApplicationNumber, (int)command.ApplicationRoleType,
                    (int)GeneralStatus.Active, DateTime.Now));

                //link the id to the key
                this.linkedKeyManager.LinkKeyToGuid(offerRoleKey, command.ApplicationRoleId);

                // populate the event
                var date = DateTime.Now;
                LeadApplicantAddedToApplicationEvent = new LeadApplicantAddedToApplicationEvent(date, command.ApplicationNumber, command.ClientKey, offerRoleKey);
                eventRaiser.RaiseEvent(date, LeadApplicantAddedToApplicationEvent, command.ClientKey, (int)GenericKeyType.LegalEntity, metadata);

                uow.Complete();
            }

            return messages;
        }
    }
}