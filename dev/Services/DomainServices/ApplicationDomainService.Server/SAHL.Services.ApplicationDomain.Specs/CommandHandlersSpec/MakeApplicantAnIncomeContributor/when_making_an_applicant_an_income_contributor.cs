using Machine.Specifications;
using Machine.Fakes;
using SAHL.Core.Data;
using SAHL.Core.Testing;
using SAHL.Services.ApplicationDomain.CommandHandlers;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using SAHL.Core.BusinessModel.Enums;
using NSubstitute;
using SAHL.Core.Services;
using SAHL.Core.Rules;
using SAHL.Services.Interfaces.ApplicationDomain.Models;

namespace SAHL.Services.ApplicationDomain.Specs.CommandHandlersSpec.MakeApplicantAnIncomeContributor
{
    public class when_making_an_applicant_an_income_contributor : WithCoreFakes
    {
        private static MakeApplicantAnIncomeContributorCommand command;
        private static MakeApplicantAnIncomeContributorCommandHandler handler;
        private static IDomainRuleManager<ApplicantRoleModel> domainRuleManager;
        private static int applicationRoleKey = 11;

        private static IApplicantManager applicantManager;

        private Establish context = () =>
        {
            applicantManager = An<IApplicantManager>();
            domainRuleManager = An<IDomainRuleManager<ApplicantRoleModel>>();
            unitOfWork = An<IUnitOfWork>();
            unitOfWorkFactory = An<IUnitOfWorkFactory>();
            unitOfWorkFactory.WhenToldTo(x => x.Build()).Return(unitOfWork);
            command = new MakeApplicantAnIncomeContributorCommand(applicationRoleKey);
            handler = new MakeApplicantAnIncomeContributorCommandHandler(applicantManager, unitOfWorkFactory, eventRaiser, domainRuleManager);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, null);
        };

        private It should_add_an_income_contributor_attribute_to_the_application_role = () =>
        {
            applicantManager.WasToldTo(x => x.AddIncomeContributorOfferRoleAttribute(applicationRoleKey));
        };

        private It should_call_complete_for_the_unit_of_work = () =>
        {
            unitOfWork.WasToldTo(x => x.Complete());
        };

        private It should_not_result_in_any_messages = () =>
        {
            messages.AllMessages.Count().Equals(0);
        };

        private It should_raise_the_applicant_made_income_contributor_event = () =>
        {
            eventRaiser.WasToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(), Arg.Is<ApplicantMadeIncomeContributorEvent>(e =>
                e.ApplicationRoleKey == applicationRoleKey),
                command.ApplicationRoleKey,
                (int)GenericKeyType.OfferRole,
                Param.IsAny<IServiceRequestMetadata>()
                ));
        };
    }
}
