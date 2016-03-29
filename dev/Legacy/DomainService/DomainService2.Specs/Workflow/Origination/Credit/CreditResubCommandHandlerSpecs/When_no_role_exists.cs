using System;
using DomainService2.Specs.DomainObjects;
using DomainService2.Workflow.Origination.Credit;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Specs.Workflow.Origination.Credit.CreditResubCommandHandlerSpecs
{
    [Subject(typeof(CreditResubCommandHandler))]
    public class When_no_role_exists : WithFakes
    {
        protected static CreditResubCommand command;
        protected static CreditResubCommandHandler handler;
        protected static IApplicationRepository applicationRepo;
        protected static IDomainMessageCollection messages;

        Establish context = () =>
        {
            IEventList<IApplicationRole> applicationRoles = new StubEventList<IApplicationRole>();

            applicationRepo = An<IApplicationRepository>();
            applicationRepo.WhenToldTo(x => x.GetApplicationRolesForKey(Param.IsAny<Int32>())).Return(applicationRoles);

            command = new CreditResubCommand(1);
            handler = new CreditResubCommandHandler(applicationRepo);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_not_create_a_role = () =>
        {
            applicationRepo.WasNotToldTo(x => x.CreateAndSaveApplicationRole_WithoutRules(Param.IsAny<Int32>(), Param.IsAny<Int32>(), Param.IsAny<Int32>()));
        };
    }
}