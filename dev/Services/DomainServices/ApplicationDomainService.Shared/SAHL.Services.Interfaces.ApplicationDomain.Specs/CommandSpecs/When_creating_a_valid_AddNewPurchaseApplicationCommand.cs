﻿using Machine.Fakes;
using Machine.Specifications;
using SAHL.DomainServiceChecks.Checks;
using SAHL.Services.Interfaces.ApplicationDomain;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;

namespace SAHL.Services.Interfaces.Application.Specs.Commands
{
    internal class When_creating_a_valid_AddNewPurchaseApplicationCommand
    {
        private static AddNewPurchaseApplicationCommand command;

        private Establish context = () =>
        {
        };

        private Because of = () =>
        {
            command = new AddNewPurchaseApplicationCommand(Param.IsAny<NewPurchaseApplicationModel>(), Param.IsAny<Guid>());
        };

        private It should_implement_the_application_domain_command = () =>
        {
            command.ShouldBeAssignableTo(typeof(IApplicationDomainCommand));
        };
    }
}