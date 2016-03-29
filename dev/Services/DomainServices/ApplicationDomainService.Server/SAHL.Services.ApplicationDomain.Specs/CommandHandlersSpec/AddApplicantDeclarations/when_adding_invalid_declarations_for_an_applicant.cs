using System;
using System.Collections.Generic;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
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
using SAHL.Services.Interfaces.DomainQuery.Model;
using SAHL.Services.Interfaces.DomainQuery.Queries;

namespace SAHL.Services.ApplicationDomain.Specs.CommandHandlersSpec.AddApplicantDeclarations
{
    public class when_adding_invalid_declarations_for_an_applicant : WithCoreFakes
    {
        private static AddApplicantDeclarationsCommand command;
        private static AddApplicantDeclarationsCommandHandler handler;
        private static ISystemMessageCollection expectedMessages;
        private static IApplicationDataManager applicationDataManager;
        private static IDomainRuleManager<ApplicantDeclarationsModel> domainRuleManager;
        private static ApplicantDeclarationsModel applicantDeclarations;
        private static OfferRoleDataModel applicantRole;
        private static IDomainQueryServiceClient domainQueryService;
        private static IApplicantDataManager applicantDataManager;

        private Establish context = () =>
        {
            expectedMessages = SystemMessageCollection.Empty();

            applicationDataManager = An<IApplicationDataManager>();
            domainRuleManager = An<IDomainRuleManager<ApplicantDeclarationsModel>>();
            domainQueryService = An<IDomainQueryServiceClient>();
            applicantDataManager = An<IApplicantDataManager>();

            handler = new AddApplicantDeclarationsCommandHandler(
                serviceCommandRouter,
                applicationDataManager,
                domainQueryService,
                domainRuleManager,
                eventRaiser,
                applicantDataManager,
                unitOfWorkFactory);

            applicantDeclarations = new ApplicantDeclarationsModel(1234,
                67,
                DateTime.MinValue,
                new DeclaredInsolventDeclarationModel(OfferDeclarationAnswer.No, null),
                new UnderAdministrationOrderDeclarationModel(OfferDeclarationAnswer.No, DateTime.MinValue),
                new CurrentlyUnderDebtCounsellingReviewDeclarationModel(OfferDeclarationAnswer.No, null),
                new PermissionToConductCreditCheckDeclarationModel(OfferDeclarationAnswer.Yes));

            command = new AddApplicantDeclarationsCommand(applicantDeclarations);

            expectedMessages.AddMessage(new SystemMessage("Test Message", SystemMessageSeverityEnum.Error));
            domainRuleManager.WhenToldTo(x => x.ExecuteRules(Param.IsAny<ISystemMessageCollection>(), Param.IsAny<ApplicantDeclarationsModel>()))
                .Callback<ISystemMessageCollection, ApplicantDeclarationsModel>((y, z) =>
                {
                    y.AddMessages(expectedMessages.AllMessages);
                });

            applicationDataManager.WhenToldTo(x => x.DoesApplicationExist(Param.IsAny<int>())).Return(true);

            applicantDataManager.WhenToldTo(x => x.CheckClientIsAnApplicantOnTheApplication(Param.IsAny<int>(), Param.IsAny<int>())).Return(true);

            IsClientANaturalPersonQuery query = new IsClientANaturalPersonQuery(command.ApplicantDeclarations.ClientKey);
            query.Result = new ServiceQueryResult<IsClientANaturalPersonQueryResult>(
                new[]
                {
                    new IsClientANaturalPersonQueryResult { ClientIsANaturalPerson = true }
                });
            domainQueryService.WhenToldTo(c => c.PerformQuery(Param.IsAny<IsClientANaturalPersonQuery>()))
                .Return<IsClientANaturalPersonQuery>(y =>
                {
                    y.Result = query.Result;
                    return SystemMessageCollection.Empty();
                });

            applicantRole = new OfferRoleDataModel(1, 2, 3, 1, DateTime.Now);
            IEnumerable<OfferRoleDataModel> applicantRoles = new[] { applicantRole };
            applicantDataManager.WhenToldTo(x => x.GetActiveClientRoleOnApplication(Param.IsAny<int>(), Param.IsAny<int>())).Return(applicantRoles);

            unitOfWorkFactory.WhenToldTo(x => x.Build()).Return(unitOfWork);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_register_applicant_declarations_validation_rules = () =>
        {
            domainRuleManager.WasToldTo(x => x.RegisterRule(Param.IsAny<IDomainRule<ApplicantDeclarationsModel>>()));
        };

        private It should_run_applicant_declarations_validation_rules = () =>
        {
            domainRuleManager.WasToldTo(x => x.ExecuteRules(Param.IsAny<ISystemMessageCollection>(),
                Arg.Is<ApplicantDeclarationsModel>
                    (y => y.ApplicationNumber == applicantDeclarations.ApplicationNumber)));
        };

        private It should_not_save_the_declarations = () =>
        {
            applicantDataManager.WasNotToldTo(x => x.SaveApplicantDeclarations(Param.IsAny<IEnumerable<OfferDeclarationDataModel>>()));
        };

        private It should_return_errors = () =>
        {
            messages.ErrorMessages().First().Message.ShouldEqual(expectedMessages.ErrorMessages().First().Message);
        };

        private It should_not_raise_an_event = () =>
        {
            eventRaiser.WasNotToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(),
                Param.IsAny<Event>(),
                Param.IsAny<int>(),
                Param.IsAny<int>(),
                Param.IsAny<IServiceRequestMetadata>()));
        };
    }
}
