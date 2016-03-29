using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.ApplicationDomain.Rules;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Services.Interfaces.DomainQuery;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.CommandHandlers
{
    public class AddApplicantDeclarationsCommandHandler : IDomainServiceCommandHandler<AddApplicantDeclarationsCommand, DeclarationsAddedToApplicantEvent>
    {
        private IServiceCommandRouter serviceCommandRouter;
        private IApplicationDataManager applicationDataManager;
        private IApplicantDataManager applicantDataManager;
        private IDomainQueryServiceClient domainQueryService;
        private IUnitOfWorkFactory uowFactory;
        private IDomainRuleManager<ApplicantDeclarationsModel> domainRuleManager;
        private IEventRaiser eventRaiser;

        public AddApplicantDeclarationsCommandHandler(IServiceCommandRouter serviceCommandRouter, IApplicationDataManager applicationDataManager,
            IDomainQueryServiceClient domainQueryService, IDomainRuleManager<ApplicantDeclarationsModel> domainRuleManager, IEventRaiser eventRaiser, 
            IApplicantDataManager applicantDataManager, IUnitOfWorkFactory uowFactory)
        {
            this.serviceCommandRouter = serviceCommandRouter;
            this.applicationDataManager = applicationDataManager;
            this.applicantDataManager = applicantDataManager;
            this.domainQueryService = domainQueryService;
            this.domainRuleManager = domainRuleManager;
            this.uowFactory = uowFactory;
            this.eventRaiser = eventRaiser;

            this.domainRuleManager.RegisterRule(new DebtArrangementAnsweredIsRequiredWhenUnderDebtReviewRule());
            this.domainRuleManager.RegisterRule(new RehabilitationDateIsRequiredWhenClientHasBeenDeclaredInsolventRule());
            this.domainRuleManager.RegisterRule(new RescindedDateRequiredWhenClientHasBeenUnderAdminOrderRule());
            this.domainRuleManager.RegisterRule(new ClientShouldBeANaturalPersonRule(domainQueryService));
            this.domainRuleManager.RegisterRule(new ClientIsAnApplicantOnApplicationRule(applicantDataManager));
            this.domainRuleManager.RegisterRule(new ClientCannotHaveExistingDeclarationsRule(applicantDataManager));
        }

        public ISystemMessageCollection HandleCommand(AddApplicantDeclarationsCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();
            domainRuleManager.ExecuteRules(messages, command.ApplicantDeclarations);
            if (messages.HasErrors)
            {
                return messages;
            }

            using (var uow = uowFactory.Build())
            {
                var clientRolesOnApplication = applicantDataManager.GetActiveClientRoleOnApplication(command.ApplicantDeclarations.ApplicationNumber, command.ApplicantDeclarations.ClientKey);
                if (!clientRolesOnApplication.Any())
                {
                    messages.AddMessage(new SystemMessage("The client role for this application does not exist.", SystemMessageSeverityEnum.Error));
                }

                if (!messages.HasErrors)
                {
                    int offerRoleKey = clientRolesOnApplication.First().OfferRoleKey;

                    IEnumerable<OfferDeclarationDataModel> offerDeclarations = new List<OfferDeclarationDataModel> {
                    (new OfferDeclarationDataModel(offerRoleKey, (int)OfferDeclarationQuestionEnum.DeclaredInsolventQuestionKey, 
                        (int)command.ApplicantDeclarations.DeclaredInsolventDeclaration.Answer, null)),
                    (new OfferDeclarationDataModel(offerRoleKey, (int)OfferDeclarationQuestionEnum.InsolventRehabilitationDateQuestionKey,
                        (int)OfferDeclarationAnswer.Date, command.ApplicantDeclarations.DeclaredInsolventDeclaration.DateRehabilitated)),

                    (new OfferDeclarationDataModel(offerRoleKey, (int)OfferDeclarationQuestionEnum.UnderAdministrationOrderQuestionKey, 
                        (int)command.ApplicantDeclarations.UnderAdministrationOrderDeclaration.Answer, null)),
                    (new OfferDeclarationDataModel(offerRoleKey, (int)OfferDeclarationQuestionEnum.AdministrationOrderDateRescindedQuestionKey,
                        (int)OfferDeclarationAnswer.Date, command.ApplicantDeclarations.UnderAdministrationOrderDeclaration.DateAdministrationOrderRescinded)),

                    (new OfferDeclarationDataModel(offerRoleKey, (int)OfferDeclarationQuestionEnum.CurrentlyUnderDebtCounsellingQuestionKey, 
                        (int)command.ApplicantDeclarations.CurrentlyUnderDebtReviewDeclaration.Answer, null)),
                    (new OfferDeclarationDataModel(offerRoleKey, (int)OfferDeclarationQuestionEnum.HasCurrentDebtArrangementQuestionKey,
                        (command.ApplicantDeclarations.CurrentlyUnderDebtReviewDeclaration.HasCurrentDebtArrangement.GetValueOrDefault(false) 
                        ? (int)OfferDeclarationAnswer.Yes : (int)OfferDeclarationAnswer.No), null)),

                    (new OfferDeclarationDataModel(offerRoleKey, (int)OfferDeclarationQuestionEnum.PermissionToConductCreditCheckQuestionKey,
                        (int)command.ApplicantDeclarations.PermissiontoConductCreditCheckDeclaration.Answer, null))
                };

                    applicantDataManager.SaveApplicantDeclarations(offerDeclarations);

                    eventRaiser.RaiseEvent(DateTime.Now, new DeclarationsAddedToApplicantEvent(DateTime.Now, command.ClientKey, command.ApplicationNumber,
                        DateTime.Now,
                        command.ApplicantDeclarations.DeclaredInsolventDeclaration.Answer, command.ApplicantDeclarations.DeclaredInsolventDeclaration.DateRehabilitated,
                        command.ApplicantDeclarations.UnderAdministrationOrderDeclaration.Answer, command.ApplicantDeclarations.UnderAdministrationOrderDeclaration.DateAdministrationOrderRescinded,
                        command.ApplicantDeclarations.CurrentlyUnderDebtReviewDeclaration.Answer, command.ApplicantDeclarations.CurrentlyUnderDebtReviewDeclaration.HasCurrentDebtArrangement,
                        command.ApplicantDeclarations.PermissiontoConductCreditCheckDeclaration.Answer)
                        , command.ApplicantDeclarations.ApplicationNumber, (int)GenericKeyType.Offer, metadata);
                }

                uow.Complete();
            }

            return messages;
        }
    }
}