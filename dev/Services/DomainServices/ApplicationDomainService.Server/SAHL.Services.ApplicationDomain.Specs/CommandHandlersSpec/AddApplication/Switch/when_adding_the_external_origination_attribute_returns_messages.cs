using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.ApplicationDomain.CommandHandlers;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Specs.CommandHandlersSpec.AddApplication.Switch
{
    public class when_adding_the_external_origination_attribute_returns_messages : WithCoreFakes
    {
        private static AddSwitchApplicationCommand command;
        private static AddSwitchApplicationCommandHandler handler;
        private static IApplicationDataManager applicationDataManager;
        private static SwitchApplicationModel refinanceApplicationModel;
        private static IApplicationManager applicationManager;
        private static int applicationNumber;
        private static ISystemMessageCollection serviceCommandMessages;
        private static IDomainRuleManager<SwitchApplicationModel> ruleContext;

        private Establish context = () =>
        {
            messages = SystemMessageCollection.Empty();
            refinanceApplicationModel = new SwitchApplicationModel(OfferStatus.Open, 1, OriginationSource.Comcorp, 250000, 1000000, 240, 65000, Product.NewVariableLoan, "reference1", 1);
            applicationManager = An<IApplicationManager>();
            applicationDataManager = An<IApplicationDataManager>();
            serviceCommandMessages = SystemMessageCollection.Empty();
            ruleContext = An<IDomainRuleManager<SwitchApplicationModel>>();
            serviceCommandMessages.AddMessage(new SystemMessage("Service Command Router call failed", SystemMessageSeverityEnum.Error));
            //save application
            applicationNumber = 1234;
            applicationManager.WhenToldTo(x => x.SaveApplication(refinanceApplicationModel.ApplicationType, refinanceApplicationModel.ApplicationStatus, Param.IsAny<DateTime>(),
                refinanceApplicationModel.ApplicationSourceKey, Param.IsAny<int>(), OriginationSource.SAHomeLoans, refinanceApplicationModel.Reference, refinanceApplicationModel.ApplicantCount))
                .Return(applicationNumber);
            serviceCommandRouter.WhenToldTo(x => x.HandleCommand(Arg.Any<SetExternalOriginatorAttributeCommand>(), Arg.Any<IServiceRequestMetadata>())).Return(serviceCommandMessages);
            command = new AddSwitchApplicationCommand(refinanceApplicationModel, Guid.NewGuid());
            handler = new AddSwitchApplicationCommandHandler(serviceCommandRouter, applicationDataManager, linkedKeyManager, unitOfWorkFactory, eventRaiser, applicationManager, ruleContext);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_return_the_messages_from_the_SetExternalOriginatorAttributeCommand = () =>
        {
            messages.ErrorMessages().ShouldContain(serviceCommandMessages.ErrorMessages().First());
        };

        private It should_complete_the_unit_of_work = () =>
        {
            unitOfWork.Received().Complete();
        };

        private It should_not_raise_the_event = () =>
        {
            eventRaiser.DidNotReceive().RaiseEvent(Arg.Any<DateTime>(), Arg.Any<IEvent>(), Arg.Any<int>(), Arg.Any<int>(), Arg.Any<IServiceRequestMetadata>());
        };
    }
}