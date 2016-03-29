using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Rules;
using SAHL.Core.Testing;
using SAHL.Services.ApplicationDomain.CommandHandlers;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.ApplicationDomain.Rules;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Specs.CommandHandlersSpec.MakeApplicantAnIncomeContributor
{
    public class when_registering_rules : WithCoreFakes
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
            };

        private Because of = () =>
            {
                handler = new MakeApplicantAnIncomeContributorCommandHandler(applicantManager, unitOfWorkFactory, eventRaiser, domainRuleManager);
            };

        private It should_register_rules = () =>
            {
                domainRuleManager.WasToldTo(x => x.RegisterRule(Param.IsAny<ApplicantCannotBeAnExistingIncomeContributorRule>()));
            };
    }
}