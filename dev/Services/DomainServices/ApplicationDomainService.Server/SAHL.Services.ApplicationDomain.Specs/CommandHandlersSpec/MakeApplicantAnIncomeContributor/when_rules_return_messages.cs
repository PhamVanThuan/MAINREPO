using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.ApplicationDomain.CommandHandlers;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Specs.CommandHandlersSpec.MakeApplicantAnIncomeContributor
{
    public class when_rules_return_messages : WithCoreFakes
    {
        private static MakeApplicantAnIncomeContributorCommand command;
        private static MakeApplicantAnIncomeContributorCommandHandler handler;
        private static IDomainRuleManager<ApplicantRoleModel> domainRuleManager;
        private static IApplicantManager applicantManager;

        private Establish context = () =>
        {
            command = new MakeApplicantAnIncomeContributorCommand(1234567);
            applicantManager = An<IApplicantManager>();
            domainRuleManager = An<IDomainRuleManager<ApplicantRoleModel>>();
            domainRuleManager.WhenToldTo(x => x.ExecuteRules(Param.IsAny<ISystemMessageCollection>(), Param.IsAny<ApplicantRoleModel>()))
                .Callback<ISystemMessageCollection>(a => { a.AddMessage(new SystemMessage("The rule failed.", SystemMessageSeverityEnum.Error)); });
            handler = new MakeApplicantAnIncomeContributorCommandHandler(applicantManager, unitOfWorkFactory, eventRaiser, domainRuleManager);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_return_messages = () =>
        {
            messages.ErrorMessages().First().Message.ShouldEqual("The rule failed.");
        };

        private It should_not_make_the_applicant_an_income_contributor = () =>
        {
            applicantManager.WasNotToldTo(x => x.AddIncomeContributorOfferRoleAttribute(Param.IsAny<int>()));
        };

        private It should_not_raise_an_event = () =>
        {
            eventRaiser.WasNotToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(), Param.IsAny<IEvent>(), Param.IsAny<int>(), Param.IsAny<int>(), Param.IsAny<IServiceRequestMetadata>()));
        };
    }
}