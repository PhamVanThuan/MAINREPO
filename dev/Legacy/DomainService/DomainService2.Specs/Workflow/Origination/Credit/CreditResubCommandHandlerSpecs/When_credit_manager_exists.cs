﻿using System;
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
    public class When_credit_manager_exists : WithFakes
    {
        protected static CreditResubCommand command;
        protected static CreditResubCommandHandler handler;
        protected static IApplicationRepository applicationRepo;
        protected static IApplicationRole role;
        protected static IApplicationRoleType managerRoleType;
        protected static IDomainMessageCollection messages;

        Establish context = () =>
        {
            role = An<IApplicationRole>();
            managerRoleType = An<IApplicationRoleType>();

            managerRoleType.WhenToldTo(x => x.Key).Return((int)OfferRoleTypes.CreditManagerD);
            role.WhenToldTo(x => x.ApplicationRoleType).Return(managerRoleType);
            role.WhenToldTo(x => x.LegalEntityKey).Return(1);

            IEventList<IApplicationRole> applicationRoles = new StubEventList<IApplicationRole>();
            applicationRoles.Add(messages, role);

            applicationRepo = An<IApplicationRepository>();
            applicationRepo.WhenToldTo(x => x.GetApplicationRolesForKey(Param.IsAny<Int32>())).Return(applicationRoles);

            command = new CreditResubCommand(1);
            handler = new CreditResubCommandHandler(applicationRepo);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_create_a_role = () =>
        {
            applicationRepo.WasToldTo(x => x.CreateAndSaveApplicationRole_WithoutRules(1, managerRoleType.Key, 1));
        };
    }
}