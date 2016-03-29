﻿using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.SystemMessages;
using SAHL.DomainServiceChecks.CheckHandlers;
using SAHL.DomainServiceChecks.Managers.ThirdPartyInvoiceDataManager;
using SAHL.DomainServiceChecks.Managers.X2InstanceDataManager;
using System;
using System.Linq;

namespace SAHL.DomainServiceCheck.Specs.CheckHandlers.RequiresX2Instance
{
    public class when_the_x2_instance_exist : WithFakes
    {
        private static IX2DataManager dataManager;
        private static RequiresX2InstanceHandler handler;
        private static RequiresX2InstanceCommand requiresX2InstanceCommand;
        private static ISystemMessageCollection systemMessages;

        private Establish context = () =>
        {
            systemMessages = SystemMessageCollection.Empty();
            dataManager = An<IX2DataManager>();
            requiresX2InstanceCommand = new RequiresX2InstanceCommand(1234,Guid.NewGuid());
            handler = new RequiresX2InstanceHandler(dataManager);
            dataManager.WhenToldTo(x => x.DoesInstanceIdExist(1234)).Return(true);
        };

        private Because of = () =>
        {
            systemMessages = handler.HandleCheckCommand(requiresX2InstanceCommand);
        };

        private It should_not_return_error_messages = () =>
        {
            systemMessages.HasErrors.ShouldBeFalse();
        };
    }
}