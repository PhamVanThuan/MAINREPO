using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.ApplicationDomain.CommandHandlers;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Services.Interfaces.DomainQuery;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Specs.CommandHandlersSpec.AddApplicantDeclarations
{
    public class when_executing_rules_returns_errors : WithCoreFakes
    {
        private static AddApplicantDeclarationsCommand command;
        private static AddApplicantDeclarationsCommandHandler handler;
        private static IApplicationDataManager applicationDataManager;
        private static IApplicantDataManager applicantDataManager;
        private static IDomainRuleManager<ApplicantDeclarationsModel> domainRuleContext;
        private static ApplicantDeclarationsModel applicantDeclarations;
        private static IDomainQueryServiceClient domainQueryService;

        private Establish context = () =>
        {
            applicationDataManager = An<IApplicationDataManager>();
            domainRuleContext = An<IDomainRuleManager<ApplicantDeclarationsModel>>();
            domainQueryService = An<IDomainQueryServiceClient>();
            applicantDataManager = An<IApplicantDataManager>();

            handler = new AddApplicantDeclarationsCommandHandler(serviceCommandRouter, applicationDataManager, domainQueryService, domainRuleContext
                , eventRaiser, applicantDataManager, unitOfWorkFactory);

            applicantDeclarations = new ApplicantDeclarationsModel(1234, 67, DateTime.MinValue,
                new DeclaredInsolventDeclarationModel(OfferDeclarationAnswer.No, null),
                new UnderAdministrationOrderDeclarationModel(OfferDeclarationAnswer.No, DateTime.MinValue),
                new CurrentlyUnderDebtCounsellingReviewDeclarationModel(OfferDeclarationAnswer.No, null),
                new PermissionToConductCreditCheckDeclarationModel(OfferDeclarationAnswer.Yes));
            command = new AddApplicantDeclarationsCommand(applicantDeclarations);

            //rules must fail
            domainRuleContext.WhenToldTo(x => x.ExecuteRules(Param.IsAny<ISystemMessageCollection>(), applicantDeclarations)).Callback<ISystemMessageCollection>
                (y => y.AddMessage(new SystemMessage("rule error messages", SystemMessageSeverityEnum.Error)));
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_not_get_the_applicants_role_for_the_application = () =>
        {
            applicantDataManager.WasNotToldTo(x => x.GetActiveClientRoleOnApplication(Param.IsAny<int>(), Param.IsAny<int>()));
        };

        private It should_not_save_the_declarations = () =>
        {
            applicantDataManager.WasNotToldTo(x => x.SaveApplicantDeclarations(Param.IsAny<IEnumerable<OfferDeclarationDataModel>>()));
        };

        private It should_return_error_messages = () =>
        {
            messages.ErrorMessages().Count().ShouldNotEqual(0);
        };

        private It should_not_raise_an_event = () =>
        {
            eventRaiser.WasNotToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(), Param.IsAny<Event>(),
                    Param.IsAny<int>(), Param.IsAny<int>(), Param.IsAny<IServiceRequestMetadata>()));
        };
    }
}
