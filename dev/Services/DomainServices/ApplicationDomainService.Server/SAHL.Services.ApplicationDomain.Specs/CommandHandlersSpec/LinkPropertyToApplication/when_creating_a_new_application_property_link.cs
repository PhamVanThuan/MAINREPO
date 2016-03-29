using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data;
using SAHL.Core.Events;
using SAHL.Core.Services;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Providers;
using SAHL.Services.ApplicationDomain.CommandHandlers;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Specs.CommandHandlersSpec.LinkPropertyToApplication
{
    public class when_creating_a_new_application_property_link : WithCoreFakes
    {
        private static LinkPropertyToApplicationCommandHandler handler;
        private static LinkPropertyToApplicationCommand command;
        private static LinkPropertyToApplicationCommandModel commandModel;
        private static IApplicationDataManager applicationDataManager;
        private static int applicationNumber;
        private static int propertyKey;

        private Establish context = () =>
        {
            applicationNumber = 1256;
            propertyKey = 24;
            applicationDataManager = An<IApplicationDataManager>();
            commandModel = new LinkPropertyToApplicationCommandModel(applicationNumber, propertyKey);
            command = new LinkPropertyToApplicationCommand(commandModel);
            handler = new LinkPropertyToApplicationCommandHandler(applicationDataManager, unitOfWorkFactory, eventRaiser);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_link_the_property_to_the_application = () =>
        {
            applicationDataManager.WasToldTo(x => x.LinkPropertyToApplication(Arg.Is<int>(y => y == command.ApplicationNumber), Arg.Is<int>(y => y == command.PropertyKey)));
        };

        private It should_raise_the_correct_event = () =>
        {
            eventRaiser.WasToldTo(x => x.RaiseEvent(
                  Param.IsAny<DateTime>()
                , Param.IsAny<PropertyLinkedToApplicationEvent>()
                , Param.IsAny<int>()
                , Param.IsAny<int>()
                , Param.IsAny<IServiceRequestMetadata>()
            ));
        };

        private It should_not_return_any_error_messages = () =>
        {
            messages.ErrorMessages().Count().ShouldEqual(0);
        };
    }
}