using System;
using DomainService2.Specs.DomainObjects;
using DomainService2.Workflow.Origination.Credit;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;

namespace DomainService2.Specs.Workflow.Origination.Credit.CreditResubCommandHandlerSpecs
{
    [Subject(typeof(CreditResubCommandHandler))]
    public class When_highest_role_type_is_underwriter : WithFakes
    {
        protected static CreditResubCommand command;
        protected static CreditResubCommandHandler handler;
        protected static IApplicationRepository applicationRepo;
        protected static IApplicationRole underwriterRole;
        protected static IApplicationRoleType underwriterRoleType;
        protected static IApplicationRole leadRole;
        protected static IApplicationRoleType leadRoleType;

        protected static IDomainMessageCollection messages;

        Establish context = () =>
        {
            underwriterRole = An<IApplicationRole>();
            underwriterRoleType = An<IApplicationRoleType>();
            leadRole = An<IApplicationRole>();
            leadRoleType = An<IApplicationRoleType>();

            underwriterRoleType.WhenToldTo(x => x.Key).Return((int)OfferRoleTypes.CreditUnderwriterD);
            underwriterRole.WhenToldTo(x => x.ApplicationRoleType).Return(underwriterRoleType);
            underwriterRole.WhenToldTo(x => x.LegalEntityKey).Return(2);

            leadRoleType.WhenToldTo(x => x.Key).Return((int)OfferRoleTypes.LeadMainApplicant);
            leadRole.WhenToldTo(x => x.ApplicationRoleType).Return(leadRoleType);
            leadRole.WhenToldTo(x => x.LegalEntityKey).Return(1);

            IEventList<IApplicationRole> applicationRoles = new StubEventList<IApplicationRole>();
            applicationRoles.Add(messages, leadRole);
            applicationRoles.Add(messages, underwriterRole);

            applicationRepo = An<IApplicationRepository>();
            applicationRepo.WhenToldTo(x => x.GetApplicationRolesForKey(Param.IsAny<Int32>())).Return(applicationRoles);

            command = new CreditResubCommand(1);
            handler = new CreditResubCommandHandler(applicationRepo);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_create_an_underwriter_role = () =>
        {
            applicationRepo.WasToldTo(x => x.CreateAndSaveApplicationRole_WithoutRules(1, underwriterRoleType.Key, 2));
        };
    }
}