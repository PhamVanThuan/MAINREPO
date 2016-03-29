using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.ApplicationDomain.Rules;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.CommandHandlers
{
    public class MakeApplicantAnIncomeContributorCommandHandler : IDomainServiceCommandHandler<MakeApplicantAnIncomeContributorCommand, ApplicantMadeIncomeContributorEvent>
    {
        private IApplicantManager applicantManager;
        private ApplicantMadeIncomeContributorEvent applicantMadeIncomeContributorEvent;
        private IEventRaiser eventRaiser;
        private IUnitOfWorkFactory unitOfWorkFactory;
        private IDomainRuleManager<ApplicantRoleModel> domainRuleManager;

        public MakeApplicantAnIncomeContributorCommandHandler(IApplicantManager applicantManager, IUnitOfWorkFactory unitOfWorkFactory, IEventRaiser eventRaiser, 
            IDomainRuleManager<ApplicantRoleModel> domainRuleManager)
        {
            this.applicantManager = applicantManager;
            this.eventRaiser = eventRaiser;
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.domainRuleManager = domainRuleManager;
            
            domainRuleManager.RegisterRule(new ApplicantCannotBeAnExistingIncomeContributorRule(applicantManager));
        }

        public ISystemMessageCollection HandleCommand(MakeApplicantAnIncomeContributorCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();
            var applicantRoleModel = new ApplicantRoleModel(command.ApplicationRoleKey);
            domainRuleManager.ExecuteRules(messages, applicantRoleModel);

            if (messages.HasErrors)
            {
                return messages;
            }
            using (var uow = unitOfWorkFactory.Build())
            {
                applicantManager.AddIncomeContributorOfferRoleAttribute(command.ApplicationRoleKey);

                uow.Complete();
            }

            var date = DateTime.Now;
            applicantMadeIncomeContributorEvent = new ApplicantMadeIncomeContributorEvent(date, command.ApplicationRoleKey);
            eventRaiser.RaiseEvent(date, applicantMadeIncomeContributorEvent, command.ApplicationRoleKey, (int)GenericKeyType.OfferRole, metadata);

            return messages;
        }
    }
}