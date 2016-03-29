using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Data.Models._2AM.Managers.ADUser;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.ApplicationDomain.Managers.Domicilium;
using SAHL.Services.ApplicationDomain.Rules;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;

namespace SAHL.Services.ApplicationDomain.CommandHandlers
{
    public class LinkDomiciliumAddressToApplicantCommandHandler : IDomainServiceCommandHandler<LinkDomiciliumAddressToApplicantCommand, DomiciliumAddressLinkedToApplicantEvent>
    {
        private IApplicantDataManager applicantDataManager;
        private IApplicationDataManager applicationDataManager;
        private IDomiciliumDataManager domiciliumDataManager;
        private DomiciliumAddressLinkedToApplicantEvent @event;
        private IUnitOfWorkFactory uowFactory;
        private IEventRaiser eventRaiser;
        private IDomainRuleManager<ApplicantDomiciliumAddressModel> domainRuleContext;
        private IADUserManager adUserManager;

        public LinkDomiciliumAddressToApplicantCommandHandler(IApplicantDataManager applicantDataManager, IApplicationDataManager applicationDataManager, IDomiciliumDataManager domiciliumDataManager,
            IUnitOfWorkFactory uowFactory, IEventRaiser eventRaiser, IDomainRuleManager<ApplicantDomiciliumAddressModel> domainRuleContext, IADUserManager adUserManager)
        {
            this.applicantDataManager = applicantDataManager;
            this.applicationDataManager = applicationDataManager;
            this.domiciliumDataManager = domiciliumDataManager;
            this.eventRaiser = eventRaiser;
            this.uowFactory = uowFactory;
            this.domainRuleContext = domainRuleContext;
            this.adUserManager = adUserManager;

            this.domainRuleContext.RegisterRule(new ApplicantCannotHaveAnExistingPendingDomiciliumAddressRule(applicantDataManager));
            this.domainRuleContext.RegisterRule(new ApplicantCanOnlyBeLinkedToAPendingDomiciliumAddressRule(domiciliumDataManager));
        }

        public ISystemMessageCollection HandleCommand(LinkDomiciliumAddressToApplicantCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection systemMessages = SystemMessageCollection.Empty();

            this.domainRuleContext.ExecuteRules(systemMessages, command.ApplicantDomicilium);
            if (systemMessages.HasErrors)
            {
                return systemMessages;
            }

            OfferRoleDataModel applicationRoleModel = applicantDataManager.GetActiveApplicationRole(command.ApplicantDomicilium.ApplicationNumber, command.ApplicantDomicilium.ClientKey);
            if (applicationRoleModel == null)
            {
                systemMessages.AddMessage(new SystemMessage("Failed to retrieve application role key.", SystemMessageSeverityEnum.Error));
                return systemMessages;
            }
            int applicationRoleTypeKey = applicationRoleModel.OfferRoleTypeKey;
            int applicationRoleKey = applicationRoleModel.OfferRoleKey;

            int? ADUserKey = adUserManager.GetAdUserKeyByUserName(metadata.UserName);
            if (ADUserKey == null || ADUserKey <= 0)
            {
                systemMessages.AddMessage(new SystemMessage("Failed to retrieve ADUserKey.", SystemMessageSeverityEnum.Error));
                return systemMessages;
            }

            OfferRoleDomiciliumDataModel applicantDomiciliumAddressDataModel = new OfferRoleDomiciliumDataModel(command.ApplicantDomicilium.ClientDomiciliumKey, applicationRoleKey,
                DateTime.Now, ADUserKey.Value);
            using (var uow = uowFactory.Build())
            {
                int applicationRoleDomiciliumKey = applicantDataManager.LinkDomiciliumAddressToApplicant(applicantDomiciliumAddressDataModel);

                if (applicationRoleDomiciliumKey > 0)
                {
                    @event = new DomiciliumAddressLinkedToApplicantEvent(DateTime.Now, applicationRoleDomiciliumKey, command.ApplicantDomicilium.ClientKey,
                        command.ApplicantDomicilium.ApplicationNumber, command.ApplicantDomicilium.ClientDomiciliumKey);
                    eventRaiser.RaiseEvent(DateTime.Now, @event, applicationRoleDomiciliumKey, applicationRoleTypeKey, metadata);
                }
                uow.Complete();
            }

            return systemMessages;
        }
    }
}